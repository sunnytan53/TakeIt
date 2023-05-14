using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMODUnity;

public class CharacterCreator : MonoBehaviour
{
	private static GameManager gameManager;
	public TMPro.TextMeshProUGUI timerText;
	public TMPro.TextMeshProUGUI team1ScoreText;
	public TMPro.TextMeshProUGUI team2ScoreText;
	public TMPro.TextMeshProUGUI winCondText;
	public Button tryAgainButton;
	
	private GameObject gameOverPanel;
	private float startTime; 
	public float timeRemaining = 310f; // 120f;
	public int scoreValueT1;
	public int scoreValueT2;


	public EventReference soundGame;
	private FMOD.Studio.EventInstance instance;

	// Start is called before the first frame update
	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameManager.createCharacters();
		gameManager.createFruits();

		instance = RuntimeManager.CreateInstance(soundGame);
		instance.start();

		startTime = Time.time; 
		scoreValueT1 = 0;
		scoreValueT2 = 0;
		//timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
		//winCondText = GameObject.Find("WinLose").GetComponent<TMPro.TextMeshProUGUI>();
		//team1ScoreText = GameObject.Find("Team1 Score").GetComponent<TMPro.TextMeshProUGUI>();
		//team2ScoreText = GameObject.Find("Team2 Score").GetComponent<TMPro.TextMeshProUGUI>();
		//tryAgainButton = GameObject.Find("PlayAgian").GetComponent<Button>();

		team1ScoreText.text = "0";
		team2ScoreText.text = "0";
		
		gameOverPanel = GameObject.Find("GameOver");
		gameOverPanel.SetActive(false);
		tryAgainButton.onClick.AddListener(OnButtonClick);
		
		// StartCoroutine(IncrementScoreTest());
	}

	// Update is called once per frame
	void Update()
    {
		if (timeRemaining > 0) {
			timeRemaining = Mathf.Clamp(timeRemaining - Time.deltaTime, 0f, 310f); // decrement time remaining by the time elapsed in each frame
			string minutes = Mathf.FloorToInt(timeRemaining / 60f).ToString();
    		string seconds = Mathf.FloorToInt(timeRemaining % 60f).ToString("00");
    		timerText.text = "Timer: " + minutes + ":" + seconds;
		}
		else {
			if (scoreValueT1 > scoreValueT2) {
				if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
					winCondText.text = "Congrats! Your Team Win!";
				}
				if (Constants.USER_ID == 3 || Constants.USER_ID == 4){
					winCondText.text = "Oh no! Your Team Lose!";
				}
				gameOverPanel.SetActive(true);
				StartCoroutine(LoadMenuSceneCoroutine());
			}
			else if (scoreValueT1 < scoreValueT2) {
				if (Constants.USER_ID == 1 || Constants.USER_ID == 2){
					winCondText.text = "Oh no! Your Team Lose!";
				}
				if (Constants.USER_ID == 3 || Constants.USER_ID == 4){
					winCondText.text = "Congrats! Your Team Win!";
				}
				gameOverPanel.SetActive(true);
				StartCoroutine(LoadMenuSceneCoroutine());
			}
			else{
				timeRemaining += 30f;
			}
			
		}
	}

	private void OnDestroy()
	{
		instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		instance.release();
	}

	public void OnButtonClick()
	{
	    SceneManager.LoadScene("Menu");
	}

	private IEnumerator LoadMenuSceneCoroutine() {
    	yield return new WaitForSeconds(5f); 
    	SceneManager.LoadScene("Menu"); 
	}

	private IEnumerator IncrementScoreTest() {
		while (timeRemaining > 0) {
        	scoreValueT1 += 10;
        	scoreValueT2 += 10;
			team1ScoreText.text = scoreValueT1.ToString();
			team2ScoreText.text = scoreValueT2.ToString();
			StartCoroutine(Pulse(team1ScoreText));
			StartCoroutine(Pulse(team2ScoreText));
        	yield return new WaitForSeconds(2.0f);
    	}
	}

	public void UpdateScore(int team, int score) {
		if (team == 1){
			scoreValueT1 += score;
			team1ScoreText.text = scoreValueT1.ToString();
			StartCoroutine(Pulse(team1ScoreText));
		}
		else {
			scoreValueT2 += score;
			team2ScoreText.text = scoreValueT2.ToString();
			StartCoroutine(Pulse(team2ScoreText));
		}
	}

	private IEnumerator Pulse(TMPro.TextMeshProUGUI scoreText) {
		for (float i = 1f; i <= 1.2f; i += 0.05f) {
			scoreText.rectTransform.localScale = new Vector3(i, i, i);
			yield return new WaitForEndOfFrame();
		}
		scoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

		for (float i = 1.2f; i >= 1f; i -= 0.05f) {
			scoreText.rectTransform.localScale = new Vector3(i, i, i);
			yield return new WaitForEndOfFrame();
		}
		scoreText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
	}
}