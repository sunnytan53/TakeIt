using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MenuSound : MonoBehaviour
{
    [SerializeField] private EventReference someSound;

    public void playSound()
    {
        AudioManager.instance.PlayOneShot(someSound, transform.position);
    }
}
