using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble_Action : MonoBehaviour {

    private bool player_In = false;
    private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Tarzan")
        {
            player = collision.gameObject.GetComponent<Player>();            
            player.InTheBubble();
            player.transform.position = this.gameObject.transform.position;            
            player.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
            player_In = true;
        }
        else if(collision.gameObject.name == "WaterLimit")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Tarzan" && player_In == true)
        {
            player = collision.gameObject.GetComponent<Player>();            
            player.GetComponent<Rigidbody2D>().velocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;            
        }
    }  
   
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Tarzan")
        {
            if (player_In == true)
            {
                player.OutOfBubble();                
            }

        }
    }
    
}
