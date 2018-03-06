﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRope : MonoBehaviour {
	//This Script could be added straight to the player script

	//Distance the player keeps from the rope
	float dist = 0f;
	void OnTriggerEnter2D (Collider2D other)
	{
		//Checking if the objecdt has parent object (Parts o rope are children of the rope)
		if (other.gameObject.transform.parent != null)
		{

			//Checking if the collider is a rope and that the player is not swinging
			if (other.gameObject.transform.parent.name.StartsWith ("Rope") &&
			    other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ().grabbed == false) 
			{
                RopeGrabbed grabscript = other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed>();
                //We create a new distance joint between the player and the rope

                var joint = gameObject.AddComponent <DistanceJoint2D> ();
				joint.connectedBody = other.GetComponent<Rigidbody2D> ();
				joint.autoConfigureConnectedAnchor = true;
				joint.distance = dist;

                //Add points based on rope grabbed
                grabscript.IncreaseScore(other.gameObject.name);

				//Set the rope as grabbed
				grabscript.grabbed = true;
				grabscript.CorrelateCameraToThisObject ();
				//Change the player state to swinging
				this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Swinging;
				this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerSwing");

			} 
		}

		//If the trigger is the animation collider
		else if (other.gameObject.name == "DivingAnimationCollider")
		{
			
			Destroy (this.gameObject.GetComponent (typeof(DistanceJoint2D)));
			this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_None;
			this.gameObject.GetComponent<Animator> ().Play ("PlayerFall");
		}
		//Trigger for level change
		else if (other.gameObject.name == "NextLevelCollider") 
		{
           GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().NextLevel();
           Debug.Log ("End");
		}
	}
}
