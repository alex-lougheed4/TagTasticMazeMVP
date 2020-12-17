using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
    
    public Texture[] untaggedPlayerTextures = null;
    public Texture[] taggedPlayerTextures = null;

    Texture thisPlayerUntaggedTexture;
    Texture thisPlayerTaggedTexture;

    public float speed;
    
	[SyncVar(hook = nameof(OnTagChanged))]
    bool hasTag = false;

    bool justTagged;
    float randomPosX;
    float randomPosZ;
    bool thisPlayerHasPowerup = false;

    public void OnTagChanged(bool _, bool nowHasTag)
	{
        if(nowHasTag)
            GetComponent<MeshRenderer>().material.mainTexture = thisPlayerTaggedTexture;
		else
	        GetComponent<MeshRenderer>().material.mainTexture = thisPlayerUntaggedTexture;
    }
    
    [SyncVar (hook = nameof(OnTextureValueChange))] protected int textureValue;

    public void OnTextureValueChange(int old, int new_)
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
