using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRope : MonoBehaviour {
	//This Script could be added straight to the player script

	//Distance the player keeps from the rope
	float dist = 0f;
	void OnTriggerEnter2D (Collider2D other){
		//Checking if the objecdt has parent object (Parts o rope are children of the rope)
		if (other.gameObject.transform.parent != null)
		{

			//Checking if the collider is a rope and that the player is not swinging
			if (other.gameObject.transform.parent.name.StartsWith ("Rope") &&
			    other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ().grabbed == false) 
			{			
				//We create a new distance joint between the player and the rope

				var joint = gameObject.AddComponent <DistanceJoint2D> ();
				joint.connectedBody = other.GetComponent<Rigidbody2D> ();
				joint.autoConfigureConnectedAnchor = true;
				joint.distance = dist;

				//Set the rope as grabbed
				other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ().grabbed = true;
				other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ().CorrelateCameraToThisObject ();
				//Change the player state to swinging
				this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Swinging;
				this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerSwing");

			} 
		}

		//If the trigger is the animation collider
		else if (other.gameObject.name == "DivingAnimationCollider")
		{
			
			Destroy (this.gameObject.GetComponent (typeof(DistanceJoint2D)));
			this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Dead;
			this.gameObject.GetComponent<Animator> ().Play ("PlayerFall");
		}
		//Trigger for level change
		else if (other.gameObject.name == "NextLevelCollider") 
		{
			Debug.Log ("End");
		}
	}
}
