using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : NetworkBehaviour
{
    public Player thisPlayer;
    Player thatPlayer;
 void OnCollisionEnter(Collision collisionInfo){
    if(collisionInfo.collider.tag == "Tag"){
        Debug.Log("Collision Occured");
        Destroy(collisionInfo.gameObject);
        thisPlayer.updateTaggedState();
    }
    if(collisionInfo.collider.tag == "Player"){ //check if either player has tag first
    thatPlayer = collisionInfo.gameObject.GetComponent<Player>();
        if (((thisPlayer.returnHasTag()==true) && (thatPlayer.returnHasTag()==false)) || ((thisPlayer.returnHasTag()==false) && (thatPlayer.returnHasTag()==true))){
            Debug.Log("collision");
            thisPlayer.updateTaggedState();
            thatPlayer.updateTaggedState();
        }
    }
     
 }
}
