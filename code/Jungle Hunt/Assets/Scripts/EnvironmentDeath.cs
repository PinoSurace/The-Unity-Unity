using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDeath : MonoBehaviour {

	//This script is used in deadly obstacles(Crocodiles, ground in level 1, cannibals etc)
    //When player hits colliders with this script, player will die
	void OnCollisionEnter2D (Collision2D other)
    {
		if (other.gameObject.name == "Tarzan") 
		{
			Player player = other.gameObject.GetComponent <Player>();
			player.DeadlyHazard ();
		}
	}
}