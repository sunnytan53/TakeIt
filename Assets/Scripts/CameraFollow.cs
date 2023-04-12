using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float frontDistance = 20;
    public float topDistance = 5;

    private Transform player;
    private Vector3 finalPos;
    private float rotationUpDown = 0;
    private Quaternion originalRotation;

    private void Start()
    {
        player = transform.parent;
        originalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        finalPos = player.position - player.forward * frontDistance + Vector3.up * topDistance;
        transform.position = finalPos;
        transform.LookAt(player.position);


        // TODO: the angle should also follow the distance so that it works when we change distance
        rotationUpDown = Mathf.Clamp(rotationUpDown + Input.GetAxis("Mouse Y") * 3f, -20f, 5f);
        Quaternion yQuaternion = Quaternion.AngleAxis(-rotationUpDown, Vector3.right);
        transform.localRotation = originalRotation * yQuaternion;
    }
}