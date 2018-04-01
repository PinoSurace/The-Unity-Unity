using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannibal_Script : MonoBehaviour
{
    public SpriteRenderer head_sprite;
    public SpriteRenderer body_sprite;
    public Rigidbody2D r;
    private float speed;    
    int direction;
    float maxDist;
    float minDist;
    
    // Use this for initialization
    void Start()
    {
        
        r = GetComponent<Rigidbody2D>();
        speed = 1.0f; 
        direction = 1;
        maxDist =5 ;
        minDist = -5;

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Tarzan")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.DeadlyHazard();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //r.AddForce(new Vector2(speed * direction, 0.0f));
        r.velocity = new Vector2(speed * direction, 0.0f);
        if (r.OverlapPoint(new Vector2(maxDist, r.position.y)))
        {
            //r.transform.eulerAngles = new Vector3(0, 180, 0);
            head_sprite.flipX = !head_sprite.flipX;
            body_sprite.flipX = !body_sprite.flipX;
            direction = -1;    
        }
        else if (r.OverlapPoint(new Vector2(minDist, r.position.y)))
        {
            head_sprite.flipX = !head_sprite.flipX;
            body_sprite.flipX = !body_sprite.flipX;
            direction = 1;            
        }
        
    }
}
