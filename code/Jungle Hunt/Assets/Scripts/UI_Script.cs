using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Script : MonoBehaviour {

    private GameObject scores_player;
    private GameObject scores_points;
    private GameObject scores_time;
    private GameObject heart1;
    private GameObject heart2;
    private GameObject heart3;
    private GameObject heart4;
    private GameObject heart5;

    private Color heartx0;
    private Color heartx1;
    private Color heartx2;
    private Color heartx3;

    private DataContainer_Character data;

    private int tickPer = 0;
    private int beforePoints = 0;
    private int beforeLives = 0;

    private bool active = false;
    private bool halt = false;
    private bool clock = false;
    private bool clock_on = false;
    public int timecounter;
    public int timemax;

    // Use this for initialization
    void Start ()
    {
        heartx0 = new Color(0.05f, 0.05f, 0.05f, 0.80f);
        heartx1 = new Color(0.95f, 0.00f, 0.00f, 1.00f);
        heartx2 = new Color(0.95f, 0.90f, 0.00f, 1.00f);
        heartx3 = new Color(0.95f, 0.90f, 0.80f, 1.00f);
    }
	
    public void Activate(GameObject player, GameObject points, GameObject time, GameObject lives, GameObject dataref)
    {
        if (!active)
        {
            scores_player = player;
            scores_points = points;
            scores_time = time;
            heart1 = lives.transform.Find("LifeMod1").gameObject;
            heart2 = lives.transform.Find("LifeMod2").gameObject;
            heart3 = lives.transform.Find("LifeMod3").gameObject;
            heart4 = lives.transform.Find("LifeMod4").gameObject;
            heart5 = lives.transform.Find("LifeMod5").gameObject;
            data = dataref.GetComponent<DataContainer_Character>();
            // life_count.GetComponent<Text>().text = string.Format("{0, 2}", data.GetNumOfLives());
            active = true;
        }
        scores_player.GetComponent<Text>().text = data.GetPlayerName();
    }

    public void TimerOn(int initialtime)
    {
        timemax = initialtime;
        timecounter = initialtime;
        clock = true;
        SetTimerText();
        StartCoroutine(Timer());
    }

    public void TimerOff()
    {
        clock = false;
        clock_on = false;
    }

    private void SetTimerText()
    {
        int minutes = (timecounter / 60);
        int seconds = (timecounter % 60);
        string result;
        if (minutes < 10 && seconds < 10)
        {
            result = string.Concat("0", minutes, ":", "0", seconds);
        }
        else if (minutes < 10)
        {
            result = string.Concat("0", minutes, ":", seconds);
        }
        else if (seconds < 10)
        {
            result = string.Concat(minutes, ":", "0", seconds);
        }
        else
        {
            result = string.Concat(minutes, ":", seconds);
        }
        scores_time.GetComponent<Text>().text = result;
    }

    public void ScoreDrop(int toScore)
    {
        halt = true;
        int change = toScore - data.GetPoints();
        beforePoints = data.GetPoints();
        tickPer = change / 20;
        StartCoroutine(TickDown());
        data.SetPoints(toScore);
    }

	// Update is called once per frame
	void Update ()
    {
        if (active && !halt)
        {
            int points = data.GetPoints();
            int lives = data.GetNumOfLives();
            scores_points.GetComponent<Text>().text = string.Format("{0,8}", points);
            if (lives != beforeLives)
            {
                Color off = heartx0;
                Color on = heartx1;
                // Create for other settings.
                if ((lives / 5) == 1)
                {
                    off = heartx1;
                    on = heartx2;
                }
                else if ((lives / 5) == 2)
                {
                    off = heartx2;
                    on = heartx3;
                }
                heart1.GetComponent<Image>().color = off;
                heart2.GetComponent<Image>().color = off;
                heart3.GetComponent<Image>().color = off;
                heart4.GetComponent<Image>().color = off;
                heart5.GetComponent<Image>().color = off;
                if ((lives % 5) > 0)
                {
                    heart1.GetComponent<Image>().color = on;
                }
                if ((lives % 5) > 1)
                {
                    heart2.GetComponent<Image>().color = on;
                }
                if ((lives % 5) > 2)
                {
                    heart3.GetComponent<Image>().color = on;
                }
                if ((lives % 5) > 3)
                {
                    heart4.GetComponent<Image>().color = on;
                }
                if ((lives % 5) > 4)
                {
                    heart5.GetComponent<Image>().color = on;
                }
            }
        }
    }

    IEnumerator TickDown()
    {
        int iterations = 0;
        while (iterations < 20)
        {
            iterations++;
            yield return new WaitForSeconds(0.05f);
            scores_points.GetComponent<Text>().text = string.Format("{0,8}", beforePoints + (tickPer * iterations));
        }
        halt = false;
    }

    IEnumerator Timer()
    {
        if (!clock_on)
        {
            while (clock)
            {
                clock_on = true;
                yield return new WaitForSeconds(1.00f);
                if (timecounter > 0)
                {
                    timecounter -= 1;
                }
                SetTimerText();
            }
        }
    }
}
