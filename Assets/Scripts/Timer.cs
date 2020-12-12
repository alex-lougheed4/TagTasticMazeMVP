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
    
    


    private void Start()
    {
        // Starts the timer automatically
        timerIsRunning = true;
    }

    void Update()
    {
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