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
    [Range(1,200)]//Only allow ranges from 1,200 can be changed
    private int mazeSizeX = 16;//How many cells in the x axis <--Edited in unity by using slider where script is attached to

    [SerializeField]
    [Range(1,200)]//Only allow ranges from 1,200 can be changed
    private int mazeSizeY = 16;//How many cells in the y axis <--Edited in unity by using slider where script is attached to

    public GameObject tag;


    [SerializeField]
    private Transform wallPrefab = null;


    [SyncVar(hook = nameof(createMaze))] int mazeSeed = -1;

    // Start is called before the first frame update
    
void createMaze(int oldseed, int newseed)
{
        Random.InitState(newseed); //applies the set seed
        Prims prims = new Prims(mazeSizeX,mazeSizeY); //Create Prims Object      
        prims.loop(); //Run Prims Algorithm to Completition
        //prims.draw(); <-- String Based UI
        var maze = prims.getMaze(); //Retrieve Maze Information
        Draw(maze); //Graphical UI
        

    }

    public override void OnStartServer()
    {
        Debug.Log("Maze Created"); //debug statement
        mazeSeed = Random.Range(0,255); //sets maze seed
        createMaze(0, mazeSeed); //creates the maze with seed
        tag = Instantiate(Resources.Load("Prefabs/Tag")) as GameObject; //instantiates (creates) tag
        NetworkServer.Spawn(tag); //spawns the tag
        tag.GetComponent<TagSpawn>().spawnTag(); //randomises the tag's position
    }



    private void Draw(Cell[,] maze)
    {

        for (int j = 0; j < mazeSizeY; j++)
        {
            for (int i = 0; i < mazeSizeX; i++)
            {
                var cell = maze[j, i];
                var position = new Vector3(-15+ i, 0, 15 - j);

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
                if (maze[j, i].getRight() || i==mazeSizeX - 1)
                {
                    var rightWall = Instantiate(wallPrefab, transform) as Transform;
                    rightWall.position = position + new Vector3(+size / 2, 0, 0);
                    rightWall.localScale = new Vector3(size, rightWall.localScale.y, rightWall.localScale.z);
                    rightWall.eulerAngles = new Vector3(0, 90, 0);

                }
                if (maze[j, i].getBottom() || j==mazeSizeY - 1)
                {
                    var bottomWall = Instantiate(wallPrefab, transform) as Transform;
                    bottomWall.position = position + new Vector3(0, 0, -size / 2);
                    bottomWall.localScale = new Vector3(size, bottomWall.localScale.y, bottomWall.localScale.z);
                }
            }
        }
    }
}