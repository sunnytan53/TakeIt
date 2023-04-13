using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Transform holdArea;
    public float range = 10;

    private GameObject heldObj;
    private float holdStartTime;


    private void Update()
    {
        // check primary click
        if (Input.GetMouseButtonDown(0))
        {
            // pick the object up if nothing is in hand
            if (heldObj == null)
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range) && hit.transform.gameObject.CompareTag("Pickable"))
                {
                    heldObj = hit.transform.gameObject;

                    Rigidbody heldObjRB = heldObj.GetComponent<Rigidbody>();
                    heldObjRB.useGravity = false;
                    heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            else // drop the held object
            {
                Rigidbody heldObjRB = heldObj.GetComponent<Rigidbody>();
                heldObjRB.useGravity = true;
                heldObjRB.constraints = RigidbodyConstraints.None;

                heldObj = null;
            }
        }

        // update the held object
        if (heldObj != null)
        {
            heldObj.transform.position = holdArea.position;

            // throw the object with timed force
            if (Input.GetMouseButtonDown(1))
            {
                holdStartTime = Time.time;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                float holdTime = Time.time - holdStartTime;
                Rigidbody heldObjRB = heldObj.GetComponent<Rigidbody>();
                heldObjRB.useGravity = true;
                heldObjRB.constraints = RigidbodyConstraints.None;
                heldObjRB.AddForce(transform.forward *1000f * holdTime);
                heldObj = null;
            }
        }
    }
}
