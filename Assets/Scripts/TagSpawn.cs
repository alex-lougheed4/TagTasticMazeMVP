using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TagSpawn : MonoBehaviour
{
    void spawnTag(){
        float randomPosX = (float)Random.Range(-15f, 15f);
        float randomPosZ = (float)Random.Range(-15f, 15f);
        transform.position = new Vector3(randomPosX,0,randomPosZ);
    }

}
