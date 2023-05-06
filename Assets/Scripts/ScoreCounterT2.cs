using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreCounterT2 : MonoBehaviour{

	[SerializeField] private TextMeshProUGUI scoreText;
	[SerializeField] private Button scoreButton;
	public int scoreValue;

	void Start() {
		scoreValue = 0;
	}

	// void Update() {
	// 	scoreText.text = scoreValue.ToString();
	// }

	private void Awake(){
		// scoreButton = GameObject.Find("TestScoreT2").GetComponent<Button>();
		scoreButton.onClick.AddListener(OnButtonClick);
		// scoreText = GetComponent<TextMeshProUGUI>();
	}

	public void RunCo(){
		StartCoroutine(Pulse());
	}

	public void OnButtonClick()
	{
	    scoreValue += 10;
		scoreText.text = scoreValue.ToString();
		StartCoroutine(Pulse());
	    // scoreText.text = "T1 Score: " + points.ToString();
	}

	private IEnumerator Pulse() {
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
