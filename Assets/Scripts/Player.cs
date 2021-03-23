using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class contains all information about the player and is connected to the prefab, this handles player controls, textures etc </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>
public class Player : NetworkBehaviour{
    
    public Texture[] untaggedPlayerTextures = null; //list of untagged textures 
    public Texture[] taggedPlayerTextures = null; //list of tagged textures

    Texture thisPlayerUntaggedTexture;
    Texture thisPlayerTaggedTexture; 

    public float speed; //player's speed multiplier (powerup)
    
	[SyncVar(hook = nameof(OnTagChanged))] //synced value to affect  OnTagChanged when changed
    bool hasTag = false; //boolean of if the player has the tag

    bool justTagged; //boolean value of if the player was just tagged
    float randomPosX; //random spawn position of player in x axis
    float randomPosZ; //random spawn position of player in z axis 

    //int wallBreakCount = 0;
    
    //bool thisPlayerHasPowerup = false; //boolean of have powerup
    
    string powerUpType;

    public GameObject mazeLoader;

    private GameNetworkManager room;
    /**private GameNetworkManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as GameNetworkManager;
        }
    }
    **/
 
    public void OnTagChanged(bool _, bool nowHasTag) //function called whenever OnTagChanged is used
	{
        if(nowHasTag) //if they now have the tag
            GetComponent<MeshRenderer>().material.mainTexture = thisPlayerTaggedTexture; //set their texture to the tagged version
		else
	        GetComponent<MeshRenderer>().material.mainTexture = thisPlayerUntaggedTexture; //set their texture to the untagged version
    }
    
    [SyncVar (hook = nameof(OnTextureValueChange))] protected int textureValue; //syncvar of the texture value changing for each player being added to the server

    public void OnTextureValueChange(int old, int new_) // attached to textureValue and OnTexture Value Change
    {
        thisPlayerUntaggedTexture = untaggedPlayerTextures[textureValue-1]; 
        thisPlayerTaggedTexture = taggedPlayerTextures[textureValue-1];
        GetComponent<MeshRenderer>().material.mainTexture = thisPlayerUntaggedTexture; 
    }

	// This is no longer needed in this script - not sure if you're calling it
	// server side or just setting textureValue above directly when spawning
    public void setTextureValue(int value)
    {
        textureValue = value;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        mazeLoader = FindObjectOfType<MazeLoader>().gameObject;
    }

    public override void OnStartClient() // is this needed?
    {
        base.OnStartClient();
        mazeLoader = FindObjectOfType<MazeLoader>().gameObject;
        //Room.playersList.Add(this);
    }
    public override void OnStartLocalPlayer()
    {
        mazeLoader = FindObjectOfType<MazeLoader>().gameObject;
        generateRandomPositions();
        if((randomPosX != 0) || (randomPosZ != 0)){
            transform.position = new Vector3(randomPosX,0,randomPosZ);
        }

        Camera.main.GetComponent<CameraFollow>().target=transform; //Fix camera on "me"
    }

    public void generateRandomPositions(){
        randomPosX = (float)Random.Range(-15f, 15f);
        randomPosZ = (float)Random.Range(-15f, 15f);
    }

    void HandleMovement()
	{
        if(isLocalPlayer)
		{   if(powerUpType == "speedUp"){
            speed = 0.025f;
        }
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * speed,0, moveVertical * speed);
            transform.position = transform.position + movement;

        }
    }

    void Update()
	{
        if (!hasAuthority){
            return;
        } 
        HandleMovement();
 /**       if(hasAuthority){ //checks if what it's running on has authority ie is the server
            if(Room.getHasGameEnded()){ //checks if the game has ended bool in the room is true
               for (Player player : Room.playersList){
                   if(player.hasTag){
                       //call leaderboard push function
                   }
               }
            }
            
        }**/

    }

    [Server]
    void pushWinnerToLeaderboaord(){

    }

    
	[ServerCallback]
    void OnCollisionEnter(Collision collisionInfo)
	{
        if(collisionInfo.collider.tag == "Tag")
		{    
            Debug.Log("Collision with Tag Occured");
            NetworkServer.Destroy(collisionInfo.gameObject);
            hasTag = true;
			return;
        }
        if(collisionInfo.collider.tag == "powerup")
		{    
            Debug.Log("Collision with powerup Occured");
            powerUpType = collisionInfo.gameObject.GetComponent<Powerup>().choosePowerUp(); //get access to the type of power up through powerup.choosePowerUp(); and set to string
            Debug.Log(powerUpType);
            NetworkServer.Destroy(collisionInfo.gameObject);
            
            
            //thisPlayerHasPowerup = true;
			return;
        }
        if((collisionInfo.collider.tag == "WallTag")){  //change HasBreakWallPowerUp to powerUpType == "breakWall"
            Debug.Log("Collided with wall");
            if(powerUpType == "breakWall"){ //could be constantly touching the wall hence the problem
                Debug.Log("Breaking Wall...");
                Debug.Log(collisionInfo.collider.gameObject.name);
                
                int indexOfWall = mazeLoader.GetComponent<MazeLoader>().mazeWalls.IndexOf(collisionInfo.collider.gameObject.transform);
                Debug.Log(indexOfWall);
                wallDestroy(indexOfWall);
                Destroy(mazeLoader.GetComponent<MazeLoader>().mazeWalls[indexOfWall].gameObject); //wall not breaking 17/03/21
                Debug.Log("Server Wall Broken: " + indexOfWall);
                powerUpType = "";
                
            }
        }

        if(hasTag && !justTagged && collisionInfo.collider.tag == "Player")
		{
            Debug.Log("Collision with Player occured");
            Debug.Log("if i am tagged player");
            collisionInfo.gameObject.GetComponent<Player>().hasTag = true;
			hasTag = false;
			justTagged = true;
            }
        }

    [ClientRpc]
    void wallDestroy(int wallIndex){
        if (isClientOnly ){
            Debug.Log(wallIndex);
            Destroy(mazeLoader.GetComponent<MazeLoader>().mazeWalls[wallIndex].gameObject);
            Debug.Log("Client Wall Broken: " + wallIndex);
        }
    }
    

    void OnCollisionExit(Collision collisionInfo)
	{
		if(hasTag && justTagged && collisionInfo.collider.tag == "Player")
			justTagged = false;
	}
}


