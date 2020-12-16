﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;

public class Timer : NetworkBehaviour
{

    [SyncVar(hook = nameof(OnTimeRemainingValueChange))] protected int countDownTimeRemaining;
    public void OnTimeRemainingValueChange(int old, int new_Value)
    {
        //After the server changed the remainingTime, this will be called on ALL clients.
        //So we dont have to put DisplayTime in a update function, we just call it here, whenever the value changes
        Debug.Log("OnTimeRemainingValueChange");
        countDownTimeRemaining = new_Value;
        DisplayTime(countDownTimeRemaining);
    }

    public Text timerLabel;

    public bool timerIsRunning = false;

    public bool gameEnded = false;

    private void Start()
    {
        /*
         * I JUST USED THIS IN THE START METHOD FOR DEMONSTRATION. 
         */
        if (isServer)
        {
            countDownTimeRemaining = 60;
            StartCoroutine(TimerEverySecond());
        }
    }

    //This will start the timer on the server
    [Server]
    public void startcountDownFunc()
    {
        StartCoroutine(TimerEverySecond());
    }

    IEnumerator TimerEverySecond()
    {
        while (true)
        {
            //DO YOUR STUFF HERE EVERY SECOND
            //Because this coroutine is started from the server, this will only run on the server and change
            //the value of countDownTimeRemaining. By changing this value on the server -> the hook OnTimeRemainingValueChange 
            //will be called
            countDownTimeRemaining--;
            yield return new WaitForSeconds(1f);
        }
    }

    //Removed the ClientRPC here because we call this method with our hook, which will be called on ALL our clients, therefor 
    //no CLIENTRPC is needed
    void DisplayTime(float timeRemaining) //enter timeRemaining
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerLabel.text = ("Time Remaining:    " + minutes + ":" + seconds);
    }
}