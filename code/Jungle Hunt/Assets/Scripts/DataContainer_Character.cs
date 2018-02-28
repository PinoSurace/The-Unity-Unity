using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContainer_Character : MonoBehaviour {

	// Keep any changes through transitions.
	void Start ()
    {
        DontDestroyOnLoad(this);
	}

    public void SetPlayerName(string value)
    {
        PlayerName = value;
    }

    public string GetPlayerName()
    {
        return PlayerName;
    }

    public void SetPoints(int value)
    {
        points = value;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetNumOfLives(int value)
    {
        num_of_lives = value;
    }

    public void LooseLive()
    {
        num_of_lives--;
    }

    public void GainLive()
    {
        num_of_lives++;
    }

    public int GetNumOfLives()
    {
        return num_of_lives;
    }

    public bool IsDied()
    {
        if (num_of_lives == 0)
            return true;
        else
            return false;
    }



    // Store data.
    public string PlayerName = "";
    int points;
    int num_of_lives = 5; 
}
