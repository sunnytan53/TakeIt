using UnityEngine;
using System.Collections;

public class MouseRotate : MonoBehaviour {
    public float horizontalRotationSpeed = 5;

    private float currentHorizontalRotation = 0f;

    private Quaternion pos = Quaternion.Euler(0, 0, 0);

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        currentHorizontalRotation += mouseX * horizontalRotationSpeed;

        // Rotate the character around the y-axis based on the mouse input
        pos.y = Quaternion.Euler(0, currentHorizontalRotation, 0).y;
        transform.rotation = pos;
    }

    void Start() {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null){
            body.freezeRotation = true;
        }
    }
}