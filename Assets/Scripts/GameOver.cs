using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class GameOver : MonoBehaviour
{
	public TMPro.TextMeshProUGUI winCondText;
	// public Button tryAgainButton;
    private static CharacterCreator creator;
    public int scoreValueT1;
	public int scoreValueT2;

    void Start()
	{
        creator = GameObject.Find("Create").GetComponent<CharacterCreator>();
        // Debug.Log("CharacterCreator game object found is: " + creator);
        // Debug.Log(" creator.scoreValueT1 is: " +  creator.scoreValueT1);
        // Debug.Log(" creator.scoreValueT2 is: " +  creator.scoreValueT2);
        winCondText = GameObject.Find("WinLose").GetComponent<TMPro.TextMeshProUGUI>();
        scoreValueT1 = creator.scoreValueT1;
        scoreValueT2 = creator.scoreValueT2;

        if (scoreValueT1 > scoreValueT2) {
			if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
				winCondText.text = "Congrats! Your Team Win!";
			}
			if (Constants.USER_ID == 3 || Constants.USER_ID == 4){
                winCondText.text = "Oh no! Your Team Lose!";
			}
		}
		else if (scoreValueT1 < scoreValueT2) {
            if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
				winCondText.text = "Oh no! Your Team Lose!";
			}
			if (Constants.USER_ID == 3 || Constants.USER_ID == 4){
				winCondText.text = "Congrats! Your Team Win!";
			}
		}
	}

}