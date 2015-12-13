using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {
	private GameController gameController;
	public float elapsedTime;

	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
	// Update is called once per frame
	void Update () {
		if (gameController.gameRunning) {
			elapsedTime = Time.time - gameController.startTime;
			GetComponent<Text> ().text = "Time: " + elapsedTime.ToString("0.00");
		}
	}
}
