using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
	private static GameManager gameManager;
	public TMPro.TextMeshProUGUI timerText;
	public TMPro.TextMeshProUGUI team1ScoreText;
	public TMPro.TextMeshProUGUI team2ScoreText;
	private GameObject gameOverPanel;
	[SerializeField] private Button scoreButton;
	private float startTime; 
	public float timeRemaining = 120f;

	// Start is called before the first frame update
	void Start()
	{
		startTime = Time.time; 
		timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
		// team1ScoreText = GameObject.Find("Team1 Score").GetComponent<TMPro.TextMeshProUGUI>();
		// team2ScoreText = GameObject.Find("Team2 Score").GetComponent<TMPro.TextMeshProUGUI>();
		// team1ScoreText.text = "Team1 Score: ";
		// team1ScoreText.text = "Team2 Score: ";
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameOverPanel = GameObject.Find("GameOver");
		gameOverPanel.SetActive(true);
		scoreButton.onClick.AddListener(OnButtonClick);
		gameManager.createCharacters();
		gameManager.createFruits();
	}

	// Update is called once per frame
	void Update()
    {
		if (timeRemaining > 0) {
			timeRemaining = Mathf.Clamp(timeRemaining - Time.deltaTime, 0f, 120f); // decrement time remaining by the time elapsed in each frame
			string minutes = Mathf.FloorToInt(timeRemaining / 60f).ToString();
    		string seconds = Mathf.FloorToInt(timeRemaining % 60f).ToString("00");
    		timerText.text = "Timer: " + minutes + ":" + seconds;
		}
		else {
			/*
			if (scoreValueT1 == scoreValueT2) {
				timeRemaining += 30f;
			}
			else {
				gameOverPanel.SetActive(true);
			}
			*/
		}
    }

	public void OnButtonClick()
	{
	    SceneManager.LoadScene("Menu");
	}
}