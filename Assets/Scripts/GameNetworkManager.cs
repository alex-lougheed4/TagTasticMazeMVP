using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

///<summary>This class refers the the Network manager which controls the multiplayer aspect of the game with it's functions </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

public class GameNetworkManager : NetworkManager
{
    string playerMaterial;

    public Timer timer;

    public int totalPlayers = 0;

    public int maxPlayers;
    int playerMaterialIndex;

    int mazeSeed;
    public GameObject MazeLoader;

    bool gameStarted = false;
    bool gameEnded = false;
    public List<Player> playersList { get; } = new List<Player>();
    


    [Server]
    private void UpdateMyInt() { 
        playerMaterialIndex = totalPlayers;
     }


    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("player added to server");
        GameObject player;
        GameObject powerUp;
        
        

        if(totalPlayers <= maxPlayers)
        {   
            totalPlayers++; 
            player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            //player.name += totalPlayers;
            //Texture s = Resources.Load<Texture>("playerTextures/Player"+ totalPlayers + "_untagged");
            player.GetComponent<Player>().setTextureValue(totalPlayers);
            NetworkServer.AddPlayerForConnection(conn,player);
            

            /**
            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            NetworkServer.Spawn(powerUp);
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned");
            **/
        }

        if(totalPlayers == maxPlayers){ 

            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            NetworkServer.Spawn(powerUp);
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned");


            gameStarted = true;
            Debug.Log("Total = Max");
            timer = GameObject.FindObjectOfType<Timer>();
            timer.startcountDownFunc(); 
            Time.timeScale = 1.0f; //start time when lobby is full -- works
            timer.timerIsRunning = true;
            Debug.Log("Timer started");

            
        }
        
        

    }

    public void FixedUpdate(){
        if(gameStarted){
            if (timer.getTimeRemaining() <= 0.0f){ 
                endGame();
                Debug.Log("Timer Ended");
                timer.timerIsRunning = false;
                gameEnded = true;
                timer.timerLabel.text = "Game Over";
                Time.timeScale = 0.0f; //freeze time after gameover
            }
        }

    }

    [Client]
    void endGame(){
        Debug.Log("Timer Ended");
        timer.timerIsRunning = false;
        gameEnded = true;
        timer.timerLabel.text = "Game Over";
        Time.timeScale = 0.0f;               
    }

    public bool getHasGameEnded(){
        return gameEnded;
    }

    public override void OnStartServer()
    {
        Debug.Log("Server Started!");
        Time.timeScale = 0.0f; //start server with time frozen
        
    }

    public override void OnStopServer()
    {
        Debug.Log("Server Stopped!");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("Connected to Server!");
        base.OnClientConnect(conn);
        
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        Debug.Log("Disconnected from Server!");
        base.OnClientDisconnect(conn);
    }
}