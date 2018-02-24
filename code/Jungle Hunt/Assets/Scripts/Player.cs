using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //Values for the force used to move the player
    public int YAxis;   //How high the player will jump.
    public int XAxis;   //How far the player will jump. Value should be negative in first level 

    public Rigidbody2D rb;
    private Animator animator;
	public float speed;

	//Different States of the Player
	public enum State
	{
		State_Idle,
		State_Jumping,
		State_Dead,
		State_Attacking,
		State_Swinging,
		State_Swimming,
		State_Bubble,
		State_Walking,
		State_Crouching
	}


	public State CurrentState;

	//Function for Changing States. Every switch statement is a different state.
	private void ChangeState(string input)
	{
		switch (CurrentState) 
		{
			case State.State_Idle:
				if (input == " ")
				{
					CurrentState = State.State_Jumping;
					//An impulse is used to move the player
					rb.AddForce (new Vector2 (XAxis, YAxis), ForceMode2D.Impulse);
					//The jump animation is triggered
					animator.SetTrigger ("PlayerJump");
					this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2 (0.03f, 0.03f);
				}
					
				else if (input == "s")
				{
					CurrentState = State.State_Swimming;
					animator.SetTrigger ("PlayerSwim");
				}
				break;

			case State.State_Dead:				
				
				break;
			case State.State_Swimming:
				
				if (input == "w") 
				{
					//rb.velocity = Vector2.zero;
					rb.AddForce (new Vector2 (0, 10 * speed));
					
				}

				else if (input == "s") 
				{
					//rb.velocity = Vector2.zero;
					rb.AddForce (new Vector2 (0, -10 * speed));
					
				}

				else if (input == "a") 
				{
					//rb.velocity = Vector2.zero;
					rb.AddForce (new Vector2 (-10 * speed, 0));
					
				}

				else if (input == "d") 
				{
					//rb.velocity = Vector2.zero;
					rb.AddForce (new Vector2 (10 * speed,0));
					
				}
				break;

			case State.State_Swinging:
			
				if (input == " ") {
					CurrentState = State.State_Jumping;
					Destroy (gameObject.GetComponent (typeof(DistanceJoint2D)));
					//An impulse is used to move the player
					
					rb.velocity = Vector2.zero;
					rb.AddForce (new Vector2 (XAxis-2, YAxis-1.5f), ForceMode2D.Impulse);
					//The jump animation is triggered
					animator.SetTrigger ("PlayerJump");
					
				}
				break;
				
		}

	}

	//When The player dies, they jump up
	public void DeadlyHazard()
	{ 
		if (CurrentState != State.State_Dead)
		{
			rb.AddForce (new Vector2 (0, YAxis), ForceMode2D.Impulse);
			animator.SetTrigger ("PlayerDead");
			CurrentState = State.State_Dead;
			rb.gravityScale = 1;
		}
	}
		

    //We create shortcuts for animator and rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		CurrentState = State.State_Idle;
    }

    //Pressing the space key makes the player jump.
    private void Update()
    {
		//Check pressed buttons
		ChangeState (Input.inputString);

		//Make the player unable to move of the gamescene. Current values are for level 2
		//Vector2 pos = transform.position;
		//pos.x = Mathf.Clamp(pos.x, -7, 8);
		//pos.y = Mathf.Clamp(pos.y, -7, 1);
		//transform.position = pos;
        
    }
}
