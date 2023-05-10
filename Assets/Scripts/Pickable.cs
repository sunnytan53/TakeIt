using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour {
    public bool isPicked { get; set; }
    public int index { get; set; }
    public bool isFruit { get; set; }

    void Awake() // do not use start
    {
        isPicked = false;
        isFruit = false;
        gameObject.tag = "Pickable";
    }
}
