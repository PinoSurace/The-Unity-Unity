using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDeath : MonoBehaviour {

	//When the Hazard collides with the player
	void OnCollisionEnter2D (Collision2D other){
		if (other.gameObject.name == "Tarzan") 
		{
			Player player = other.gameObject.GetComponent <Player>();
			player.DeadlyHazard ();

		}

	}

}