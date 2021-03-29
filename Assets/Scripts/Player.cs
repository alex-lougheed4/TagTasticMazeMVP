using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>This class contains all information about the player and is connected to the prefab, this handles player controls, textures etc </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>
public class Player : NetworkBehaviour{
    
    public Texture[] untaggedPlayerTextures = null; //list of untagged textures 
    public Texture[] taggedPlayerTextures = null; //list of tagged textures

    public Sprite[] powerUpImages = null;

    Texture thisPlayerUntaggedTexture;
    Texture thisPlayerTaggedTexture; 

    Texture thisPowerUpImage;

    [SyncVar] public float speed = 0.0f; //player's speed multiplier (powerup)
    
	[SyncVar(hook = nameof(OnTagChanged))] //synced value to affect  OnTagChanged when changed
    public bool hasTag = false; //boolean of if the player has the tag

    bool justTagged; //boolean value of if the player was just tagged
    
    [SyncVar] string powerUpType;

    [SyncVar] public string username;

    public GameObject mazeLoader;
    public bool winnerUploaded = false;

    Timer playerTimer;
    Image playerPowerUpImage;

    [SyncVar]public GameObject playerRadarIndicator;
    Vector3 playerRadarIndicatorPos;
    Vector3 playerRadarIndicatorPosOffset = new Vector3(0,16,0);
    

    private GameNetworkManager room;
        private GameNetworkManager Room
        {
            get
            {
                if (room != null) { return room; }
                return room = NetworkManager.singleton as GameNetworkManager;
            }
        }
    
 
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
        Room.playersList.Add(this);
    }

    public override void OnStartClient() 
    {
        base.OnStartClient();
        this.enabled = true;
        mazeLoader = FindObjectOfType<MazeLoader>().gameObject;
        playerPowerUpImage = (Image)GameObject.FindObjectOfType<GameCanvasScript>().powerUpImage;
        playerTimer = GameObject.FindObjectOfType<Timer>();
        username = PlayerPrefs.GetString("username");
        Room.playersList.Add(this);
        updatePowerupImage();
        //speed = 0.00f;
    }
    public override void OnStartLocalPlayer()
    {
        mazeLoader = FindObjectOfType<MazeLoader>().gameObject;
        Camera.main.GetComponent<CameraFollow>().target=transform; //Fix camera on "me"
        Debug.Log(speed);
    }


    void HandleMovement()
	{
        if(isLocalPlayer){
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * speed,0, moveVertical * speed);
            transform.position = transform.position + movement;

        }
    }

    [Client]
    void updatePowerupImage(){
       if(powerUpType == "speedUp"){
            playerPowerUpImage.sprite = powerUpImages[0];
            playerPowerUpImage.gameObject.SetActive(true);
       }
       else if(powerUpType == "breakWall"){
            playerPowerUpImage.sprite = powerUpImages[1];
            playerPowerUpImage.gameObject.SetActive(true);           
       }
       else{
           playerPowerUpImage.sprite = null;
           playerPowerUpImage.gameObject.SetActive(false);
       }

    }

    void Update()
	{
        if(playerRadarIndicator != null){
            playerRadarIndicatorPos = transform.position + playerRadarIndicatorPosOffset;
            playerRadarIndicator.transform.position = playerRadarIndicatorPos;
        
            if(isLocalPlayer){
                updatePowerupImage();
                playerRadarIndicator.GetComponent<Renderer>().material.color = Color.green;
            }
            else{
                playerRadarIndicator.GetComponent<Renderer>().material.color = Color.red;
            }
        }
          
        if (!hasAuthority){
            return;
        } 
        HandleMovement();
        /**if(hasAuthority){ //checks if what it's running on has authority ie is the server
            Debug.Log("If is Server");
            if((Room.getHasGameEnded())&& (!winnerUploaded)){ //checks if the game has ended bool in the room is true
                Debug.Log("Game ended, push to clients");
                endGameForClients();
               foreach (Player p in Room.playersList){
                    if(p.hasTag){
                        pushWinnerToLeaderboaord(p.username); //call leaderboard push function
                        Debug.Log("winner's username: " + p.username);
                        winnerUploaded = true;
                    }
                }
            }
        
        }**/

    }


    [ClientRpc]
    public void endGameForClients(){
        playerTimer.timerLabel.text = "Game Over";
        Time.timeScale = 0.0f; 
        if(hasTag){
            pushWinnerToLeaderboaord(username);
        }
    }

    public void pushWinnerToLeaderboaord(string userName){
        FindObjectOfType<LeaderboardScript>().UploadWinner(userName);
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
            powerUpType = collisionInfo.gameObject.GetComponent<Powerup>().choosePowerUp();
            Debug.Log(powerUpType);
            NetworkServer.Destroy(collisionInfo.gameObject);
            if(powerUpType == "speedUp"){
                speed = 0.015f;
            }
			return;
        }
        if(collisionInfo.collider.tag == "WallTag"){ 
            Debug.Log("Collided with wall");
            if(powerUpType == "breakWall"){ 
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
        if (isClientOnly){ //could be the cause of host wall not destroying 
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