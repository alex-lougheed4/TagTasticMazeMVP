using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TagSpawn : MonoBehaviour
{
    
    public void spawnTag(){
        float randomPosX = (float)Random.Range(-15f, 15f); //creates random position on x axis
        float randomPosZ = (float)Random.Range(-15f, 15f); //creates random position on z axis 
        transform.position = new Vector3(0,0,0); //sets position to default 0,0,0 replace with randomPos' when needed
    }

}
