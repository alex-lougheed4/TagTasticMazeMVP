using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;

///<summary>This class refers to the timer aspect of the system, controlling how a timer is created and it's method to count down etc </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

public class Timer : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTimeRemainingValueChange))]
	protected int countDownTimeRemaining = 120; //change value for length of timer

    public void OnTimeRemainingValueChange(int old, int new_Value)
    {
        //After the server changed the remainingTime, this will be called on ALL clients.
        //So we dont have to put DisplayTime in a update function, we just call it here, whenever the value changes
        //Debug.Log("OnTimeRemainingValueChange");
        DisplayTime();
    }

    public Text timerLabel;

    public bool timerIsRunning = false;

    public bool gameEnded = false;

    //This will start the timer on the server
    [Server]
    public void startcountDownFunc()
    {
        StartCoroutine(TimerEverySecond());
    }

    IEnumerator TimerEverySecond()
    {
        while (countDownTimeRemaining > 0)
        {
            countDownTimeRemaining--;
            yield return new WaitForSeconds(1f);
        }
    }


    void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(countDownTimeRemaining / 60);
        float seconds = Mathf.FloorToInt(countDownTimeRemaining % 60);
        timerLabel.text = ("Time Remaining:    " + minutes + ":" + seconds);
    }

    public float getTimeRemaining(){
        return countDownTimeRemaining;
    }
}