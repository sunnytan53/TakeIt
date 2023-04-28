using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
	private static GameManager gameManager;

	public TMPro.TextMeshProUGUI timerText;
	public TMPro.TextMeshProUGUI team1ScoreText;
	public TMPro.TextMeshProUGUI team2ScoreText;
	private float startTime; 

	// Start is called before the first frame update
	void Start()
	{
		startTime = Time.time; 
		timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
		team1ScoreText = GameObject.Find("Team1 Score").GetComponent<TMPro.TextMeshProUGUI>();
		team2ScoreText = GameObject.Find("Team2 Score").GetComponent<TMPro.TextMeshProUGUI>();
		team1ScoreText.text = "Team1 Score: ";
		team1ScoreText.text = "Team2 Score: ";
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameManager.createCharacters();
		gameManager.createFruits();
	}

	// Update is called once per frame
	void Update()
    {
        float t = Time.time - startTime;
		string minutes = ((int)t / 60).ToString();
		string seconds = (t % 60).ToString("f2");
		timerText.text = minutes + ":" + seconds;
    }
}