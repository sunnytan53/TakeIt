using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float cameraDistance = 10.0f;

    private Vector3 finalPos;
    private float rotationY = 0;
    private Quaternion originalRotation;

    private void Start()
    {
        originalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        transform.position = player.position - player.forward * cameraDistance;
        finalPos = player.position;
        transform.LookAt(finalPos);


        rotationY += Input.GetAxis("Mouse Y") * 3f;
        rotationY = Mathf.Clamp(rotationY, -10f, 10f);
        Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
        transform.localRotation = originalRotation * yQuaternion;

    }
}