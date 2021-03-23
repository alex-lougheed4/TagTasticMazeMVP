using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{

    const string privateCode = "luTL-dhEpEeq83598DpGVwZFPy8mgNGU6fllF3Y0Wg_A"; 
    const string publicCode = "6059c8738f40bb473467e4c6";
    const string webURL = "http://dreamlo.com/lb/"; //Declares Constants
    int currentWins;

    public void UploadWinner(){
        string username = ""; //get player username
        AddNewWin(username);
    }

    public void AddNewWin(string username){
        StartCoroutine(UploadNewWin(username));
    }

    IEnumerator UploadNewWin(string username){
        WWW WWW = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + currentWins+1);
        yield return WWW;

        if(string.IsNullOrEmpty(WWW.error)){
            Debug.Log("Upload Successful");
        }
        else{
            Debug.Log("Error Uploading: " + WWW.error);
        }
    }

    IEnumerator downloadCurrentWins(){
        string username = ""; //get player username
        WWW WWW = new WWW(webURL + publicCode + "/pipe-get/" + username);
        yield return WWW;

        if(string.IsNullOrEmpty(WWW.error)){
            Debug.Log("CurrentWins Downloaded");
            SaveHighScores(WWW.text);
        }
        else{
            Debug.Log("Error Downloading: " + WWW.error);
        }
    }

    public void SaveHighScores(string textStream){
        currentWins = int.Parse(textStream);
    }
 

}