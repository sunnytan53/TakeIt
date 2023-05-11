using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WarehoseCollider : MonoBehaviour
{
    private List<GameObject> fruits = new List<GameObject>();

    public EventReference soundPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && other.GetComponent<Pickable>().isFruit)
        {
            fruits.Add(other.gameObject);
            RuntimeManager.PlayOneShot(soundPoint);
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

    public int getCount()
    {
        return fruits.Count;
    }
}
