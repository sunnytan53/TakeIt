using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WarehoseCollider : MonoBehaviour
{
    private List<GameObject> objs = new List<GameObject>();
    private static CharacterCreator creator;
    private int team1 = 1;
    private int team2 = 2;

    void Start() {
        creator = GameObject.Find("Create").GetComponent<CharacterCreator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            objs.Add(other.gameObject);
            if (gameObject.CompareTag("Warehouse1")){
                creator.UpdateScore(team1, getPoints());
                // Debug.Log("update score with team1: "+ team1 + " and getPoints(): " + getPoints());
            }
            else {
                creator.UpdateScore(team2, getPoints());
            }
        }
        Debug.Log("total fruits colliding :" + objs.Count);
        Debug.Log("the current points are: " + getPoints());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            objs.Remove(other.gameObject);
            if (gameObject.CompareTag("Warehouse1")){
                creator.UpdateScore(team1, getPoints());
                // Debug.Log("update score with team1: "+ team1 + " and getPoints(): -" + getPoints());
            }
            else {
                creator.UpdateScore(team2, getPoints());
            }

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
