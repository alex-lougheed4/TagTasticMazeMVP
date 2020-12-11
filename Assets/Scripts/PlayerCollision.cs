using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : NetworkBehaviour
{
    public Player thisPlayer;
 void OnCollisionEnter(Collision collisionInfo){
     if(collisionInfo.collider.tag == "Tag"){
         Debug.Log("Collision Occured");
         Destroy(collisionInfo.gameObject);
         thisPlayer.updateTaggedState();
     }
 }


}
