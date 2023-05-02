using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotateSpeed = 3f;

    private Transform player;
    private float rotationUpDown = 0;
    private Quaternion originalRotation;


    private void Start()
    {
        player = transform.parent;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        transform.LookAt(player.position);

        // TODO: the angle should also follow the distance so that it works when we change distance
        rotationUpDown = Mathf.Clamp(rotationUpDown + Input.GetAxis("Mouse Y") * rotateSpeed, 0f, 60f);
        Quaternion yQuaternion = Quaternion.AngleAxis(-rotationUpDown, Vector3.right);
        transform.localRotation = originalRotation * yQuaternion;
    }
}