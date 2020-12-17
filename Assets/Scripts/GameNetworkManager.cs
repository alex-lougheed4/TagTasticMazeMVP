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
    GameObject tag;


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
            //player.name += totalPlayers;
            //Texture s = Resources.Load<Texture>("playerTextures/Player"+ totalPlayers + "_untagged");
            player.GetComponent<Player>().setTextureValue(totalPlayers);
            NetworkServer.AddPlayerForConnection(conn,player);

            //test
            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned");
        }
        if(totalPlayers == maxPlayers){
            Debug.Log("Total = Max");
            tag = Instantiate(Resources.Load("Prefabs/Tag")) as GameObject;
            NetworkServer.Spawn(tag);
            tag.GetComponent<TagSpawn>().spawnTag();
            timer.startcountDownFunc();
            //timer.timerIsRunning = true;
            Debug.Log("Timer started");
            if (timer.getTimeRemaining() == 0.0f){
                timer.timerIsRunning = false;
                timer.timerLabel.text = "Game Over";
            }
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