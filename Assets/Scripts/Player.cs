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
    bool thisPlayerHasPowerup = false; //boolean of have powerup
    bool HasBreakWallPowerUp = false;

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

    public override void OnStartLocalPlayer()
    {

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
		{   if(thisPlayerHasPowerup){
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
        HandleMovement();

        if(Input.GetKeyDown("escape")){ //close game if escape is pressed, could do with a canvas to check if they are sure they want to quit over the game 
            Application.Quit();
        }
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
            NetworkServer.Destroy(collisionInfo.gameObject);
            thisPlayerHasPowerup = true;
			return;
        }
        //if((collisionInfo.collider.tag == "wallTag") && (HasBreakWallPowerUp)){              EDIT POWERUP SAVE STATUS TO HAS POWER UP TYPE TO CHECK BEFORE BREAKING WALL
        //    Debug.Log("Collided with wall");
        //    NetworkServer.Destroy(collisionInfo.gameObject);


        //}

        if(hasTag && !justTagged && collisionInfo.collider.tag == "Player")
		{
            Debug.Log("Collision with Player occured");
            Debug.Log("if i am tagged player");
            collisionInfo.gameObject.GetComponent<Player>().hasTag = true;
			hasTag = false;
			justTagged = true;
            }
        }
    

	[ServerCallback]
    void OnCollisionExit(Collision collisionInfo)
	{
		if(hasTag && justTagged && collisionInfo.collider.tag == "Player")
			justTagged = false;
	}
}
