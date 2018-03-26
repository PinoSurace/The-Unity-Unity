using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIndicator_Effect : MonoBehaviour {

    GameObject flair;
    GameObject pointamount;

    public Color starcolor = Color.black;
    public Color textcolor = Color.red;
    public string flairtext = "Dreadful";
    public int pointtext = 100;

    private int alive = 0;

	// Use this for initialization
	void Start ()
    {
        flair = this.transform.Find("ScoreBubble_Text").gameObject;
        pointamount = this.transform.Find("ScoreBubble_Points").gameObject;

        this.GetComponent<Image>().color = starcolor;
        flair.GetComponent<Text>().color = textcolor;
        flair.GetComponent<Text>().text = flairtext;
        pointamount.GetComponent<Text>().text = string.Format("{0, 3}", pointtext);

        flair.SetActive(false);
        pointamount.SetActive(false);
        GameObject.Find("SoundSystem").GetComponent<Sound_System>().PlaySFX(2);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (alive == 10)
        {
            flair.SetActive(true);
        }
        else if (alive >= 70)
        {
            Destroy(this.gameObject);
        }
        else if (alive >= 30)
        {
            pointamount.SetActive(true);
            Vector3 newposition = new Vector3(pointamount.transform.position.x, pointamount.transform.position.y + 1.2f, pointamount.transform.position.z);
            pointamount.transform.SetPositionAndRotation(newposition, Quaternion.Euler(0, 0, 0));
        }
        this.GetComponent<Rigidbody2D>().rotation += 3.00f;
        flair.GetComponent<Rigidbody2D>().rotation = 0.00f;

        alive += 1;
	}
}
