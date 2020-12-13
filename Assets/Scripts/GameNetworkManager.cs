using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameNetworkManager : NetworkManager
{
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

    public void gameEnd(){ //check if GameNetworkManager.gameEnded == true then end game function
        //record last holder of tag
        //freeze time (timescale)
        //stop playermovement
        
    }

    public void preGameCountdownStart(){
        GameNetworkManager.starterTimer();
        if (GameNetworkManager.timeRemaining == 0){ //should be moved to update function
            GameNetworkManager.starterTimerBool = false;
            startGame();
        }
    }

    public void startGame(){
        //Start Timer 
        GameNetworkManager.gameRunTimerBool = true;
        GameNetworkManager.timerIsRunning = true;
        // Allows player movement 
        // Set timescale to 1 (not frozen)
    }
    
}