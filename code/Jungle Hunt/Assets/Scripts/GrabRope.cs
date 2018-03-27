using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRope : MonoBehaviour {
	//This Script could be added straight to the player script

	//Distance the player keeps from the rope
	float dist = 0f;
	void OnTriggerEnter2D (Collider2D other)
	{
        // Check that player is not in state_none.
        if (this.GetComponent<Player>().CurrentState == Player.State.State_Inv)
        {
            return;
        }
		//Checking if the objecdt has parent object (Parts o rope are children of the rope)
		if (other.gameObject.transform.parent != null) {

			//Checking if the collider is a rope and that the player is not swinging
			if (other.gameObject.transform.parent.name.StartsWith ("Rope") &&
			    other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ().grabbed == false) {
				RopeGrabbed grabscript = other.gameObject.transform.parent.gameObject.GetComponent<RopeGrabbed> ();
				//We create a new distance joint between the player and the rope

				var joint = gameObject.AddComponent <DistanceJoint2D> ();
				joint.connectedBody = other.GetComponent<Rigidbody2D> ();
				joint.autoConfigureConnectedAnchor = true;
				joint.distance = dist;

				//Add points based on rope grabbed
				grabscript.IncreaseScore (other.gameObject.name);

				//Set the rope as grabbed
				grabscript.grabbed = true;
				//Change the player state to swinging
				this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Swinging;
				this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerSwing");

			}

            else if (other.gameObject.name == "CrocodileScore")
            {
                float dist_to_reduce = 0.50f;
                Vector3 crocpos = other.transform.root.position;
                float crocdist = Mathf.Abs(this.transform.position.y - crocpos.y);
                GameObject chardata = GameObject.Find("CharacterData");
                int scoretype = 7;
                if (chardata != null)
                {
                    while (crocdist >= dist_to_reduce)
                    {
                        crocdist -= dist_to_reduce;
                        scoretype -= 1;
                    }
                    if (scoretype >= 0)
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(scoretype);
                    }
                }
                Destroy(other.GetComponent<BoxCollider2D>());

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
           this.gameObject.GetComponent<Player>().CurrentState = Player.State.State_Inv;
           Debug.Log ("End");
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.name == "GroundCollider")
		{
			this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Running;
			this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerRun");
		}
		else if (other.gameObject.name == "Land")
		{
			this.gameObject.GetComponent<Player> ().CurrentState = Player.State.State_Idle;
			this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerIdle");
		}

	}
}
