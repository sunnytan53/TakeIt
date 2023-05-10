using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioTest : MonoBehaviour
{
    public EventReference soundTest;
    private FMOD.Studio.EventInstance instance;

    public void ClickToPlay()
    {
        instance = RuntimeManager.CreateInstance(soundTest);
        instance.start();

    }


    private void OnDestroy()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
