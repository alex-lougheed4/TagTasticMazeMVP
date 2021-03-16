using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

///<summary>This class refers to the tag and controls the tag's spawn position which can be randomized  </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

public class TagSpawn : MonoBehaviour
{
    
    public void spawnTag(){
        float randomPosX = (float)Random.Range(-15f, 15f); //creates random position on x axis
        float randomPosZ = (float)Random.Range(-15f, 15f); //creates random position on z axis 
        transform.position = new Vector3(0,0,0); //sets position to default 0,0,0 replace with randomPos' when needed
    }

}
