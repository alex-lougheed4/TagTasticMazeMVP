using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Timer : NetworkBehaviour
{
    public Text timerLabel;
    public float timeRemaining = 240;
    public bool timerIsRunning = false;

    public bool starterTimerBool = false;

    public bool gameRunTimerBool = false;

    public bool gameEnded = false;
    
    public void starterTimer(){ //Timer countdown before game starts
        timeRemaining = 10;
        starterTimerBool = true;
    }

    void Update()
    {
        if(starterTimerBool){
            if (timeRemaining > 0)
            {
                Debug.Log("starter Timer Running");
                DisplayTime(timeRemaining);
                Debug.Log(timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("starter Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }

        }
        if(gameRunTimerBool){

            if (timerIsRunning)
            {
             if (timeRemaining > 0)
                {
                    Debug.Log("Timer Running");
                    DisplayTime(timeRemaining);
                    Debug.Log(timeRemaining);
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    gameEnded = true;
                    
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay) //enter timeRemaining
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60); 
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerLabel.text = ("Time Remaining:    " + minutes + ":" + seconds);
    }
}