using UnityEngine;
using System.Collections;
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
    private Animator animator;
    private NetworkManager networkManager;

    private Vector3 movement = new Vector3();
    private float horizontalRotation = 0;
    private Quaternion rotation;

    private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    private float holdStartTime;

    public Face faces;
    public GameObject SmileBody; // this should adpat to all individual slimes
    private Material faceMaterial;
    private bool isIdled;

    private animationCode code;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();

        holdArea = transform.Find("HoldArea");
        camera = transform.Find("Camera");

        animator = GetComponent<Animator>();
        gravityMultiplier *= Physics.gravity.y;
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        StartCoroutine(SendMovementRequest());
    }

    void Update(){
        Move();
        Rotate();
        PickOrThrow();
        PlayAnimation();
    }


    private void PlayAnimation()
    {
        switch (code) {
            case animationCode.idle:
                // TODO the idle can't be detected in animator
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
                if (isIdled) return;
                Debug.Log("IDLE");
                isIdled = true;
                animator.ResetTrigger("Jump");
                faceMaterial.SetTexture("_MainTex", faces.IdleFace);
                break;

            case animationCode.walk:
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
                //Debug.Log("WALK");
                isIdled = false;
                animator.SetTrigger("Jump");
                faceMaterial.SetTexture("_MainTex", (heldObj == null) ? faces.IdleFace: faces.WalkFace);
                break;

            case animationCode.throwObj:
                //Debug.Log("THROW");
                isIdled = false;
                animator.SetTrigger("Attack");
                faceMaterial.SetTexture("_MainTex", faces.AttackFace);
                break;
        }
    }

    private void Move()
    {
        // use GetButton instead of GetButtonDown for continous pressing
        // isGrounded is True only when last move collide
        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            movement.y = Mathf.Sqrt(jumpMultiplier * -gravityMultiplier);
        }
        else
        {
            movement.y += gravityMultiplier * Time.deltaTime;
        }
        movement.x = Input.GetAxis("Horizontal") * speed;
        movement.z = Input.GetAxis("Vertical") * speed;
        characterController.Move(transform.TransformDirection(movement) * Time.deltaTime);
        
        if ((movement.x == 0 && movement.z == 0) || !characterController.isGrounded || movement.y > 0)
        {
            code = animationCode.idle;
        }
        else
        {
            code = animationCode.walk;
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
                    heldObj = hit.transform.gameObject;
                    heldObj.GetComponent<Pickable>().isPicked = true;

                    heldObjRB = heldObj.GetComponent<Rigidbody>();
                    heldObjRB.useGravity = false;
                    heldObjRB.drag = 10;
                    heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
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
                float holdTime = Time.time - holdStartTime;

                heldObjRB.useGravity = true;
                heldObjRB.drag = 0;
                heldObjRB.constraints = RigidbodyConstraints.None;
                //heldObjRB.AddForce(transform.forward * 100f * throwForce * holdTime);
                heldObjRB.AddForce(camera.forward * throwForce * holdTime);

                heldObjRB = null;
                heldObj.GetComponent<Pickable>().isPicked = false;
                heldObj = null;
                code = animationCode.throwObj;
            }
        }
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