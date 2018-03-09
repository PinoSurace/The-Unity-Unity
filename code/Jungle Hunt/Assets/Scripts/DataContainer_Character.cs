using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataContainer_Character : MonoBehaviour {

    public delegate void bcCharcterGameOver();
    public static event bcCharcterGameOver EVGameOver;

    // Store data.
    private string PlayerName = "";
    private int points;
    public int num_of_lives = 5;
    private int difficulty = 1;

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

    public void ChangePoints(int change)
    {
        points += change;
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetLives(int value)
    {
        num_of_lives = value;
    }

    public void ChangeLives(int change)
    {
        num_of_lives += change;
        if (num_of_lives <= 0)
        {
            // Subscribe for game over events.
            if (EVGameOver != null)
            {
                EVGameOver();
            }
        }
    }

    public int GetNumOfLives()
    {
        return num_of_lives;
    }

    public void SetDifficulty(int value)
    {
        difficulty = value;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }
}
