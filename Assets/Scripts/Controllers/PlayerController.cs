using UnityEngine;
using System.Collections;
using FMODUnity;

public class PlayerController : MonoBehaviour {
    public float speed = 5f;
    public float gravityMultiplier = 5f;
    public float jumpMultiplier = 10f;
    public float rotateSpeed = 3f;
    public float pickRange = 10f;
    public float throwForce = 100f;
    private new Transform camera;

    private CharacterController characterController;
    private PlayerArtController artController;
    private NetworkManager networkManager;

    private Vector3 movement = new Vector3();
    private float horizontalRotation = 0;
    private Quaternion rotation;

    private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    private float holdStartTime;

    private bool wasLanded = false;


    void Start()
    {
        gravityMultiplier *= Physics.gravity.y;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        holdArea = transform.Find("HoldArea");
        camera = transform.Find("Camera");
        artController = GetComponent<PlayerArtController>();

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine(SendMovementRequest());
    }

    void Update(){
        Move();
        Rotate();
        PickOrThrow();
    }

    private void Move()
    {
        // use GetButton instead of GetButtonDown for continous pressing
        // isGrounded is True only when last move collide
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            movement.y = Mathf.Sqrt(jumpMultiplier * -gravityMultiplier);
            artController.setAnimationCode(AnimationCodeEnum.jump);
            wasLanded = false;
        }
        else
        {
            movement.y += gravityMultiplier * Time.deltaTime;
        }
        movement.x = Input.GetAxis("Horizontal") * speed;
        movement.z = Input.GetAxis("Vertical") * speed;
        characterController.Move(transform.TransformDirection(movement) * Time.deltaTime);

        if (characterController.isGrounded)
        {
            if ((movement.x != 0 || movement.z != 0))
            {
                artController.setAnimationCode(AnimationCodeEnum.walk);
            }
            if (!wasLanded)
            {
                artController.setAnimationCode(AnimationCodeEnum.landing);
                wasLanded = true;
            }
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
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, pickRange)
                    && hit.transform.gameObject.CompareTag("Pickable"))
                {
                    GameObject hitObj = hit.transform.gameObject;
                    Pickable pickable = hitObj.GetComponent<Pickable>();
                    if (!pickable.isPicked)
                    {
                        artController.setAnimationCode(AnimationCodeEnum.pick);

                        heldObj = hit.transform.gameObject;
                        heldObjRB = heldObj.GetComponent<Rigidbody>();
                        heldObjRB.useGravity = false;
                        //heldObjRB.drag = 10;
                        heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                        pickable.isPicked = true;

                        SendPickRequest(pickable.index);
                    }
                }
            }
        }

        // update the held object
        if (heldObj != null)
        {
            heldObj.transform.position = holdArea.position;
            //if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
            //{
            //    heldObjRB.AddForce((holdArea.position - heldObj.transform.position) * 10);
            //}

            // throw the object with timed force
            if (Input.GetMouseButtonDown(1))
            {
                holdStartTime = Time.time;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                artController.setAnimationCode(AnimationCodeEnum.throwObj);

                float holdTime = Mathf.Min(Time.time - holdStartTime, 3f);
                holdTime = Mathf.Max(holdTime, 0.1f); // sometimes holdTime is negative for unknown reasons

                heldObjRB.useGravity = true;
                //heldObjRB.drag = 0;
                heldObjRB.constraints = RigidbodyConstraints.None;
                Vector3 addedForce = (camera.forward + camera.up * 0.3f) * throwForce * holdTime;
                heldObjRB.AddForce(addedForce);
                heldObjRB = null;
                Pickable pickable = heldObj.GetComponent<Pickable>();
                pickable.isPicked = false;
                heldObj = null;

                SendThrowRequest(pickable.index, addedForce);                
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pickable"))
        {
            if (collision.relativeVelocity.magnitude > 10f)
            {
                // force to throw the fruit
                artController.setAnimationCode(AnimationCodeEnum.stun);
                bool isFruit = collision.gameObject.GetComponent<Pickable>().isFruit;

                heldObjRB.useGravity = true;
                //heldObjRB.drag = 0;
                heldObjRB.constraints = RigidbodyConstraints.None;
                heldObjRB = null;
                Pickable pickable = heldObj.GetComponent<Pickable>();
                pickable.isPicked = false;
                heldObj = null;
                SendThrowRequest(pickable.index, new Vector3(0,0,0));

                StartCoroutine(UnstunPlayer(collision.relativeVelocity.magnitude/10f, isFruit));
                this.enabled = false;
            }
        }
    }

    IEnumerator UnstunPlayer(float time, bool toDouble)
    {
        if (toDouble) time *= 2;
        yield return new WaitForSeconds(time);
        this.enabled = true;
    }


    IEnumerator SendMovementRequest()
    {
        while (true)
        {
            networkManager.SendMovementRequest(transform.position, transform.rotation);
            // Wait for the next update
            yield return new WaitForSeconds(0.1f); // update every 100ms
        }
    }

    public void SendPickRequest(int index) {

        Debug.Log("In SendPickRequest, sending index: " + index);
        networkManager.SendPickRequest(index);
    }

    public void SendThrowRequest(int index, Vector3 force){
        Debug.Log("In SendThrowRequest, sending index: " + index);
        networkManager.SendThrowRequest(index, force);
    }
}