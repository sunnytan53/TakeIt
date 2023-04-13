using UnityEngine;

public class MouseRotate : MonoBehaviour {
    public float horizontalRotationSpeed = 5;

    private float horizontalRotation = 0;
    private Quaternion pos;

    void Update()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * horizontalRotationSpeed;

        // Rotate the character around the y-axis based on the mouse input
        pos = Quaternion.Euler(0, horizontalRotation, 0);
        pos.z = 0;
        pos.x = 0;

        transform.rotation = pos;
    }

    void Start() {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null){
            body.freezeRotation = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }
}