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
    public float verticalRotationSpeed = 3f;

    private float currentHorizontalRotation = 0f;
    private float currentVerticalRotation = 0f;
    private Quaternion pos = Quaternion.Euler(0, 0, 0);

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        currentHorizontalRotation += mouseX * horizontalRotationSpeed;
        currentVerticalRotation -= mouseY * verticalRotationSpeed;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation, -45f, 45f);

        // Rotate the character around the y-axis based on the mouse input
        pos.y = Quaternion.Euler(currentVerticalRotation, currentHorizontalRotation, 0f).y;
        transform.rotation = pos;
    }

    void Start() {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null){
            body.freezeRotation = true;
        }
        pos.x = 0;
        pos.z = 0;
    }
}