using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Powerup : NetworkBehaviour
{
    public GameObject[] possiblePowerupSpawnpoints = new GameObject[4];
    
    string[] possiblePowerUps = {"speedUp"};
    
    string thisPowerUp;


    public void spawnPowerUp(){
        // maybe add if isn't local player
        possiblePowerupSpawnpoints =  new[] {GameObject.Find("powerUpSpawnPoint1"),GameObject.Find("powerUpSpawnPoint2"),GameObject.Find("powerUpSpawnPoint3"),GameObject.Find("powerUpSpawnPoint4")};
        int spawnPositionValue = Random.Range(0,possiblePowerupSpawnpoints.Length);
        transform.position = possiblePowerupSpawnpoints[spawnPositionValue].transform.position;
    }
    
    void choosePowerUp(){
        int value = Random.Range(0, possiblePowerUps.Length);
        thisPowerUp = possiblePowerUps[value];
    }




}
