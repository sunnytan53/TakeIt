using UnityEngine;
using System.Collections;

public class PickableController : MonoBehaviour {
    //private GameObject fruit;
    //private Rigidbody rb;
    //private ParticleSystem ps;


    public bool isPicked;

    //void Awake() {
    //    sc = GameObject.Find("SceneController").GetComponent<SceneController>();
    //    ps = GameObject.Find("ParticleCollision").GetComponent<ParticleSystem>();
    //    fruit = gameObject;
    //    rb = fruit.GetComponent<Rigidbody>();
    //    float timer = 5f;
    //    rb.useGravity = false;
    //    Debug.Log("Fruit is falling");
    //    StartCoroutine(DropFruit(fruit, timer));
    //}

    //IEnumerator DropFruit(GameObject fruit, float timer) {
    //    yield return new WaitForSeconds(timer);
    //    rb.useGravity = true;
    //}

    //IEnumerator Wait(float timer) {
    //    yield return new WaitForSeconds(timer);
    //    Destroy(gameObject);
    //}

    //void OnCollisionEnter(Collision other)
    //{
    //    float timer = 2f;
    //    Debug.Log("Fruit Collision with: " + other.gameObject.name);
    //    ps.transform.position = other.contacts[0].point;
    //    ps.Play();
    //    if (other.gameObject.CompareTag("Floor"))
    //    {
    //        StartCoroutine(Wait(timer));
    //    }
    //}

    void Start()
    {
        isPicked = false;
        gameObject.tag = "Pickable";
    }

    //void OnMouseEnter()
    //{
    //    if (!isPicked)
    //    {
    //        GetComponent<Renderer>().material.color = Color.yellow;
    //    }
    //}

    //public void OnMouseExit()
    //{
    //    GetComponent<Renderer>().material.color = originalColor;
    //}
}
