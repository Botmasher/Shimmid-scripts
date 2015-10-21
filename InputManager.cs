using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {
	
	// player variables
	public GameObject player;				// player to manipulate with input
	public GameObject world;				// world to manipulate with input
	public GameObject safetyNet;			// platform to instantiate below player on action
	private GameObject thisNet;				// point to instance of safetyNet
	public UnityEngine.Audio.AudioMixer master;

	[Range(0,500)]
	public float playerSpeed = 100f;		// multiplier to player movement
	public float actionTimer;				// how long to cooldown between actions
	private float coolDown;					// cooldown counter to count down from actiontimer

	// set player states
	public bool movingPlayer=false;			// is the input moving the player or the world?
	public bool restingPlayer=false;		// set the player character to idle

	float horiz;							// get horizontal axis
	float pressWeight;						// how much beat keypress contributes to speed
	float pressTime;						// how long since last beat keypress

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
		// take horizontal from left/right keys
		//horiz = Input.GetAxis ("Horizontal");

		// set horizontal based on press frequency
		if (Input.GetButtonDown("Jump")) {
			pressWeight = 1f;
			pressTime = 0f;
		} else if (pressTime < 0.4f) {
			pressWeight = pressWeight;
			pressTime += Time.deltaTime;
		} else {
			pressWeight = 0f;
			pressTime += Time.deltaTime;
		}
		horiz = Mathf.Lerp (horiz, pressWeight, Time.deltaTime);

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
		} else if (horiz > 0f) {
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			player.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (horiz * playerSpeed * Time.deltaTime, 0f));
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
			Player.shielded=false;
			// check for inputs and perform actions
			if (pressingAction1) {
				// shell protection
				coolDown=actionTimer;
				player.GetComponent<Animator> ().SetBool ("isShell", true);
				// tell player that Shimmid is protected
				Player.shielded = true;
				// prevent player movement while shelled
				horiz = 0f;
			} else if (pressingAction2) {
				// place platform below
				coolDown=actionTimer;
				thisNet = Instantiate (safetyNet, new Vector2 (player.transform.position.x+1f,player.transform.position.y-0.86f), Quaternion.Euler(Vector3.zero)) as GameObject;
				// parent to world for tilt and actions
				//thisNet.transform.parent = world.transform;
			}
		} else {
			// do no actions
		}

		// rotate world around z in proportion to positive axis input
		//worldRot = (horiz > 0f) ? new Vector3(0f,0f,-horiz*20f) : Vector3.zero;
		//world.transform.rotation = Quaternion.Lerp (world.transform.rotation, Quaternion.Euler(worldRot), 4f*Time.deltaTime);

		// move non-idle, non-shelled player
		if (!restingPlayer && !player.GetComponent<Animator>().GetBool ("isShell")) {
			master.SetFloat("MasterPitch", horiz*1.5f);
			player.GetComponent<Animator> ().SetBool ("isWalking", true);
			if (horiz >= 0.8f) {
				player.GetComponent<Animator> ().SetBool ("isRunning", true);
			} else {
				player.GetComponent<Animator> ().SetBool ("isRunning", false);
			}
			// walk at pace influenced by world rotation and player speed
			// SPEED OR SLOW DEPENDING ON ROTATING WORLD
			//player.transform.Translate (new Vector2(0.004f + -horiz*playerSpeed*(world.transform.rotation.z*0.1f)*Time.deltaTime, 0f));

			player.transform.Translate (new Vector2(0.004f + horiz*5f*Time.deltaTime, 0f));
		// idle player if not shelled
		} else if (!player.GetComponent<Animator>().GetBool ("isShell")) {
			// set idle animation and movement
			player.GetComponent<Animator> ().SetBool ("isWalking", false);
		}

	}

}
