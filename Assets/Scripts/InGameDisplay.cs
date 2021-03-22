using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDisplay : MonoBehaviour
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
            Application.Quit();
    }
}
