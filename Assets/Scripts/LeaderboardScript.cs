using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LeaderboardScript : MonoBehaviour
{

    const string privateCode = "x69VF7tpq063ZGkeNLClAAOLKpdA1oA0-xPFQMeKgNjw"; 
    const string publicCode = "6059c8738f40bb473467e4c6";
    const string webURL = "http://dreamlo.com/lb/"; //Declares Constants
    int currentWins;
    string username;

    public void UploadWinner(string username){
        this.username = username;
        AddNewWin(this.username);
    }

    public void AddNewWin(string username){
        StartCoroutine(UploadNewWin(username));
    }

    IEnumerator UploadNewWin(string username){
        downloadCurrentWins();
        Debug.Log("username: "+ username);
        Debug.Log("current wins: " + currentWins);
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
        string username = this.username; //get player username
        WWW WWW = new WWW(webURL + publicCode + "/pipe-get/" + username);
        yield return WWW;

        if(string.IsNullOrEmpty(WWW.error)){
            Debug.Log("CurrentWins Downloaded");
            SaveScore(WWW.text);
        }
        else{
            Debug.Log("Error Downloading: " + WWW.error);
        }
    }

    public void SaveScore(string textStream){
        if(textStream == null){
            currentWins = 0;
        }
        else{
            currentWins = int.Parse(textStream);
        }
    }
 

}