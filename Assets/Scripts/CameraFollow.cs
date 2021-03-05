using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    public Transform target; //what to follow
    public float smoothing = 5f; //camera speed

    Vector3 offset; //offset of camera and player

    void Start() //on start
    {
        offset.y = 15.0f; //set offset
    }

    void FixedUpdate() //every update
    {
        Vector3 targetCamPos = target.position + offset; //set the camera position in relation to the player
        transform.position = Vector3.Lerp (transform.position,  targetCamPos,smoothing * Time.deltaTime); //linear interpolation to set the camera's position with time (camera follow effect)
    }
}