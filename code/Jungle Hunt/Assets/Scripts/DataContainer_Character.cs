﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataContainer_Character : MonoBehaviour {

    public delegate void bcCharcterGameOver();
    public static event bcCharcterGameOver EVGameOver;

    public List<int> scores;
    public List<string> nicknames;

    public int plays = 0;

    public Transform scoreprefab;

    public Canvas canvas;

    private int runner = 0;
    public static int SAVESLOTS = 12;

    // Store data.
    private string PlayerName = "";
    private int points;
    private int num_of_lives = 5;
    private int difficulty = 1;

    // ScoreSystem
    private int MAXSCORE = 450;
    private int SCOREDETRIMENT = 50;

    // ScoreLists
    private List<string> scorenames = new List<string>{"Xtraordinary" ,"Saintly", "Superb", "Awesome", "Beautiful", "Cool", "Decent", "Eradic" };
    private List<Color> scorecolors = new List<Color> { new Color(0.80f, 0.85f, 0.20f), new Color(0.70f, 0.75f, 0.30f), new Color(0.60f, 0.65f, 0.40f), new Color(0.80f, 0.55f, 0.10f),
    new Color(0.90f, 0.75f, 0.70f), new Color(0.30f, 0.35f, 0.95f), new Color(0.30f, 0.70f, 0.70f), new Color(0.40f, 0.45f, 0.20f)};
    private List<Color> textcolors = new List<Color> { new Color(0.90f, 0.20f, 0.20f), new Color(0.90f, 0.90f, 0.90f), new Color(0.80f, 0.80f, 0.00f), new Color(0.50f, 0.25f, 0.20f),
    new Color(0.20f, 0.85f, 0.85f), new Color(0.25f, 0.25f, 0.75f), new Color(0.60f, 0.60f, 0.10f), new Color(0.10f, 0.10f, 0.10f)};
    public List<int> scoresawarder = new List<int> { };
    public List<int> actualscores = new List<int> { };

    // Keep any changes through transitions.
    void Start ()
    {
        if (GameObject.FindGameObjectsWithTag("Container").Length != 2)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
	}

    public string GetRankName(int rank)
    {
        string retval = scorenames[rank];
        return retval;
    }

    public Color GetRankColor1(int rank)
    {
        Color retval = scorecolors[rank];
        return retval;
    }

    public Color GetRankColor2(int rank)
    {
        Color retval = textcolors[rank];
        return retval;
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

    public void AwardPoints(int index)
    {
        if (index < 0 || index > 7)
        {
            Debug.Log("Invalid Index on AwardPoints, 0-7.");
        }
        else
        {
            // Reverse order;
            index = 7 - index;
            int scoretogive = (MAXSCORE - (index * SCOREDETRIMENT));

            scoresawarder.Add(index);
            actualscores.Add(scoretogive);
            
            int alive = GameObject.FindGameObjectsWithTag("ScoreInstance").Length;

            if (alive != 0 && runner < 10)
            {
                runner += 1;
            }
            else
            {
                runner = 0;
            }

            ScoreIndicator_Effect scoreToShow = Instantiate(scoreprefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<ScoreIndicator_Effect>();
            scoreToShow.gameObject.transform.SetParent(canvas.transform, false);
            scoreToShow.gameObject.GetComponent<RectTransform>().Translate(280f, 100f - (runner * 15f), 0);
            scoreToShow.flairtext = scorenames[index];
            scoreToShow.pointtext = scoretogive;
            ChangePoints(scoretogive);
            scoreToShow.starcolor = textcolors[index];
            scoreToShow.textcolor = scorecolors[index];
        }
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

    public void SaveResult()
    {
        bool isHighScore = false;
        List<int> newScores = new List<int>();
        List<string> newNicks = new List<string>();
        // Recreate Ordered List
        for (int i = 0; i < scores.Count; i++)
        {
            if (scores[i] <= points && !isHighScore)
            {
                newScores.Add(points);
                newNicks.Add(PlayerName);
                isHighScore = true;
            }
            newScores.Add(scores[i]);
            newNicks.Add(nicknames[i]);
        }
        if (scores.Count == 0)
        {
            newScores.Add(points);
            newNicks.Add(PlayerName);
        }
        // Check if capacity is exceeded.
        if (newNicks.Count > SAVESLOTS)
        {
            newNicks.RemoveAt(SAVESLOTS);
            newScores.RemoveAt(SAVESLOTS);
        }
        // Save the variables.
        nicknames = newNicks;
        scores = newScores;
        plays += 1;
        // Create the save file.
        SaveLoadManager.SaveData(this);
    }

    public void LoadResult()
    {
        scores.Clear();
        nicknames.Clear();
        Data save = SaveLoadManager.LoadData();
        if (save.contains)
        {
            for (int a = 0; a < save.scores.Length; a++)
            {
                scores.Add(save.scores[a]);
            }

            for (int b = 0; b < save.nicks.Length; b++)
            {
                nicknames.Add(save.nicks[b]);
            }
        }
        plays = save.levelsComplete;
    }
}
