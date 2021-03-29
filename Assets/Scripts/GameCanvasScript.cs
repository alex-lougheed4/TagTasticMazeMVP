using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasScript : MonoBehaviour
{
    public Image powerUpImage;
    
    void Start()
    {
        powerUpImage = GameObject.Find("PowerUpImage").GetComponent<Image>();
    }
}
