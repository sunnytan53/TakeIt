using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameOver : MonoBehaviour
{
	public EventReference soundWin;
	public EventReference soundLose;
	public EventReference soundEndScene;

	private FMOD.Studio.EventInstance instance;
	private bool wasEnded = false;

	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		CharacterCreator creator = GameObject.Find("Create").GetComponent<CharacterCreator>();
		TMPro.TextMeshProUGUI winCondText = GameObject.Find("WinLose").GetComponent<TMPro.TextMeshProUGUI>();
        int scoreValueT1 = creator.scoreValueT1;
        int scoreValueT2 = creator.scoreValueT2;

		EventReference finalSound;
		if (scoreValueT1 > scoreValueT2) {
			if (Constants.USER_ID <= 2){
				winCondText.text = "Congrats! Your Team Win!";
				finalSound = soundWin;
			}
			else
			{
                winCondText.text = "Oh no! Your Team Lose!";
				finalSound = soundLose;
			}
		}
		else if (scoreValueT1 < scoreValueT2) {
            if (Constants.USER_ID <= 2){
				winCondText.text = "Oh no! Your Team Lose!";
				finalSound = soundLose;
			}
			else
			{
				winCondText.text = "Congrats! Your Team Win!";
				finalSound = soundWin;
			}
		}
		else
        {
			winCondText.text = "Not bad! Your team makes a draw!";
			finalSound = soundWin;
		}

		instance = RuntimeManager.CreateInstance(finalSound);
		instance.start();
	}

    private void Update()
    {
		// loop music
		instance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackState);

		if (playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
		{
			if (!wasEnded)
            {
				wasEnded = true;
				instance = RuntimeManager.CreateInstance(soundEndScene);
			}
			instance.start();
		}
	}

    public void OnExitClick()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}