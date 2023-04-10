using UnityEngine;
using System.Collections;

public class KeyMove : MonoBehaviour {
    public float speed = 6.0f;
    public float gravityMultiplier = 5f;
    public float jumpMultiplier = 10f;

    // moving CharacterController for collision detection instead of transform
    private CharacterController _charController;
    private ParticleSystem ps;
    private Vector3 movement = new Vector3();

    void Start(){
        _charController = GetComponent<CharacterController>();
        ps = GameObject.Find("ParticleCollision").GetComponent<ParticleSystem>();
        gravityMultiplier *= Physics.gravity.y;
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("Floor"))
        {
            Debug.Log("Character Collision with: " + hit.gameObject.name);
            ps.transform.position = hit.point;
            ps.Play();
        }
    }
}