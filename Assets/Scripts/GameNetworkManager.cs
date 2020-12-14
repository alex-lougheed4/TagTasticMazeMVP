using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameNetworkManager : NetworkManager
{
    string playerMaterial;
    TimerClass countdownTimer = new TimerClass();
    TimerClass gameTimer = new TimerClass();
    public int totalPlayers = 0;

    int playerMaterialIndex;

    [Server]
    private void UpdateMyInt() { 
        playerMaterialIndex = totalPlayers;
     }



    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        //base.OnServerAddPlayer(conn);
        GameObject player;
        

        if(totalPlayers <=4)
        {
            
            player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            
            NetworkServer.AddPlayerForConnection(conn,player);
            player.GetComponent<Player>().playerMaterialIndex = totalPlayers;
            totalPlayers++; 
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

   /** public void gameEnd(){ //check if GameNetworkManager.gameEnded == true then end game function
        //record last holder of tag
        //freeze time (timescale)
        //stop playermovement
        
    }

    public void preGameCountdownStart(){
        timer.starterTimer();
        Debug.Log(timer.starterTimerBool);
        if (timer.timeRemaining == 0){ //should be moved to update function
           timer.starterTimerBool = false;
            startGame();
        }
    }

    public void startGame(){
        //Start Timer 
        timer.timeRemaining = 60;
        timer.gameRunTimerBool = true;
        timer.timerIsRunning = true;
        // Allows player movement 
        // Set timescale to 1 (not frozen)
    }
    **/
    
}