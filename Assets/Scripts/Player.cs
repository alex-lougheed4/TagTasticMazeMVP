using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
    
  

    public Texture[] untaggedPlayerTextures = null;
    public Texture[] taggedPlayerTextures = null;

    Texture thisPlayerUntaggedTexture;
    Texture thisPlayerTaggedTexture;
    
 
    bool hasTag = false;

    [SyncVar (hook = nameof(OnTextureValueChange))] protected int textureValue;

    public void OnTextureValueChange(int old, int new_)
    {
        thisPlayerUntaggedTexture = untaggedPlayerTextures[textureValue-1];
        thisPlayerTaggedTexture = taggedPlayerTextures[textureValue-1];
        GetComponent<MeshRenderer>().material.mainTexture = untaggedPlayerTextures[textureValue-1]; 
    }
    public void setTextureValue(int value)
    {
        textureValue = value;
    }

    
 



    public override void OnStartLocalPlayer()
    {

        
        
        
        float randomPosX = (float)Random.Range(-15f, 15f);
        float randomPosZ = (float)Random.Range(-15f, 15f);
        transform.position = new Vector3(randomPosX,0,randomPosZ);

        Camera.main.GetComponent<CameraFollow>().target=transform; //Fix camera on "me"
  

    }
    void HandleMovement(){
        if(isLocalPlayer){
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal * 0.05f,0, moveVertical * 0.05f);
            transform.position = transform.position + movement;

        }
    }

    void Update(){
        HandleMovement();

        
    }


    public void updateTaggedState(){
        hasTag = !hasTag;
        UpdatePlayerTexture();
    }

    public void UpdatePlayerTexture(){
        if (GetComponent<MeshRenderer>().material.mainTexture ==thisPlayerUntaggedTexture){
            GetComponent<MeshRenderer>().material.mainTexture = thisPlayerTaggedTexture;
        }
        else if (GetComponent<MeshRenderer>().material.mainTexture == thisPlayerTaggedTexture){
            GetComponent<MeshRenderer>().material.mainTexture = thisPlayerUntaggedTexture;
        }
        
    }
    

    public bool returnHasTag(){
        return hasTag;
    }

}

