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
    float randomPosX; //random spawn position of player in x axis
    float randomPosZ; //random spawn position of player in z axis 

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
        GameObject playerRadarIndicator;
        
        

        if(totalPlayers <= maxPlayers)
        {   
            totalPlayers++; 
            player = Instantiate(Resources.Load("Prefabs/Player")) as GameObject;
            player.GetComponent<Player>().setTextureValue(totalPlayers);
            generateRandomPositions();
            if((randomPosX != 0) || (randomPosZ != 0)){
            player.transform.position = new Vector3(randomPosX,0,randomPosZ);
            }
            NetworkServer.AddPlayerForConnection(conn,player);
            
        }

        if(totalPlayers == maxPlayers){ 

            powerUp = Instantiate(Resources.Load("Prefabs/PowerUp")) as GameObject;
            NetworkServer.Spawn(powerUp);
            powerUp.GetComponent<Powerup>().spawnPowerUp();
            Debug.Log("PowerUpSpawned"); 

            foreach (Player p in playersList){
                playerRadarIndicator = Instantiate(Resources.Load("Prefabs/playerRadarIndicator")) as GameObject;
                NetworkServer.Spawn(playerRadarIndicator);
                p.playerRadarIndicator = playerRadarIndicator;
                p.speed = 0.01f;  
            }


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
                Debug.Log("Timer Ended");
                timer.timerIsRunning = false;
                gameEnded = true;
                timer.timerLabel.text = "Game Over";
                foreach (Player p in playersList){
                    p.endGameForClients();
                    if (p.hasTag){
                        p.pushWinnerToLeaderboaord(p.username); //call leaderboard push function
                        Debug.Log("winner's username: " + p.username);
                        p.winnerUploaded = true;
                    }
                }
                Time.timeScale = 0.0f; //freeze time after gameover
            }
        }

    }
    public void generateRandomPositions(){
        randomPosX = (float)Random.Range(-15f, 15f);
        randomPosZ = (float)Random.Range(-15f, 15f);
    }

    public bool getHasGameEnded(){
        return gameEnded;
    }

    public bool getHasGameStarted(){
        return gameStarted;
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