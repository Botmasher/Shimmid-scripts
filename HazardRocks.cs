using UnityEngine;
using System.Collections;

public class HazardRocks : MonoBehaviour {
	
	void Start () {
		StartCoroutine("SelfDestruct");
	}


	IEnumerator SelfDestruct () {
		yield return new WaitForSeconds (2.5f);
		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (!Player.shielded && collision.collider.gameObject.tag == "Player") {
			Player.health -= 10;
		}
		Destroy (this.gameObject);
	}

}
