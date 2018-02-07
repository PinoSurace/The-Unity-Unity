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

	enum State
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

	private State CurrentState;

	private void ChangeState(string input)
	{
		switch (CurrentState) 
		{
			case State.State_Idle:
				if (input == " ") {
					print("Jee2");
					CurrentState = State.State_Jumping;
					//An impulse is used to move the player
					rb.AddForce (new Vector2 (XAxis, YAxis), ForceMode2D.Impulse);
					//The jump animation is triggered
					animator.SetTrigger ("PlayerJump");
				}
				break;				

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

		ChangeState (Input.inputString);
        
    }
}
