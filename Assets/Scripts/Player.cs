using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour{

    bool hasTag = false;
    

    public override void OnStartLocalPlayer()
{
    
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

