using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{

    SyncList<string> playerMats = new SyncList<string>();
    SyncList<string> taggedPlayerMats = new SyncList<string>();
    [SyncVar] int index = 0;
    
    string thisPlayerMat;

    public MeshRenderer meshRenderer;
    public Material taggedMaterial;


    bool hasTag = false;

    public override void OnStartLocalPlayer()
{
        float randomPosX = (float)Random.Range(-15f, 15f);
        float randomPosZ = (float)Random.Range(-15f, 15f);
        transform.position = new Vector3(randomPosX,0,randomPosZ);

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();


        playerMats.Add("Player1Untagged");
        playerMats.Add("Player2Untagged");
        playerMats.Add("Player3Untagged");
        playerMats.Add("Player4Untagged");

        taggedPlayerMats.Add("Player1Tagged");
        taggedPlayerMats.Add("Player2Tagged");
        taggedPlayerMats.Add("Player3Tagged");
        taggedPlayerMats.Add("Player4Tagged");

        

    thisPlayerMat = playerMats[index];
    index++;
    meshRenderer.material = Resources.Load<Material>(thisPlayerMat); 
    taggedMaterial = Resources.Load<Material>(taggedPlayerMats[index]);
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

    void onCollisionEnter(Collision collisionInfo){
        if(collisionInfo.collider.tag == "Tag"){
            Debug.Log("collision");
            Destroy(collisionInfo.gameObject);
            updateTaggedState();
        }
    }

    public void updateTaggedState(){
        hasTag = !hasTag;
    }


    



}

