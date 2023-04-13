using UnityEngine;
using System.Collections;

public class KeyMove : MonoBehaviour {
    public float speed = 6.0f;
    public float gravityMultiplier = 5f;
    public float jumpMultiplier = 10f;
    public float mouseSensitivity = 100f;

    // moving CharacterController for collision detection instead of transform
    private CharacterController _charController;
    private NetworkManager networkManager;

    // placeholders for changing variables in Update()
    private Vector3 movement = new Vector3();

    void Start(){
        _charController = GetComponent<CharacterController>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        gravityMultiplier *= Physics.gravity.y;
        StartCoroutine(SendMovementRequest());
    }

    void Update(){
        // use GetButton instead of GetButtonDown for continous pressing
        if (_charController.isGrounded && Input.GetButton("Jump"))
        {
            movement.y = Mathf.Sqrt(jumpMultiplier * -gravityMultiplier);
        }
        else
        {
            // do not set y=0 when not moving for isGrounded check
            // NOTE: isGrounded is True only when last move collide
            movement.y += gravityMultiplier * Time.deltaTime;
        }

        // "Horizontal" and "Veritcal" are indirect names for keyboard mappings
        movement.x = Input.GetAxis("Horizontal") * speed;
        movement.z = Input.GetAxis("Vertical") * speed;

        movement = transform.TransformDirection(movement);
        _charController.Move(movement * Time.deltaTime);
    }


    IEnumerator SendMovementRequest()
    {
        while (true)
        {
            // Get the current position and rotation of the player
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;

            // Send the position and rotation to the server
            // TODO: replace with your own networking code
            networkManager.SendMovementRequest(position, rotation);

            // Wait for the next update
            yield return new WaitForSeconds(0.1f); // update every 100ms
        }
    }
}