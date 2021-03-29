using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InGameDisplay : NetworkBehaviour
{
    public GameObject confirmPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            confirmPanel.SetActive(true);
        }
        
    }

        public void confirmQuitButton(){
            if(isServer){
                NetworkManager.singleton.StopHost();
            } 
            else{
                NetworkManager.singleton.StopClient();
            }
            NetworkManager.singleton.ServerChangeScene("GUIScene");
    }
}
