using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject followingObject;
    float originalZ;

    private void Start()
    {
        originalZ = transform.position.z;
    }

    void Update()
    {
        if (followingObject)
        {
            Vector3 destination = followingObject.transform.position;
            destination.z = originalZ;
            transform.position = destination;
        }
    }
}
