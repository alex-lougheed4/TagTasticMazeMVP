using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{
    
    string[] playerMaterialStrings = {"Player1Untagged","Player2Untagged","Player3Untagged","Player4Untagged"}; 
    string[] playerTaggedMaterialStrings = {"Player1Tagged","Player2Tagged","Player3Tagged","Player4Tagged"}; 

    
    


    public MeshRenderer meshRenderer;
    public Material taggedMaterial;
    public Material untaggedMaterial;
    string thisPlayerUntaggedMaterialString;
    string thisPlayerTaggedMaterialString;
    bool hasTag = false;

    
    [SyncVar(hook = nameof(MyHook))] public int playerMaterialIndex = -1;
    private void MyHook(int oldPlayerMaterialIndex, int newPlayerMaterialIndex) { 
        Debug.Log(newPlayerMaterialIndex + "alpha");
        thisPlayerUntaggedMaterialString = playerMaterialStrings[newPlayerMaterialIndex];
        thisPlayerTaggedMaterialString = playerTaggedMaterialStrings[newPlayerMaterialIndex];

        untaggedMaterial = Resources.Load<Material>("Materials/" + thisPlayerUntaggedMaterialString);
        taggedMaterial = Resources.Load<Material>("Materials/" + thisPlayerTaggedMaterialString);

        meshRenderer.material = untaggedMaterial; //for some reason doesn't work for player 1 but player 1 can use the default player material anway since it's assigned to player 1 naturally


     }

    public override void OnStartLocalPlayer()
    {

        
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        
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

        
    }

//only calls on server so people can't call this function from a client themselves
/**    void onCollisionEnter(Collision collisionInfo){
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
    **/

    public void updateTaggedState(){
        hasTag = !hasTag;
        UpdatePlayerMaterial();
    }

    public void UpdatePlayerMaterial(){

        if(meshRenderer.material = Resources.Load<Material>("Materials/Player1Untagged")){          //taggedmaterial was null for player1's material so this method used as a quick fix
            meshRenderer.material = Resources.Load<Material>("Materials/Player1Tagged");
        }
        else if (meshRenderer.material = Resources.Load<Material>("Materials/Player1Tagged")){
            meshRenderer.material = Resources.Load<Material>("Materials/Player1Untagged");
        }
        else if(meshRenderer.material = untaggedMaterial){
            meshRenderer.material = taggedMaterial;
        }
         else if (meshRenderer.material = taggedMaterial){
            meshRenderer.material = untaggedMaterial;
        }
        
    }

    public bool returnHasTag(){
        return hasTag;
    }

}

