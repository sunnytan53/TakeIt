using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;


    [SerializeField] private float pickUpRange = 5;
    [SerializeField] private float pickUpForce = 100;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
