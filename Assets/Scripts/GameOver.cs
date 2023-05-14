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

	void Start()
	{
		Cursor.lockState = CursorLockMode.None;
		CharacterCreator creator = GameObject.Find("Create").GetComponent<CharacterCreator>();
		TMPro.TextMeshProUGUI winCondText = GameObject.Find("WinLose").GetComponent<TMPro.TextMeshProUGUI>();
        int scoreValueT1 = creator.scoreValueT1;
        int scoreValueT2 = creator.scoreValueT2;

        if (scoreValueT1 > scoreValueT2) {
			if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
				winCondText.text = "Congrats! Your Team Win!";
				RuntimeManager.PlayOneShot(soundWin);
			}
			else
			{
                winCondText.text = "Oh no! Your Team Lose!";
				RuntimeManager.PlayOneShot(soundLose);
			}
		}
		else if (scoreValueT1 < scoreValueT2) {
            if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
				winCondText.text = "Oh no! Your Team Lose!";
				RuntimeManager.PlayOneShot(soundLose);
			}
			else
			{
				winCondText.text = "Congrats! Your Team Win!";
				RuntimeManager.PlayOneShot(soundWin);
			}
		}
		else
        {
			winCondText.text = "Not bad! Your team makes a draw!";
			RuntimeManager.PlayOneShot(soundWin);
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