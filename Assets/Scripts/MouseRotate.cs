using UnityEngine;
using System.Collections;

public class MouseRotate : MonoBehaviour {
    /***
    public enum RotationAxes {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityHor = 2.0f;
    public float sensitivityVert = 2.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float _rotationX = 0;
    ***/

    public float horizontalRotationSpeed = 3f;

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