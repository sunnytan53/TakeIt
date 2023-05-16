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
	
	public float timeRemaining = 240f;
	public int scoreValueT1;
	public int scoreValueT2;

	public EventReference soundPointGain;
	public EventReference soundPointLose;

	public EventReference soundGame;
	private FMOD.Studio.EventInstance instance;
	private bool isEnd = false;

	// Start is called before the first frame update
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameManager.initMap(GameObject.Find("Tree").transform.position,
			GameObject.Find("WarehouseTeam1").transform.position,
			GameObject.Find("WarehouseTeam2").transform.position);

		TMPro.TextMeshProUGUI youText = GameObject.Find("you").GetComponent<TMPro.TextMeshProUGUI>();
		if (Constants.USER_ID <= 2)
        {
			youText.text = "<-YOU";
        }
		else
		{
			youText.text = "\n<-YOU";
		}

		instance = RuntimeManager.CreateInstance(soundGame);
		instance.start();

		scoreValueT1 = 0;
		scoreValueT2 = 0;
		timerText = GameObject.Find("Timer").GetComponent<TMPro.TextMeshProUGUI>();
		team1ScoreText = GameObject.Find("Team1 Score").GetComponent<TMPro.TextMeshProUGUI>();
		team2ScoreText = GameObject.Find("Team2 Score").GetComponent<TMPro.TextMeshProUGUI>();

		team1ScoreText.text = "0";
		team2ScoreText.text = "0";
	}

	// Update is called once per frame
	void Update()
    {
		if (timeRemaining > 0) {
			timeRemaining = timeRemaining - Mathf.Clamp(Time.deltaTime, 0f, 10f);
			string minutes = Mathf.FloorToInt(timeRemaining / 60f).ToString();
    		string seconds = Mathf.FloorToInt(timeRemaining % 60f).ToString("00");
    		timerText.text = "Timer: " + minutes + ":" + seconds;
		}
		else if (!isEnd)
		{
			isEnd = true;
			instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
			instance.release();
			Destroy(gameManager);
			SceneManager.LoadScene("GameOver");
		}
	}


	public void UpdateScore(int team, int score) {
		bool gainOrLose;
		if (team == 1){
			gainOrLose = scoreValueT1 < score;
			scoreValueT1 = score;
			team1ScoreText.text = scoreValueT1.ToString();
			//StartCoroutine(Pulse(team1ScoreText));
		}
		else
		{
			gainOrLose = scoreValueT2 < score;
			scoreValueT2 = score;
			team2ScoreText.text = scoreValueT2.ToString();
			//StartCoroutine(Pulse(team2ScoreText));
		}

		if (gainOrLose)
        {
			RuntimeManager.PlayOneShot(soundPointGain);
		}
		else
        {
			RuntimeManager.PlayOneShot(soundPointLose);
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