using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameNetworkManager : NetworkManager
{
    string playerMaterial;

    public Timer timer;

    public int totalPlayers = 0;

    public int maxPlayers = 1;
    int playerMaterialIndex;

    [Server]
    private void UpdateMyInt() { 
        playerMaterialIndex = totalPlayers;
     }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);
        GameObject player;
        GameObject powerUp;
        

        if(totalPlayers <=4)
        {
            
            player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player.GetComponent<Player>().playerMaterialIndex = totalPlayers;
            NetworkServer.AddPlayerForConnection(conn,player);
            totalPlayers++; 
            //test
            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned");
        }
        if(totalPlayers == maxPlayers){
        

            timer.startFunc();
        }

    }
    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Connected to Server!");
        
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected from Server!");
    }

 
}