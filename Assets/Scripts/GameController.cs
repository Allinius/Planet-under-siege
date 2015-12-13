using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	public GameObject enemyPrefab;
	public GameObject missilePrefab;
	public int pooledEnemies;
	public int pooledMissiles;
	public List<GameObject> enemies;
	public List<GameObject> missiles;
	public float startTime;
	public bool gameRunning;

	private float enemyBatchSize;
	private float lastEnemySpawnTime;
	private GameObject shield;
	private AudioSource audio;


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		enemyBatchSize = 3;
		gameRunning = true;
		startTime = Time.time;
		shield = GameObject.FindGameObjectWithTag ("shield");
		enemies = new List<GameObject> ();
		for (int i = 0; i < pooledEnemies; i++) {
			GameObject obj = Instantiate (enemyPrefab);
			obj.SetActive (false);
			obj.transform.parent = gameObject.transform;
			enemies.Add (obj);
		} 
		for (int i = 0; i < enemyBatchSize; i++) {
			SpawnEnemy (GenerateOffscreenPosition ());
		} 
		lastEnemySpawnTime = Time.time;

		missiles = new List<GameObject> ();
		for (int i = 0; i < pooledMissiles; i++) {
			GameObject obj = Instantiate (missilePrefab);
			obj.SetActive (false);
			obj.transform.parent = gameObject.transform;
			missiles.Add (obj);
		}
	}
		

	// Update is called once per frame
	void Update () {
		if (gameRunning) {
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
				shield.transform.Rotate(Vector3.forward * 360 * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
				shield.transform.Rotate (Vector3.forward * -360 * Time.deltaTime);
			}

			if (Time.time - lastEnemySpawnTime > 3.0f) {
				lastEnemySpawnTime = Time.time;

				for (int i = 0; i < enemyBatchSize; i++) {
					SpawnEnemy (GenerateOffscreenPosition ());
				}
			}
			audio.pitch += 0.01f * Time.deltaTime;
		} else {
			audio.pitch = 0.3f;
			if (Input.GetKey(KeyCode.Return)) {
				SceneManager.LoadScene (SceneManager.GetActiveScene().name);
			}
		}


	}

	public void SpawnMissile(Vector3 position, Quaternion rotation, GameObject target, bool friendly) {
		for (int i = 0; i < missiles.Count; i++) {
			if(!missiles[i].activeInHierarchy) {
				missiles [i].transform.position = position;
				missiles [i].transform.rotation = rotation;
				missiles [i].GetComponent<Missile> ().targetObject = target;
				missiles [i].GetComponent<Missile> ().friendly = friendly;
				missiles [i].SetActive (true);
				GameObject trail = Instantiate (missiles [i].GetComponent<Missile> ().trailPrefab, position + new Vector3(0,0,1f), rotation) as GameObject;
				trail.transform.parent = missiles [i].transform;
				if (friendly) {
					missiles [i].GetComponent<SpriteRenderer> ().sprite = missiles [i].GetComponent<Missile> ().friendlyMissile;
					trail.GetComponent<TrailRenderer> ().material.color = new Color (0, 0.6f, 0.8f);
				} else {
					missiles [i].GetComponent<SpriteRenderer> ().sprite = missiles [i].GetComponent<Missile> ().enemyMissile;;
					trail.GetComponent<TrailRenderer> ().material.color = new Color (1, 0, 0);
				}
				break;
			}
		}
	}

	public void SpawnEnemy(Vector3 position) {
		for (int i = 0; i < enemies.Count; i++) {
			if (!enemies[i].activeInHierarchy) {
				enemies [i].transform.position = position;
				enemies [i].SetActive (true);
				break;
			}
		}
	}

	private Vector3 GenerateOffscreenPosition() {
		Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, 1.1f, -Camera.main.transform.position.z));
		Vector3 topRight = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, 1.1f, -Camera.main.transform.position.z));
		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, -Camera.main.transform.position.z));
		Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, -0.1f, -Camera.main.transform.position.z));
		switch(Random.Range(0,4)) {
		case 0:
			return new Vector3 (topLeft.x, Mathf.Lerp (bottomLeft.y, topLeft.y, Random.Range (0.0f, 1.0f)), 0);
		case 1:
			return new Vector3 (Mathf.Lerp (topLeft.x, topRight.y, Random.Range (0.0f, 1.0f)), topRight.y, 0);
		case 2:
			return new Vector3 (topRight.x, Mathf.Lerp (topRight.y, bottomRight.y, Random.Range (0.0f, 1.0f)), 0);
		case 3:
			return new Vector3 (Mathf.Lerp (bottomLeft.x, bottomRight.x, Random.Range (0.0f, 1.0f)), bottomLeft.y, 0);
		}
		return new Vector3 ();
	}
}
