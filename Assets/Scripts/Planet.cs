using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {
	public GameObject missilePrefab;
	public float missileSpawnInterval;
	public GameObject explosionPrefab;

	private GameObject[] enemies;
	private float lastMissileSpawn;
	private GameController gameController;


	// Use this for initialization
	void Start () {
		lastMissileSpawn = Time.time;
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.gameRunning) {
			enemies = GameObject.FindGameObjectsWithTag ("enemy");
			if (Time.time - lastMissileSpawn > missileSpawnInterval && enemies.Length > 0 && missilePrefab) {
				foreach (GameObject enemy in enemies) {
					if (enemy){
						lastMissileSpawn = Time.time;
						Vector3 toEnemy = enemy.transform.position - transform.position;

						float angle = Vector3.Angle (transform.up, toEnemy);
						if (Vector3.Cross(transform.up, toEnemy).z < 0) {
							angle = -angle;
						} 
						gameController.SpawnMissile(transform.position, Quaternion.AngleAxis (angle, Vector3.forward), enemy, true); 
					}
				}
			}
		}
	}

	public void Hit() {
		Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		gameController.gameRunning = false;
		gameObject.SetActive (false);
	}
}
