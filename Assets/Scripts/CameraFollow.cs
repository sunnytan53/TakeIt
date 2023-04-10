using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 10.0f;

    void LateUpdate()
    {
        transform.position = player.position - player.forward * cameraDistance;
        transform.LookAt(player.position);
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
    }
}