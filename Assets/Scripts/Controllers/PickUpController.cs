using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    public Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;


    public float range = 10;

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
                    heldObjRB = heldObj.GetComponent<Rigidbody>();

                    heldObjRB.useGravity = false;
                    heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
            else // drop the held object
            {
                heldObjRB.useGravity = true;
                heldObjRB.constraints = RigidbodyConstraints.None;

                heldObj = null;
                heldObjRB = null;
            }
        }

        // update the held object
        if (heldObj != null)
        {
            heldObj.transform.position = holdArea.position;
        }
    }
}
