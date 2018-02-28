using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Action : MonoBehaviour {

    private bool player_In = false;
    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("collision name = " + other.gameObject.name);
        if (other.gameObject.name == "Tarzan")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.CurrentState = Player.State.State_Bubble;
            player.transform.position = this.gameObject.transform.position;
            player.transform.Translate(0, 1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y * 0.000001f);
            this.gameObject.transform.Translate(0, 1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y * 0.000001f);
            //player.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
            player_In = true;
        }

        

    }

    /*private void OnDestroy()
    {
        Player player = GameObject.Find("Tarzan").GetComponent<Player>();
        if(player_In == true)
        {
            player.CurrentState = Player.State.State_Swimming;
        }
        
    }*/

    

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tarzan")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.transform.position = this.gameObject.transform.position;
            player.transform.Translate(0,1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y*0.1f);
            this.gameObject.transform.Translate(0,1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y * 0.1f);
            //player.transform.Translate(0, -1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y * 0.0000009f);
            //this.gameObject.transform.Translate(0, -1, this.gameObject.GetComponent<Rigidbody2D>().velocity.y * 0.0000009f);
            //player.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
            player_In = true;
        }
    }
}
