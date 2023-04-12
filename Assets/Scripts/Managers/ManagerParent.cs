using UnityEngine;
using System.Collections;

public class ManagerParent : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
