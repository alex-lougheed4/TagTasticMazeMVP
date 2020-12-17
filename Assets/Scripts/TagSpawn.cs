using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TagSpawn : MonoBehaviour
{
    
    public void spawnTag(){
        float randomPosX = (float)Random.Range(-15f, 15f);
        float randomPosZ = (float)Random.Range(-15f, 15f);
        transform.position = new Vector3(0,0,0);
    }

}
