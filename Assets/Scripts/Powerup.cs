using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

///<summary>This class refers to the powerup object and it's various attributes e.g the type of powerup </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

public class Powerup : NetworkBehaviour
{
    public GameObject[] possiblePowerupSpawnpoints = new GameObject[4];
    
    string[] possiblePowerUps = {"speedUp", "breakWall"};
    //string[] possiblePowerUps = {"breakWall"}; //testing

    [SyncVar] public Vector3 currentPosition = Vector3.zero; 
 
    string thisPowerUp;
    
    [Server]
    public void spawnPowerUp(){
        possiblePowerupSpawnpoints =  new[] {GameObject.Find("powerUpSpawnPoint1"),GameObject.Find("powerUpSpawnPoint2"),GameObject.Find("powerUpSpawnPoint3"),GameObject.Find("powerUpSpawnPoint4")};
        int spawnPositionValue = Random.Range(0,possiblePowerupSpawnpoints.Length);
        transform.position = possiblePowerupSpawnpoints[spawnPositionValue].transform.position;
    }

    [ClientRpc]
    void clientSpawnPowerUp(GameObject[] possiblePowerupSpawnpoints , int spawnPositionValue){
        transform.position = possiblePowerupSpawnpoints[spawnPositionValue].transform.position;
    }
    
    public string choosePowerUp(){
        int value = Random.Range(0, possiblePowerUps.Length);
        thisPowerUp = possiblePowerUps[value];
        Debug.Log(thisPowerUp);
        return thisPowerUp;
    }

}
