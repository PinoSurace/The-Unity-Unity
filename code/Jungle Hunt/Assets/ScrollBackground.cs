using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour {

    private Rigidbody2D rb;
    public float scrollSpeed = -1.5f;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(scrollSpeed, 0);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
