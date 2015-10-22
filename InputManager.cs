using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputManager : MonoBehaviour {
	
	// player variables
	public GameObject player;				// player to manipulate with input
	public GameObject world;				// world to manipulate with input
	public GameObject safetyNet;			// platform to instantiate below player on action
	private GameObject thisNet;				// point to instance of safetyNet

	[Range(0,500)]
	public float playerSpeed = 100f;		// multiplier to player movement
	public float actionTimer;				// how long to cooldown between actions
	private float coolDown;					// cooldown counter to count down from actiontimer

	// set player states
	public bool movingPlayer=false;			// is the input moving the player or the world?
	public bool restingPlayer=false;		// set the player character to idle

	// track press timing
	float pressTime;											// how long since last beat keypress
	public static List<float> pressAvgs = new List<float> ();	// average time between keypresses (used to judge good/bad beatkeeping)

	float vert;								// get vertical axis
	Vector3 worldRot;						// amount to rotate the world this update
	bool pressingAction1;					// whether or not player is pressing an action button
	bool pressingAction2;					// whether or not player is pressing an action button


	void Start () {
		// set action countdown clock
		coolDown = actionTimer;
	}


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
		// set pitch (and, by extension, x-mvmt) based on press frequency
		// start pitching up
		if (Input.GetButtonDown("Jump")) {
			MusicManager.pitch = 1f;
			pressAvgs.Add (pressTime);		// add to list of presstimes to judge beatkeeping skills
			pressTime = 0f;					// reset time since last press
		// wait a bit for button rhythm window before changing pitch
		} else if (pressTime < 0.4f) {
			pressTime += Time.deltaTime;
		// start pitching down
		} else {
			MusicManager.pitch = 0f;
			pressTime += Time.deltaTime;
		}

		//vert = Input.GetAxis ("Vertical");
		pressingAction1 = Input.GetButtonDown ("Fire2");
		pressingAction2 = Input.GetButtonDown ("Fire1");
	}

	/**
	 *	Use inputs to change player movement and actions
	 */
	void ActPlayer (){
		// animation and behavior on pressing action button
		if (pressingAction1) {
			if (player.GetComponent<Animator> ().GetBool ("isShell")) {
				player.GetComponent<Animator> ().SetBool ("isShell", false);
			} else {
				player.GetComponent<Animator> ().SetBool ("isShell", true);
			}
		// animation and movement on pressing axes
		} else if (MusicManager.pitch > 0f) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (MusicManager.pitch * playerSpeed * Time.deltaTime, 0f));
		// idle state
		} else {
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}
	}

	/**
	 *	Use inputs to change world movement and actions
	 */
	void ActWorld (){
		coolDown -= Time.deltaTime;
		// if player action time is ready
		if (coolDown <= 0f) {
			// reset player
			player.GetComponent<Animator> ().SetBool ("isShell", false);
			Player.shielded = false;
			MusicManager.flangeRate = 0.1f;
			// check for inputs and perform actions
			if (pressingAction1) {
				// shell protection
				coolDown=actionTimer;
				player.GetComponent<Animator> ().SetBool ("isShell", true);
				// tell player that Shimmid is protected
				Player.shielded = true;
			} else if (pressingAction2) {
				// place platform below
				coolDown=actionTimer;
				thisNet = Instantiate (safetyNet, new Vector2 (player.transform.position.x+1f,player.transform.position.y-0.86f), Quaternion.Euler(Vector3.zero)) as GameObject;
				MusicManager.flangeRate = 10f;
				// parent to world for tilt and actions
				//thisNet.transform.parent = world.transform;
			}
		} else {
			// do no actions
		}

		// move non-idle, non-shelled player
		if (!restingPlayer && !player.GetComponent<Animator>().GetBool ("isShell")) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			if (MusicManager.pitch >= 0.8f) {
				player.GetComponent<Animator> ().SetBool ("isRunning", true);
			} else {
				player.GetComponent<Animator> ().SetBool ("isRunning", false);
			}
			// move player from song pitch
			player.transform.position = Vector2.Lerp (player.transform.position, new Vector2 (player.transform.position.x + 0.004f + MusicManager.pitch*5f, player.transform.position.y), 0.5f * Time.deltaTime);
			//player.transform.Translate (new Vector2(0.004f + MusicManager.pitch*5f*Time.deltaTime, 0f));
		// idle player if not shelled
		} else if (!player.GetComponent<Animator>().GetBool ("isShell")) {
			// set idle animation and movement
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}

	}

}
