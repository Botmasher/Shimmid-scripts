using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HazardSpawn : MonoBehaviour {

	// set behavior of this hazard spawner
	public List<GameObject> hazards = new List <GameObject>();
	public enum hazardTypes {rocks, spikes, fire};
	public hazardTypes hazardType;
	public bool fromAbove=true;

	// specific hazard to instantiate
	private GameObject hazard;				// hazard chosen from hazards list
	private GameObject thisHazard;			// instance of hazard

	// time to wait between spawning hazards
	private float spawnCountdown;


	void Start () {

		// determine type of hazard chosen
		switch (hazardType) {
			case hazardTypes.rocks:
				hazard = hazards[0];
				break;
			case hazardTypes.spikes:
				hazard = hazards[1];
				break;
			case hazardTypes.fire:
				hazard = hazards[2];
				break;
			default:
				break;
		}

	}
	

	void Update () {
		if (spawnCountdown <= 0f) {
			spawnCountdown = 0.5f;
			thisHazard = Instantiate (hazard, new Vector3 (Random.Range(this.transform.position.x-5f,this.transform.position.x+5f), this.transform.position.y, 0f) , Quaternion.identity) as GameObject;
		} else {
			spawnCountdown -= Time.deltaTime;
		}
	}

}
