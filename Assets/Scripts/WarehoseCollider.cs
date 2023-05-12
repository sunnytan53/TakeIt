using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehoseCollider : MonoBehaviour
{
    private List<GameObject> fruits = new List<GameObject>();
    private static CharacterCreator creator;
    private int team1 = 1;
    private int team2 = 2;
    private int normalFruitScore = 2;

    void Start() {
        creator = GameObject.Find("Create").GetComponent<CharacterCreator>();
    }

    void Update()
    {
        Debug.Log("total colliding :" + fruits.Count);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            fruits.Add(other.gameObject);
            Debug.Log("Collider game object tag***************: "+ gameObject.tag );
            if (gameObject.CompareTag("Warehouse1")){
                creator.UpdateScore(team1, normalFruitScore);
                Debug.Log("update score with team1: "+ team1 + " and normalFruitScore: " + normalFruitScore);
            }
            else {
                creator.UpdateScore(team2, normalFruitScore);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            fruits.Remove(other.gameObject);
            if (gameObject.CompareTag("Warehouse1")){
                creator.UpdateScore(team1, -normalFruitScore);
                Debug.Log("update score with team1: "+ team1 + " and normalFruitScore: -" + normalFruitScore);
            }
            else {
                creator.UpdateScore(team2, -normalFruitScore);
            }
        }
    }

}
