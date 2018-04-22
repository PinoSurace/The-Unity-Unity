using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessInBubbleScript : MonoBehaviour {
    public float upForce = 200f;
    public bool isDead = false;

    private Rigidbody2D rb;
    private Animator anim;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.ResetTrigger("Died");

		
	}
	
	// Update is called once per frame
	void Update () {
        if (isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(0, upForce));
        }

        
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        anim.SetTrigger("Died");
    }
}
