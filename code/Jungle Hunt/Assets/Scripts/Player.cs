using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    // Delecate to BroadCast character state changing to dead.
    public delegate void bcCharcterDeath();
    // The event to declare once player is dead.
    public static event bcCharcterDeath EVDeath;

    // Values for the force used to move the player
    public float YAxis;   //How high the player will jump.
    public float XAxis;   //How far the player will jump.
    public float ColliderSize;

    public Rigidbody2D rb;
    private Animator animator;
    public float speed;

    // Placeholder to differentiate levels 3 and 4. If the current level can be extracted from scene manager, replace this with that
	public int level;

    // Different States of the Player
    public enum State
    {
        State_Idle,
        State_Jumping,
        State_Swinging,
        State_Swimming,
        State_Attacking,
        State_Running,
        State_Crouching,
        State_Helpless,
        State_Inv,
        State_Dead,
    }

    private State currentState = State.State_Helpless;
    public State CurrentState
    {
        get
        {
            return currentState;
        }
    }
    private int statePriority = -1;

    // Function for Changing States. Every switch statement is a different state.
    private void ChangeState()
    {
        switch (CurrentState) 
        {
            case State.State_Idle:
                if (Input.GetButtonDown("Jump")) 
                {
                    ManageState(State.State_Jumping);
                    // An impulse is used to move the player
                    rb.AddForce(new Vector2 (XAxis, YAxis), ForceMode2D.Impulse);
                    // The jump animation is triggered
                    animator.SetTrigger("PlayerJump");
                    this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(ColliderSize, ColliderSize);
                }

                //Changes the player from idle to running
				if (Input.GetButtonDown("Horizontal") == true && level == 4)
                {
                    ManageState(State.State_Running);
                    animator.SetTrigger("PlayerRun");
                }
                break;

            case State.State_Dead:
                // Initiating an event with 0 subscribers is not allowed.
                if (EVDeath != null)
                {

                    GameObject.Find("SoundSystem").GetComponent<Sound_System>().PlaySFX(0);

                    EVDeath();
                    EVDeath = null;
                }
                break;

            case State.State_Swimming:
                float moveHorizontal = Input.GetAxis("Horizontal");
                float moveVertical = Input.GetAxis("Vertical");
                Vector2 movement = new Vector2(moveHorizontal, moveVertical);
                rb.AddForce(movement * speed);
                break;

            case State.State_Swinging:
                if (Input.GetButtonDown("Jump"))
                {
                    ManageState(State.State_Jumping);
                    Destroy(gameObject.GetComponent(typeof(DistanceJoint2D)));
                    //An impulse is used to move the player
                    rb.velocity = Vector2.zero;
                    rb.AddForce(new Vector2(XAxis * 1.2f, YAxis * 0.8f), ForceMode2D.Impulse);
                    //The jump animation is triggered
                    animator.SetTrigger("PlayerJump");
                }
                //Players velocity is checked and the swinhinh animation changes according to the
                //direction player is swinging
                if (rb.velocity.x < -1)
                {
                    animator.SetFloat("Direction", -1.0f);
                }
                else if (rb.velocity.x > 1)
                {
                    animator.SetFloat("Direction", 1.0f);
                }
                break;

            case State.State_Crouching:
                //When the duck button is no longer pressed, the player starts running again
                if (Input.GetButtonDown("Duck"))
                {
                    ManageState(State.State_Running);
                    animator.SetTrigger("PlayerRun");
                    this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.18f, 0.18f);
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
                }
                break;

            case State.State_Running:
                // Player can accelerate and deccelerate while "moving"
                float accelerate = Input.GetAxis("Horizontal");
                Vector2 acceleration = new Vector2(accelerate, 0);
                rb.AddForce(acceleration * speed);

                // Pressing and holding the duck button makes player crouch
                if (Input.GetButtonDown("Duck"))
                {
                    ManageState(State.State_Crouching);
                    animator.SetTrigger("PlayerDuck");
                    this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.06f, 0.06f);
                    this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (0, -0.06f);
                }
                //Pressing the jump button makes the player jump
                else if (Input.GetButtonDown("Jump"))
                {
                    rb.velocity = Vector2.zero;
                    ManageState(State.State_Jumping);
                    //An impulse is used to move the player
                    rb.AddForce(new Vector2(XAxis, YAxis), ForceMode2D.Impulse);
                    //The jump animation is triggered
                    animator.SetTrigger("PlayerJump");
                }
                else if (Input.GetButton("Horizontal") == false && level == 4)
                {
                    ManageState(State.State_Idle);
                    animator.SetTrigger("PlayerStop");
                }
                break;
        }
    }

    // Only allows higher priority state changes to pass in the same frame.
    public void ManageState(Player.State state)
    {
        int priority = (int)state;
        if (statePriority < priority)
        {
            currentState = state;
            statePriority = priority;
        }
    }

    // When The player dies, they jump up
    public void DeadlyHazard()
    {
        if (CurrentState != State.State_Dead)
        {
            rb.AddForce(new Vector2 (0, YAxis), ForceMode2D.Impulse);
            animator.SetTrigger("PlayerDead");
            ManageState(State.State_Dead);
            rb.gravityScale = 1;
            Destroy (gameObject.GetComponent (typeof(Collider2D)));
        }
    }

    // When The player is in the bubble animation changes
    public void InTheBubble()
    {
        ManageState(State.State_Helpless);
        animator.SetTrigger("PlayerIdle");
    }

    // When The player exits from the bubble animation changes
    public void OutOfBubble()
    {
        ManageState(State.State_Swimming);
        animator.SetTrigger("PlayerSwim");
    }

    // We create shortcuts for animator and rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // If we can access Scene Manager (started from menu) change according to level.
        try
        {
            int level = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().currentIndex - 1;
            if (level == 2)
            {
                OutOfBubble();
            }
            else if(level == 5)
            {
                animator.SetTrigger("PlayerCelebrate");
                ManageState(State.State_Helpless);
            }
            else
            {
                ManageState(State.State_Idle);
            }
        }
        catch
        {
            // If errored, default to idle.
            ManageState(State.State_Idle);
        }
    }

   
    private void Update()
    {
        //Check pressed buttons
        ChangeState ();
        statePriority = -1;

        if (CurrentState == State.State_Swimming)
        {
            //Make the player unable to move of the gamescene. Current values are for level 2
            Vector2 pos = transform.position;
            //pos.x = Mathf.Clamp(pos.x, -7, 8);
            pos.y = Mathf.Clamp(pos.y, -5, 10);
            //transform.position = pos;
        }
    }

    private void OnDestroy()
    {
        EVDeath = null;
    }
}
