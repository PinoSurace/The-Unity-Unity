using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Script : MonoBehaviour {

    private GameObject scores_player;
    private GameObject scores_points;
    private GameObject scores_time;
    private DataContainer_Character data;

    private int tickPer = 0;
    private int beforePoints = 0;

    private bool active = false;
    private bool halt = false;
    private bool clock = false;
    private bool clock_on = false;
    private int timecounter;

    // Use this for initialization
    void Start ()
    {
        
    }
	
    public void Activate(GameObject player, GameObject points, GameObject time, GameObject dataref)
    {
        if (!active)
        {
            scores_player = player;
            scores_points = points;
            scores_time = time;
            data = dataref.GetComponent<DataContainer_Character>();
            scores_player.GetComponent<Text>().text = data.GetPlayerName();
            active = true;
        }
    }

    public void TimerOn(int initialtime)
    {
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
            scores_points.GetComponent<Text>().text = string.Format("{0,8}", points);
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
