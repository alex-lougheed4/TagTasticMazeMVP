using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Mirror{


public class MenuNetworkScript : MonoBehaviour
{

    NetworkManager manager;
    public GameObject sceneLoader;
    public GameObject ipInputField;
    //needs inputting in Unity to get access to the input field
    
    

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }
/**       void OnGUI()
        {

            if (!NetworkClient.isConnected && !NetworkServer.active)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            // client ready
            if (NetworkClient.isConnected && !ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    ClientScene.Ready(NetworkClient.connection);

                    if (ClientScene.localPlayer == null)
                    {
                        ClientScene.AddPlayer(NetworkClient.connection);
                    }
                }
            }

            StopButtons();

            GUILayout.EndArea();
        }
        **/

        public void startHostServerClient(){ //Call for host button
            if(!NetworkClient.active){
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    Debug.Log("test 1");
                    manager.StartHost();
                    Debug.Log("test 2");
                    SceneManager.UnloadSceneAsync("GUIScene");
                    //sceneLoader.GetComponent<SceneLoader>().LoadGame(); //WIP
                    Debug.Log("test 3");
                }
            }
        }

        public void joinClient(){ //call for join with ip button
            if(!NetworkClient.active){
                
                manager.StartClient();
                
            }
            string ipText = ipInputField.GetComponent<TMP_InputField>().text;
            if (ipText == ""){
                ipText = "localhost";
            }

            manager.networkAddress = ipText; //insert ip from textfield here into the networkAddress to connect to
            SceneManager.UnloadSceneAsync("GUIScene");
        }

        public void startServerOnly(){ //call for starting server only
            if(!NetworkClient.active){
                if(Application.platform != RuntimePlatform.WebGLPlayer){
                    manager.StartServer();
                }
            }
        }

/**
        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();
                    }
                    
                }


                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();
                }
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();

                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }

        void StatusLabels()
        {
            // server / client status message
            if (NetworkServer.active)
            {
                GUILayout.Label("Server: active. Transport: " + Transport.activeTransport);
            }
            if (NetworkClient.isConnected)
            {
                GUILayout.Label("Client: address=" + manager.networkAddress);
            }
        }

        void StopButtons()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }
            }
            // stop client if client-only
            else if (NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop Client"))
                {
                    manager.StopClient();
                }
            }
            // stop server if server-only
            else if (NetworkServer.active)
            {
                if (GUILayout.Button("Stop Server"))
                {
                    manager.StopServer();
                }
            }
        }
        **/


}
}