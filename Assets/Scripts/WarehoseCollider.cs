using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
        Debug.Log("total fruits colliding :" + objs.Count);
        Debug.Log("the current points are: " + getPoints());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            fruits.Remove(other.gameObject);
        }
        Debug.Log("total fruits colliding :" + objs.Count);
        Debug.Log("the current points are: " + getPoints());
    }

    public int getPoints()
    {
        int allPoints = 0;
        foreach (GameObject pick in objs)
        {
            Pickable pickable = pick.GetComponent<Pickable>();
            if (pickable.isFruit)
            {
                allPoints += pickable.points;
            }
            else
            {
                allPoints -= pickable.points;
            }
        }
        return allPoints;
    }
}
