using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {
    public bool isPicked { get; set; }

    void Start()
    {
        isPicked = false;
        gameObject.tag = "Pickable";
    }
}
