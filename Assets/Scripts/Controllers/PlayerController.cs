using UnityEngine;
using System.Collections;

public enum soundCode { none, jump, pick, throwObj };
public enum animationCode { idle, walk, throwObj };

public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    public float gravityMultiplier = 5f;
    public float jumpMultiplier = 10f;
    public float rotateSpeed = 3f;
    public float pickRange = 10f;
    public float throwForce = 100f;
    private new Transform camera;

    private CharacterController characterController;
    private NetworkManager networkManager;
    private PlayerAnimationController animationController;

    private Vector3 movement = new Vector3();
    private float horizontalRotation = 0;
    private Quaternion rotation;

    private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    private float holdStartTime;

    private bool sendNow = false;

    void Start()
    {
        gravityMultiplier *= Physics.gravity.y;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        holdArea = transform.Find("HoldArea");
        camera = transform.Find("Camera");
        animationController = GetComponent<PlayerAnimationController>();

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine(SendPlayerControlRequest());
    }

    void Update(){
        Move();
        Rotate();
        PickOrThrow();
        if (sendNow)
        {
            sendRequest();
            sendNow = false;
        }
    }

    private void Move()
    {
        // use GetButton instead of GetButtonDown for continous pressing
        // isGrounded is True only when last move collide
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            movement.y = Mathf.Sqrt(jumpMultiplier * -gravityMultiplier);
            animationController.sCode = soundCode.jump;
            sendNow = true;
        }
        else
        {
            movement.y += gravityMultiplier * Time.deltaTime;
        }
        movement.x = Input.GetAxis("Horizontal") * speed;
        movement.z = Input.GetAxis("Vertical") * speed;
        characterController.Move(transform.TransformDirection(movement) * Time.deltaTime);
        
        if ((movement.x == 0 && movement.z == 0)
            || !characterController.isGrounded
            || movement.y > 0)
        {
            animationController.aCode = animationCode.idle;
        }
        else
        {
            animationController.aCode = animationCode.walk;
        }
    }

    private void Rotate()
    {
        horizontalRotation += Input.GetAxis("Mouse X") * rotateSpeed;
        rotation = Quaternion.Euler(0, horizontalRotation, 0);
        rotation.z = 0;
        rotation.x = 0;

        transform.rotation = rotation;
    }

    private void PickOrThrow()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // pick the object up if nothing is in hand
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickRange) && hit.transform.gameObject.CompareTag("Pickable"))
                {
                    animationController.sCode = soundCode.pick;

                    heldObj = hit.transform.gameObject;
                    heldObj.GetComponent<PickableController>().isPicked = true;

                    heldObjRB = heldObj.GetComponent<Rigidbody>();
                    heldObjRB.useGravity = false;
                    heldObjRB.drag = 10;
                    heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

                    sendNow = true;
                }
            }
        }

        // update the held object
        if (heldObj != null)
        {
            //heldObj.transform.position = holdArea.position;
            if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
            {
                heldObjRB.AddForce((holdArea.position - heldObj.transform.position) * 10);
            }

            // throw the object with timed force
            if (Input.GetMouseButtonDown(1))
            {
                holdStartTime = Time.time;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                animationController.sCode = soundCode.throwObj;

                float holdTime = Time.time - holdStartTime;

                heldObjRB.useGravity = true;
                heldObjRB.drag = 0;
                heldObjRB.constraints = RigidbodyConstraints.None;
                //heldObjRB.AddForce(transform.forward * 100f * throwForce * holdTime);
                //heldObjRB.AddForce(camera.forward * throwForce * holdTime);
                heldObjRB.AddForce((camera.forward + camera.up * 0.3f) * throwForce * holdTime);


                heldObjRB = null;
                heldObj.GetComponent<PickableController>().isPicked = false;
                heldObj = null;
                animationController.aCode = animationCode.throwObj;

                sendNow = true;
            }
        }
    }


    //IEnumerator SendMovementRequest()
    //{
    //    while (true)
    //    {
    //        // Get the current position and rotation of the player
    //        Vector3 position = transform.position;
    //        Quaternion rotation = transform.rotation;

    //        // Send the position and rotation to the server
    //        // TODO: replace with your own networking code
    //        networkManager.SendMovementRequest(position, rotation);

    //        // Wait for the next update
    //        yield return new WaitForSeconds(0.1f); // update every 100ms
    //    }
    //}

    public void sendRequest()
    {
        networkManager.SendPlayerControlRequest(transform.position, transform.rotation, animationController.aCode, animationController.sCode);
    }

    IEnumerator SendPlayerControlRequest()
    {
        while (true)
        {
            sendRequest();
            yield return new WaitForSeconds(0.1f);
        }
    }
}