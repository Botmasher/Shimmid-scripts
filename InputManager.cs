using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public GameObject player;
	[Range(0,500)]public float playerSpeed = 100f;

	float horiz;
	float vert;
	bool action;


	void Start () {
	}


	void Update () {

		// check for button inputs
		horiz = Input.GetAxis ("Horizontal");
		vert = Input.GetAxis ("Vertical");
		action = Input.GetButtonDown ("Jump");

		if (action) {
			if (player.GetComponent<Animator> ().GetBool ("isShell")) {
				player.GetComponent<Animator> ().SetBool ("isShell", false);
			} else {
				player.GetComponent<Animator> ().SetBool ("isShell", true);
			}
		} else if (Mathf.Abs (horiz) > 0f) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (horiz * playerSpeed * Time.deltaTime, 0f));
		} else {
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}

		if (vert != 0f) {}

	}

}
