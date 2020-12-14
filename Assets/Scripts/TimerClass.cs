using System.Collections;
using System.Collections.Generic;
using System;
using System.Timers;

public class TimerClass
{
    private static System.Timers.Timer thisTimer;

    public int gameTimeRemaining;
    public int startTimeRemaining;

    public  void createTimer(string startOrGame, int remainingTime){
        if (startOrGame == "STARTER"){
            startTimeRemaining = remainingTime;
        }
        else if (startOrGame == "GAME"){
            gameTimeRemaining = remainingTime;
        }
        SetTimer();

        thisTimer.Stop();
        thisTimer.Dispose();
    }

    private  void SetTimer(){
        thisTimer = new System.Timers.Timer(1000);
        thisTimer.Elapsed += reduceTime;
        thisTimer.AutoReset = true;
        thisTimer.Enabled = true;
    }


    
    private  void reduceTime(Object source, ElapsedEventArgs e)
    {
        gameTimeRemaining --;
        startTimeRemaining --;
        
    }

}
