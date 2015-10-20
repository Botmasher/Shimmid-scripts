using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public static bool shielded;
	public static int health;
	public UnityEngine.UI.Text scoreText;

	// Use this for initialization
	void Start () {
		health = 100;
		shielded = false;
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Health: "+health;
	}
}
