using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MazeLoader : NetworkBehaviour
{
    [SerializeField]
    [Range(1,50)]
    private int width =10;

    [SerializeField]
    private float size = 1f;

    [SerializeField]
    [Range(1,50)]
    private int height = 10;

    [SerializeField]
    private Transform wallPrefab = null;

    [SyncVar] int mazeSeed;

    // Start is called before the first frame update
    void createMaze(int seed){
        Random.InitState(seed);
        Prims prims = new Prims(Random.Range(0,15),Random.Range(0,15)); //Create Prims Object      
        prims.loop(); //Run Prims Algorithm to Completition
        //prims.draw(); <-- String Based UI
        var maze = prims.getMaze(); //Retrieve Maze Information
        Draw(maze); //Graphical UI

    }
    public void createMazeServer(){
        createMaze(mazeSeed);
    }

    [ClientRpc]
    public void createMazeClient(){
        createMaze(mazeSeed);
    }

    public override void OnStartServer()
    {
        Debug.Log("Maze Created");
        mazeSeed = Random.Range(0,255);
        createMazeServer();
    }



    private void Draw(Cell[,] maze)
    {
        //for (int y = 0; y < 2; y++)
        //{
        //    for (int x = 0; x < 10; x++)
        //    {
        //        var position = new Vector3(-width / 2 +y, 0, -height / 2 + x);//y is across x is up (when - width and height)

        //        var topWall = Instantiate(wallPrefab, transform) as Transform;
        //        topWall.position = position + new Vector3(0, 0, size / 2);

        //        var leftWall = Instantiate(wallPrefab, transform) as Transform;
        //        leftWall.position = position + new Vector3(-size / 2, 0, 0);
        //        leftWall.eulerAngles = new Vector3(0, 90, 0);

        //        var rightWall = Instantiate(wallPrefab, transform) as Transform;
        //        rightWall.position = position + new Vector3(size / 2, 0, 0);
        //        rightWall.eulerAngles = new Vector3(0, 90, 0);

        //        var bottomWall = Instantiate(wallPrefab, transform) as Transform;
        //        bottomWall.position = position + new Vector3(0, 0, -size / 2);
        //    }
        //}
        for (int j = 0; j < 16; j++)
        {
            for (int i = 0; i < 16; i++)
            {
                var cell = maze[j, i];
                var position = new Vector3(-width / 2 + i, 0, -height / 2 - j);

                if (j == 0)
                {
                    var topWall = Instantiate(wallPrefab, transform) as Transform;
                    topWall.position = position + new Vector3(0, 0, size / 2);
                    topWall.localScale = new Vector3(size, topWall.localScale.y, topWall.localScale.z);

                }
                if (i == 0)
                {
                    var leftWall = Instantiate(wallPrefab, transform) as Transform;
                    leftWall.position = position + new Vector3(-size / 2, 0, 0);
                    leftWall.localScale = new Vector3(size, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }
                if (maze[j, i].getRight() || i==15)
                {
                    var rightWall = Instantiate(wallPrefab, transform) as Transform;
                    rightWall.position = position + new Vector3(+size / 2, 0, 0);
                    rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                    rightWall.eulerAngles = new Vector3(0, 90, 0);

                }
                if (maze[j, i].getBottom() || j==15)
                {
                    var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                    bottomWall.position = position + new Vector3(0, 0, -size / 2);
                    bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
