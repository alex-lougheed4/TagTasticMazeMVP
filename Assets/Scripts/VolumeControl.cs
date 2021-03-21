using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    AudioSource audioSource;
    public Slider volumeSlider;

    // Start is called before the first frame update
    void Start()
    {   
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable(){
        //Register Slider Events
        volumeSlider.onValueChanged.AddListener(delegate { changeVolume(volumeSlider.value); });    
    }

    void changeVolume(float sliderValue){
        audioSource.volume = sliderValue;
        PlayerPrefs.SetFloat("volume",sliderValue);
        PlayerPrefs.Save();
    }

    void OnDisable(){
        //Un-Register Slider Events
        volumeSlider.onValueChanged.RemoveAllListeners();    
    }
 
}
