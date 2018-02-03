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

    //We create shortcuts for animator and rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //Pressing the space key makes the player jump.
    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            //An impulse is used to move the player
            rb.AddForce(new Vector2(XAxis, YAxis), ForceMode2D.Impulse);
            //The jump animation is triggered
            animator.SetTrigger("PlayerJump");
            
            
        }
    }
}
