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

    public int maxPlayers;
    int playerMaterialIndex;

    int mazeSeed;
    public GameObject MazeLoader;


    [Server]
    private void UpdateMyInt() { 
        playerMaterialIndex = totalPlayers;
     }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);
        GameObject player;
        GameObject powerUp;
        

        if(totalPlayers <=maxPlayers)
        {   
            totalPlayers++; 
            player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player.name += totalPlayers;
            //Texture s = Resources.Load<Texture>("playerTextures/Player"+ totalPlayers + "_untagged");
            player.GetComponent<Player>().setTextureValue(totalPlayers);
            NetworkServer.AddPlayerForConnection(conn,player);

            if (totalPlayers > 1 && totalPlayers <= maxPlayers){
                MazeLoader.GetComponent<MazeLoader>().createMazeClient();
            }
            
            //test
            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned");
        }
        if(totalPlayers == maxPlayers){
        
            timer.startcountDownFunc();
            Debug.Log("Timer started");
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