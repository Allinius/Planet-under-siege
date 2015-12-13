using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	public GameObject targetObject;
	public float speed;
	public float turnSpeed;
	public bool friendly;
	public GameObject trailPrefab;
	public ParticleSystem explosionPrefab;
	public Sprite friendlyMissile;
	public Sprite enemyMissile;
	public AudioClip explosionSound;

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
	}
		
	void Update () {
		if (gameController.gameRunning) {
			if (targetObject && targetObject.activeInHierarchy) {
				Vector3 toTarget = targetObject.transform.position - transform.position;
				float angle = Vector3.Angle (transform.up, toTarget);
				float rotateAngle = Mathf.Min (angle, turnSpeed);
				Vector3 cross = Vector3.Cross(transform.up, toTarget);
				if (cross.z < 0) {
					rotateAngle = -rotateAngle;
				} 
				transform.Rotate (new Vector3 (0, 0, rotateAngle));
			} else {
				targetObject = null;
			}

			transform.position += transform.up * speed * Time.deltaTime;
			cleanUpOffscreen ();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (enabled){
			if (col.gameObject.tag == "enemy" && friendly) {
				col.gameObject.SendMessage ("Hit");
				Explode ();
			} else if (col.gameObject.tag == "planet" && !friendly) {
				col.gameObject.SendMessage ("Hit");
				Explode ();
			} else if(col.gameObject.tag =="shield") {
				Explode ();
			}
			/* else if(col.gameObject.tag =="missile" && (friendly^col.gameObject.GetComponent<Missile>().friendly)) {
				col.gameObject.GetComponent<Missile> ().Explode ();
				Explode ();
			}*/
		}
	}

	public void Explode() {
		if (transform.childCount > 0) {
			Destroy (transform.GetChild (0).gameObject, transform.GetChild (0).gameObject.GetComponent<TrailRenderer>().time - 0.01f);
			transform.DetachChildren ();
		}
		Instantiate (explosionPrefab, transform.position, Quaternion.identity);
		gameObject.SetActive (false);
	}

	private void cleanUpOffscreen() {
		Vector3 topRight = Camera.main.ViewportToWorldPoint (new Vector3 (1.1f, 1.1f, -Camera.main.transform.position.z));
		if (transform.position.magnitude > topRight.magnitude) {
			Explode ();
		}
	}
}
