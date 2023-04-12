using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Audio Managers is in the scene!");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 pos)
    {
        // play sound from using FMODUnity
        RuntimeManager.PlayOneShot(sound, pos);
    }
}
