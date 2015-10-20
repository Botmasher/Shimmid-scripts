using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	public GameObject player;
	public GameObject world;
	[Range(0,500)]public float playerSpeed = 100f;

	float horiz;
	float vert;
	bool pressingAction;
	bool movingPlayer=false;
	bool restingPlayer=false;
	

	void Update () {

		GetInputs ();
  		
		// decide to control player or world
		if (!movingPlayer) {
			ActWorld();
		} else {
			ActPlayer();
		}

	}

	/**
	 * 	Check for input
	 */
	void GetInputs () {
		horiz = Input.GetAxis ("Horizontal");
		vert = Input.GetAxis ("Vertical");
		pressingAction = Input.GetButtonDown ("Jump");
	}

	/**
	 *	Use inputs to change player movement and actions
	 */
	void ActPlayer (){
		if (pressingAction) {
			if (player.GetComponent<Animator> ().GetBool ("isShell")) {
				player.GetComponent<Animator> ().SetBool ("isShell", false);
			} else {
				player.GetComponent<Animator> ().SetBool ("isShell", true);
			}
		} else if (horiz > 0f) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (horiz * playerSpeed * Time.deltaTime, 0f));
		} else {
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}
		
		// unused
		if (vert != 0f) {}
	}

	/**
	 *	Use inputs to change world movement and actions
	 */
	void ActWorld (){
		if (pressingAction) {
			// do some action
		}

		// rotate world around z with axis input
		world.transform.rotation = Quaternion.Lerp (world.transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, -horiz*20f)), 4f*Time.deltaTime);

		if (!restingPlayer) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			player.transform.Translate (new Vector2 ((0.1f+-horiz * playerSpeed * (world.transform.rotation.z*0.05f)) * Time.deltaTime, 0f) );
		} else {
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}

	}

}
