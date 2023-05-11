using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehoseCollider : MonoBehaviour
{
    private List<GameObject> fruits = new List<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && other.GetComponent<Pickable>().isFruit)
        {
            fruits.Add(other.gameObject);
        }
        Debug.Log("total fruits colliding :" + fruits.Count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && other.GetComponent<Pickable>().isFruit)
        {
            fruits.Remove(other.gameObject);
        }
        Debug.Log("total fruits colliding :" + fruits.Count);
    }

}
