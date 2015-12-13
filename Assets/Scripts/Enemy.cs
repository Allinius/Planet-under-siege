using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float speed;
	public float noSpawnZoneRadius;
	public GameObject explosionPrefab;
	public Sprite[] sprites;

	public float missileSpawnInterval;
	private GameController gameController;
	private GameObject planet;
	private GameObject missilePrefab;
	private Vector3 targetMovePosition;
	private float lastMissileSpawn;


	void OnEnable() {
		GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, 4)];
		targetMovePosition = GenerateValidMovePosition ();
		missileSpawnInterval = Random.Range (2.0f, 6.0f);
	}

	void Start() {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		planet = GameObject.FindGameObjectWithTag("planet");
		missilePrefab = gameController.missilePrefab;
		missileSpawnInterval = Random.Range (1.0f, 5.0f);
	}
	// Update is called once per frame
	void Update () {
		if (gameController.gameRunning) {
			if ((transform.position - targetMovePosition).magnitude < 0.1f) {
				targetMovePosition = GenerateValidMovePosition ();
			}
			transform.position = Vector3.MoveTowards(transform.position, targetMovePosition, speed*Time.deltaTime);

			if (Time.time - lastMissileSpawn > missileSpawnInterval && missilePrefab) {
				if (planet){
					lastMissileSpawn = Time.time;
					Vector3 toPlanet = planet.transform.position - transform.position;

					float angle = Vector3.Angle (transform.up, toPlanet);
					if (Vector3.Cross(transform.up, toPlanet).z < 0) {
						angle = -angle;
					} 
					gameController.SpawnMissile(transform.position, Quaternion.AngleAxis (angle, Vector3.forward), planet, false);
					missileSpawnInterval = Random.Range (1.0f, 5.0f);
				}
			}
		}
	}

	public Vector3 GenerateValidPosition() {
		while(true) {
			Vector3 randomPosition = Camera.main.ViewportToWorldPoint (new Vector3 (Random.Range (0.0f, 1.0f),
				Random.Range (0.0f, 1.0f), -Camera.main.transform.position.z));
			if (Mathf.Sqrt(Mathf.Pow(randomPosition.x,2) + Mathf.Pow(randomPosition.y,2)) > noSpawnZoneRadius) {
				return randomPosition;
			}
		}
	}

	public Vector3 GenerateValidMovePosition() {
		while(true){
			Vector3 pos = GenerateValidPosition ();
			Vector3 halfSpaceAxis = Vector3.Cross (transform.position, Vector3.forward);
			Vector3 cross = Vector3.Cross (pos - (Vector3.ClampMagnitude(transform.position, noSpawnZoneRadius)), halfSpaceAxis);
			if (cross.z < 0) {
				return pos;
			}
		}
	}

	void OnDrawGizmosSelected() {
		//Gizmos.DrawWireSphere (new Vector3 (0, 0, 0), noSpawnZoneRadius);
		//Gizmos.DrawWireSphere (targetMovePosition, 0.2f);
		
		//Vector3 halfSpaceAxis = Vector3.Cross (transform.position, Vector3.forward);
		//Gizmos.DrawLine (Vector3.zero, halfSpaceAxis);
	}

	public void Hit() {
		Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		gameObject.SetActive (false);
	}
}
