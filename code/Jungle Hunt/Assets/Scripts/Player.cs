using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    public float playerSpeed;
    public Vector2 jumpHeight;

    public Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            rb.AddForce(new Vector2(-2, 10), ForceMode2D.Impulse);
            animator.SetTrigger("PlayerJump");
            
        }
    }
}
