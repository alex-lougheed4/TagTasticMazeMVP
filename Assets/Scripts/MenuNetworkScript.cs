using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

///<summary>This class is used to give functionality to the GUI allowing the user to make meaningful use of the GUI in reference to the multiplayer aspect of the solution </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

namespace Mirror{


public class MenuNetworkScript : NetworkBehaviour
{

    public GameObject ipInputField;
    public GameObject usernameInputField;
    NetworkManager manager;
    //NetworkManager manager => GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    
    string baseServerAddress;
    string address;
    
    public void Awake(){
        manager = GetComponent<NetworkManager>();
    }
    
    public void HostButton() {
        setUsername();
        manager.StartHost();
    } 

    public void JoinButton(){
        address = ipInputField.GetComponent<TMP_InputField>().text;
        if(address == ""){
            address = "localhost";
        }
        manager.networkAddress = address;
        manager.StartClient();
        setUsername();
    }

    public void StartButton(){ //called when start game is clicked
        baseServerAddress = "ec2-3-16-125-214.us-east-2.compute.amazonaws.com";//insert the ip for the main server here
        manager.networkAddress = baseServerAddress;
        manager.StartClient();
        setUsername();
    }

    public void startServerOnlyButton(){
        manager.StartServer();
    }
    public void setUsername(){
        string username = usernameInputField.GetComponent<TMP_InputField>().text;
        Debug.Log("Username at GUI input: " + username);
        PlayerPrefs.SetString("username",username);
        PlayerPrefs.Save();
    }
}
}