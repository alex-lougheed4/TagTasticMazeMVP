﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
    
    public Material thisPlayerMat;

    public MeshRenderer meshRenderer;
    public Material taggedMaterial;
    public Material untaggedMaterial;

    int playerMaterialStringRequestCount = 1;

    string[] playerMaterialStrings = {"Player1Untagged","Player2Untagged","Player3Untagged","Player4Untagged"}; 
    string[] playerTaggedMaterialStrings = {"Player1Tagged","Player2Tagged","Player3Tagged","Player4Tagged"}; 
    
    string thisPlayerUntaggedMaterialString;
    string thisPlayerTaggedMaterialString;


    


    bool hasTag = false;


    public override void OnStartLocalPlayer()
    {
        requestThisPlayerMaterialString();
        setPlayerMaterial();

        meshRenderer.material = thisPlayerMat;
        untaggedMaterial = meshRenderer.material;
        taggedMaterial = Resources.Load<Material>(thisPlayerTaggedMaterialString);


        float randomPosX = (float)Random.Range(-15f, 15f);
        float randomPosZ = (float)Random.Range(-15f, 15f);
        transform.position = new Vector3(randomPosX,0,randomPosZ);

        Camera.main.GetComponent<CameraFollow>().target=transform; //Fix camera on "me"
  

    }
    void HandleMovement(){
        if(isLocalPlayer){
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * 0.1f,0, moveVertical * 0.1f);
            transform.position = transform.position + movement;

        }
    }

    void Update(){
        HandleMovement();
        if (hasTag)
        {
            meshRenderer.material = taggedMaterial;
        }
        
    }

//only calls on server so people can't call this function from a client themselves
    void onCollisionEnter(Collision collisionInfo){
        if(collisionInfo.collider.tag == "Tag"){
            Debug.Log("collision");
            Destroy(collisionInfo.gameObject);
            updateTaggedState();
        }
        if(collisionInfo.collider.tag == "Player"){
            Debug.Log("collision");
            Destroy(collisionInfo.gameObject);
            updateTaggedState();
        }
    }

    public void updateTaggedState(){
        hasTag = !hasTag;
    }

    [Command]
    void requestThisPlayerMaterialString(){
        Debug.Log("Recieved playerMaterial request from client");
        getThisPlayerMaterialString();


    }

    [TargetRpc]
    void getThisPlayerMaterialString(){
        thisPlayerUntaggedMaterialString = playerMaterialStrings[playerMaterialStringRequestCount];
        thisPlayerTaggedMaterialString = playerTaggedMaterialStrings[playerMaterialStringRequestCount];

        playerMaterialStringRequestCount++;
        Debug.Log("Recieved playerMaterial string from server");
    }

    void setPlayerMaterial(){
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        thisPlayerMat = Resources.Load<Material>(thisPlayerUntaggedMaterialString);
        Debug.Log(thisPlayerUntaggedMaterialString);

    }
}
