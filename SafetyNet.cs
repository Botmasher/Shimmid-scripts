using UnityEngine;
using System.Collections;

public class SafetyNet : MonoBehaviour {

	public float lifetime=2f;
	private bool isBlinking;

	void Start() {
		StartCoroutine (SelfDestruct(lifetime));
	}

	void Update () {
		lifetime -= Time.deltaTime;
		// start flashing shortly before disappearing
		if (!isBlinking && lifetime < 1f) {
			StartCoroutine ("Blink");
		}
	}

	// destroy object once lifetime expires
	IEnumerator SelfDestruct (float countdown) {
		yield return new WaitForSeconds (lifetime);
		Destroy (this.gameObject);
		yield return null;
	}

	// flash off and on rapidly
	IEnumerator Blink () {
		isBlinking = true;
		GetComponent<MeshRenderer>().enabled = false;
		yield return new WaitForSeconds (lifetime*0.1f);
		GetComponent<MeshRenderer>().enabled = true;
		isBlinking = false;
	}

}
