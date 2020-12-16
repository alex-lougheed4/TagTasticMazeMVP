using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Timer : NetworkBehaviour
{

    TimerClass countdownTimer = new TimerClass();
    TimerClass gameTimer = new TimerClass();

    [SyncVar] int timeRemaining;    
    public Text timerLabel;

    public bool timerIsRunning = false;

    public bool gameEnded = false;

    [ClientRpc]
    void RpcUpdateTimer(int varToSync){
        timeRemaining = varToSync;
    }
    

    public void startFunc(){
        countdownTimer.createTimer("STARTER",10);
    }
    void Update(){
        ServerTimerUpdate();
        DisplayTime(timeRemaining);
    }

    [Server]
    void ServerTimerUpdate(){
        RpcUpdateTimer(timeRemaining);
    }
 


    void DisplayTime(float timeRemaining) //enter timeRemaining
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60); 
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerLabel.text = ("Time Remaining:    " + minutes + ":" + seconds);
    }
}