using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	Transform player;

	void Start () {
		// grab player
		player = GameObject.FindGameObjectWithTag ("Player").transform as Transform;
	}

	void Update () {
		// follow along with player's x-y
		this.transform.position = Vector3.Lerp (this.transform.position, new Vector3 (player.position.x, player.position.y, this.transform.position.z), 2f*Time.deltaTime);
	}
}
