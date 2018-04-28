using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour {
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
		//Checking if the object has parent object (Parts of rope are children of the rope)
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
				this.gameObject.GetComponent<Player> ().ManageState(Player.State.State_Swinging);
				this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerSwing");

			}

            else if (other.gameObject.name == "CrocodileScore")
            {
                float dist_to_reduce = 0.45f;
                Vector3 crocpos = other.transform.root.position;
                float crocdist = Mathf.Abs(this.transform.position.y - crocpos.y);
                GameObject chardata = GameObject.Find("CharacterData");
                int scoretype = 7;
                if (chardata != null)
                {
                    crocdist -= dist_to_reduce;
                    while (crocdist >= dist_to_reduce)
                    {
                        crocdist -= dist_to_reduce;
                        scoretype -= 1;
                    }
                    if (scoretype >= 0)
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(scoretype);
                    }
                    else
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(0);
                    }
                }
                Destroy(other.GetComponent<BoxCollider2D>());

            }

            else if (other.gameObject.name == "ScoreSystem")
            {
                int riskbonus = 0;
                float riskfactor = this.transform.position.x + 10;
                while (riskfactor >= 3.50f && riskbonus < 3)
                {
                    riskbonus++;
                    riskfactor -= 3.50f;
                }
                float dist_to_reduce = 0.20f;
                Vector3 boulderpos = other.transform.root.position;
                float dodgedist = Mathf.Abs(this.transform.position.y - boulderpos.y);

                GameObject chardata = GameObject.Find("CharacterData");

                int scoretype = 4;
                if (chardata != null)
                {
                    dodgedist -= (dist_to_reduce * 1.5f);
                    while (dodgedist >= dist_to_reduce)
                    {
                        dodgedist -= dist_to_reduce;
                        scoretype -= 1;
                    }
                    if (scoretype >= 0)
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(scoretype + riskbonus);
                    }
                    else
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(0 + riskbonus);
                    }
                }
                Destroy(other.GetComponent<BoxCollider2D>());

            }

            else if (other.gameObject.name == "Head_Score")
            {
                float dist_to_reduce = 0.24f;
                Vector3 headpos = other.transform.root.position;
                float headdist = Mathf.Abs(this.transform.position.y - headpos.y);
                GameObject chardata = GameObject.Find("CharacterData");
                int scoretype = 7;
                if (chardata != null)
                {
                    headdist -= (dist_to_reduce * 2.0f);
                    while (headdist >= dist_to_reduce)
                    {
                        headdist -= dist_to_reduce;
                        scoretype -= 1;
                    }
                    if (scoretype >= 0)
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(scoretype);
                    }
                    else
                    {
                        chardata.GetComponent<DataContainer_Character>().AwardPoints(0);
                    }
                }
                Destroy(other.GetComponent<BoxCollider2D>());

            }
        }

        //If the trigger is the animation collider at the end of level 1
		else if (other.gameObject.name == "DivingAnimationCollider" && this.gameObject.GetComponent<Player> 
			().CurrentState == Player.State.State_Swinging)
		{
			//Joint to rope is destroyed, player state set to helpless and player falls into the water
			Destroy (this.gameObject.GetComponent (typeof(DistanceJoint2D)));
			this.gameObject.GetComponent<Player> ().ManageState(Player.State.State_Helpless);
			this.gameObject.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
			this.gameObject.GetComponent<Rigidbody2D> ().AddForce(new Vector2(this.gameObject.GetComponent<Player> ().XAxis/2
				, 0), ForceMode2D.Impulse);
			this.gameObject.GetComponent<Animator> ().Play ("PlayerFall");
		}

        //Trigger for level change
        else if (other.gameObject.name == "NextLevelCollider") 
		{
           
           GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().NextLevel();
           this.gameObject.GetComponent<Player>().ManageState(Player.State.State_Inv);
		}

        //Trigger for level 4 end where player jumps towards the cage
        if (other.gameObject.name == "SavedSceneCollider")
        {
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(this.gameObject.GetComponent<Player>().XAxis / 2
                , this.gameObject.GetComponent<Player>().YAxis), ForceMode2D.Impulse);
            this.gameObject.GetComponent<Animator>().Play("PlayerJump");
            
            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();
            if (manager.inStory == false)
            {
                manager.NextLevel();
            }
            else
            {
                manager.ChangeScene(6);
            }
            this.gameObject.GetComponent<Player>().ManageState(Player.State.State_Inv);
        }
    }

    //Triggers for levels 3 and 4
	void OnCollisionEnter2D (Collision2D other)
	{
        //Trigger in level 3. When the player hits the ground, they continue running
		if (other.gameObject.name == "GroundCollider" && this.gameObject.GetComponent<Player>
            ().CurrentState != Player.State.State_Crouching)
		{
			this.gameObject.GetComponent<Player> ().ManageState(Player.State.State_Running);
			this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerRun");
		}
        //Trigger in level 4. When the player hits the ground, they return to idle state
        else if (other.gameObject.name == "LevelGenerator4")
		{
			this.gameObject.GetComponent<Player> ().ManageState(Player.State.State_Running);
			this.gameObject.GetComponent<Animator> ().SetTrigger ("PlayerRun");
		}

	}
}
