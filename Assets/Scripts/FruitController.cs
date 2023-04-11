using UnityEngine;
using System.Collections;

public class FruitController : MonoBehaviour {
    private GameObject fruit;
    private Rigidbody rb;
    private SceneController sc;
    private ParticleSystem ps;

    private Color originalColor;
    private bool isPicked = false;

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
        originalColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        if (isPicked)
        {
            //transform.position = 
        }
    }

    void OnMouseDown()
    {
        string playerTag = gameObject.tag;
        Debug.Log(playerTag);
        if (!isPicked)
        {
            isPicked = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<SphereCollider>().enabled = false;
            Update();
            
        }
        //Vector3 mousePosition = Input.mousePosition;

        //if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit))
        //{
        //    // Get the game object that was hit by the ray
        //    GameObject clickedObject = hit.collider.gameObject;

        //    Debug.Log("Clicked on game object name: " + clickedObject.name);

        //    if (clickedObject.CompareTag("Fruit"))
        //    {
        //        float mouseX = mousePosition.x - 5;
        //        float mouseY = mousePosition.y + 5;
        //        float mouseZ = mousePosition.z;
        //        Rect myRect = new Rect(mouseX, mouseY, 150, 50);
        //        sc.updateScore(1);
        //        Destroy(clickedObject);
        //        //GUI.Button(myRect, "+ 10 pts");
        //    }
        //}

    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }
}
