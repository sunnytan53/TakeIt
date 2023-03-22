using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFruitController : MonoBehaviour {
    public GameObject fruitPrefab;
    private GameObject fruit;
    private Rigidbody rb;
    private ParticleSystem ps;

    void Start(){
        ps = GameObject.Find("ParticleCollision").GetComponent<ParticleSystem>();
    }

    void OnMouseDown()
    {
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Get the game object that was hit by the ray
            GameObject clickedObject = hit.collider.gameObject;

            // Perform some action in response to the click
            Debug.Log("Clicked on game object name: " + clickedObject.name);

            if (clickedObject.CompareTag("Tree"))
            {
                System.Random rand = new System.Random();
                int randomX = rand.Next(-5,6);
                int randomY = rand.Next(8,15);
                int randomZ = rand.Next(-5,6);
                Vector3 proPosition = new Vector3(randomX, randomY, randomZ);
                // Instantiate the prefab at the random position
                fruit = Instantiate(fruitPrefab, proPosition, Quaternion.identity);
            }
        }       
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Tree Collision with: " + other.gameObject.name);
        ps.transform.position = other.contacts[0].point;
        ps.Play();
    }

}
