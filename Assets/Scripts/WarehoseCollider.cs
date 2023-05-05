using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehoseCollider : MonoBehaviour
{
    private List<GameObject> fruits = new List<GameObject>();

    void Update()
    {
        //Debug.Log("total colliding :" + fruits.Count);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            fruits.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            fruits.Remove(other.gameObject);
        }
    }

}
