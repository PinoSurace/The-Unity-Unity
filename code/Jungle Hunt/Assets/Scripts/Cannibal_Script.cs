using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannibal_Script : MonoBehaviour
{
    public SpriteRenderer head_sprite;
    public SpriteRenderer body_sprite;
    public Rigidbody2D r;

    [SerializeField] private List<Sprite> head_sprites;

    public float speed = 1.0f;  
    public int direction = -1;
    public float maxDist = 5;
    public float minDist = -5;

    private float anchor;

    // Use this for initialization
    void Start()
    {
        anchor = this.gameObject.transform.position.x;
        head_sprite.sprite = head_sprites[Random.Range(0, head_sprites.Count)];
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
        if (r.OverlapPoint(new Vector2(anchor + maxDist, r.position.y)))
        {
            //r.transform.eulerAngles = new Vector3(0, 180, 0);
            head_sprite.flipX = !head_sprite.flipX;
            body_sprite.flipX = !body_sprite.flipX;
            direction = -1;    
        }
        else if (r.OverlapPoint(new Vector2(anchor + minDist, r.position.y)))
        {
            head_sprite.flipX = !head_sprite.flipX;
            body_sprite.flipX = !body_sprite.flipX;
            direction = 1;            
        }
        
    }
}
