using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverScreen : MonoBehaviour {
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameController.gameRunning) {
			float elapsedTime = GameObject.Find ("ScoreDisplay").GetComponent<ScoreDisplay> ().elapsedTime;
			Color c = GetComponent<Image> ().color;
			c.a += Time.deltaTime;
			GetComponent<Image>().color = c;
			transform.GetChild (0).GetComponent<Text> ().text = "Game Over" + System.Environment.NewLine + "You survived for" + elapsedTime.ToString("0.00")
				+ " months."+ System.Environment.NewLine + "Press Enter to restart";
		}
	}
}
