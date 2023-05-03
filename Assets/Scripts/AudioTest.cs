using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioTest : MonoBehaviour
{
    public EventReference soundTest;

    public void ClickToPlay()
    {
        RuntimeManager.PlayOneShot(soundTest);
    }
}
