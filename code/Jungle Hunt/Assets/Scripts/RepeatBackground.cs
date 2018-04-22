using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour {

    private BoxCollider2D groundCollider;
    private float groundHorizLength;

	// Use this for initialization
	void Start () {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizLength = groundCollider.size.x;
		
	}
	
	// Update is called once per frame
	void Update () {

        if (transform.position.x < -groundHorizLength)
        {
            RepositionBackground();
        }
		
	}

    private void RepositionBackground()
    {
        Vector2 groundOffset = new Vector2(groundHorizLength * 2, 0);
        transform.position = (Vector2)transform.position + groundOffset;
    }
}
