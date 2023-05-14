using UnityEngine;
using System.Collections;

public class Pickable : MonoBehaviour
{
    public int index { get; set; }
    public bool isPicked { get; set; } = false;
    public bool isFruit { get; set; } = false;
    public int points { get; set; } = 1;

    void Awake() // do not use start
    {
        gameObject.tag = "Pickable";
    }
}
