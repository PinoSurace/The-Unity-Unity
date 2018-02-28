using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Script : MonoBehaviour {

    public Text player_lives;
    public Text player_name;
    public Text player_scores;
    public Text timer;
    int timer_value;
    int score_value;
    // Use this for initialization
    void Start () {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        player_lives.text = data.GetNumOfLives().ToString();
        player_name.text = data.GetPlayerName();
        timer_value = 0;
        timer.text = "00";
        player_scores.text = "0";
        score_value = 0;
        player_lives.text = data.GetNumOfLives().ToString();
        StartCoroutine(TimerAndScores());




    }
	
	// Update is called once per frame
	void Update () {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        if (player_lives.text != data.GetNumOfLives().ToString())
        {
            player_lives.text = data.GetNumOfLives().ToString();
            timer.text = "00";
            timer_value = 0;
        }       
            


    }

    IEnumerator TimerAndScores()
    {       

        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            timer_value += 1;
            timer.text = timer_value.ToString();
            if(timer_value %10 == 0)
            {
                score_value += 1;
                player_scores.text = score_value.ToString();
            }            

        }
    }
}
