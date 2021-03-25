using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class contains the method necessary to "attach" the camera to the user's player and smooths the camera movement </summary>
///
///<author email="190045601@aston.ac.uk">Alexander Lougheed </author>

public class CameraFollow : NetworkBehaviour
{
    public Transform target; //what to follow
    public float smoothing = 5f; //camera speed

    AudioSource audioSource;
    Vector3 offset; //offset of camera and player

    void Start() //on start
    {
        offset.y = 8.0f; //set offset
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("volume");
    }

    void FixedUpdate() //every update
    {
        Vector3 targetCamPos = target.position + offset; //set the camera position in relation to the player
        transform.position = Vector3.Lerp (transform.position,  targetCamPos,smoothing * Time.deltaTime); //linear interpolation to set the camera's position with time (camera follow effect)
    }
}