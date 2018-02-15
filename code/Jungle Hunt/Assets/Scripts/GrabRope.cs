using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRope : MonoBehaviour {
	//This Script could be added straight to the player script

	//Distance the player keeps from the rope
	float dist = 0.2f;
	void OnTriggerEnter2D (Collider2D other){
		//Checking if the collider is a rope and that the player is not swinging
		if (other.gameObject.transform.parent.name == "Rope" && 
			this.gameObject.GetComponent<Player>().CurrentState != Player.State.State_Swinging)
		{			
			//We create a new distance joint between the player and the rope
			var joint = gameObject.AddComponent <DistanceJoint2D>();
			joint.connectedBody = other.GetComponent<Rigidbody2D>();
			joint.distance = dist;

			this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Swinging;

		}
	}
}
