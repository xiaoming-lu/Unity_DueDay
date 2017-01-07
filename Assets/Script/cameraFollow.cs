using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour {

    
    public Transform target;
    public float smoothing = 5.0f;

    Vector3 offset;
    // Use this for initialization
    void Start()
    {

        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        Vector3 CameraPosition = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, CameraPosition, smoothing * Time.deltaTime);
    }
}

