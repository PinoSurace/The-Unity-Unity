using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrincessInBubbleScript : MonoBehaviour {
    public float upForce = 200f;
    public bool isDead = false;
    private int countstart = 80;

    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private Player player;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.ResetTrigger("Died");

        player.ManageState(Player.State.State_Helpless);
	}
	
	// Update is called once per frame
	void Update () {
        if (countstart > 0)
        {
            countstart--;
            rb.velocity = Vector2.zero;
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        float dist_to_reduce = 0.30f;
        Vector3 columnspos = collision.transform.root.position;
        float headdist = Mathf.Abs(this.transform.position.y - columnspos.y);
        GameObject chardata = GameObject.Find("CharacterData");
        int scoretype = 7;
        if (chardata != null)
        {
            headdist += (dist_to_reduce * 0.8f);
            while (headdist <= 2.50f)
            {
                headdist += dist_to_reduce;
                scoretype -= 1;
            }
            if (scoretype >= 0)
            {
                chardata.GetComponent<DataContainer_Character>().AwardPoints(scoretype);
            }
            else
            {
                chardata.GetComponent<DataContainer_Character>().AwardPoints(0);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.StartsWith("Castle"))
        {
            GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().NextLevel();
            player.ManageState(Player.State.State_Inv);
        }
        else
        {
            isDead = true;
            rb.velocity = Vector2.zero;
            anim.SetTrigger("Died");
            player.ManageState(Player.State.State_Dead);
        }
        
    }
}
