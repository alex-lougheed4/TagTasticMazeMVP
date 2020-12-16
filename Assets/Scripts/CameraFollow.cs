using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : NetworkBehaviour
{
    public Transform target; //what to follow
    public float smoothing = 5f; //camera speed

    Vector3 offset;

    void Start()
    {
        offset.y = 15.0f;
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp (transform.position,  targetCamPos,smoothing * Time.deltaTime);
    }
}

