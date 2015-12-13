using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour {
	private float timeLeft;

	// Use this for initialization
	void Awake () {
		timeLeft = GetComponent<ParticleSystem> ().startLifetime;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft <=0) {
			GameObject.Destroy (gameObject);
		}
	}


}
