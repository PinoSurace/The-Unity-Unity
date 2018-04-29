using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataContainer_Character : MonoBehaviour {

    public delegate void bcCharcterGameOver();
    public static event bcCharcterGameOver EVGameOver;

    // SaveData information.
    public static int SAVESLOTS = 12;

    public List<int> scores;
    public List<string> nicknames;
    public int plays = 0;
    public bool cheat;

    // Prefab for score effect.
    public Transform scorePrefab;
    // Canvas to draw prefab on.
    public Canvas canvas;

    // Internal Counters.
    private int runner = 0; // Used for multiple simultanious score effects.

    // Data to be stored.
    private string playerName = "";
    private int points;
    private int numOfLives = 5;

    // Difficulty stored as an integer, should be considered a percentage. 100 is 100% [normal].
    private int difficulty = 100;
    private int DIFFICULTYRAISE = 15;

    // Determines Scores. Score is MAXSCORE - SCOREDETRIMENT*SCORERANKINDEX.
    private int MAXSCORE = 450;
    private int SCOREDETRIMENT = 50;

    // Score Effect Variables, controls the look of the spawned effects.
    private List<string> scoreNames = new List<string>{"Xtraordinary" ,"Saintly", "Superb", "Awesome", "Beautiful", "Cool", "Decent", "Eradic" };

    private List<Color> scoreColors = new List<Color> { new Color(0.80f, 0.85f, 0.20f), new Color(0.70f, 0.75f, 0.30f), new Color(0.60f, 0.65f, 0.40f), new Color(0.80f, 0.55f, 0.10f),
    new Color(0.90f, 0.75f, 0.70f), new Color(0.30f, 0.35f, 0.95f), new Color(0.30f, 0.70f, 0.70f), new Color(0.40f, 0.45f, 0.20f)};
    private List<Color> textColors = new List<Color> { new Color(0.90f, 0.20f, 0.20f), new Color(0.90f, 0.90f, 0.90f), new Color(0.80f, 0.80f, 0.00f), new Color(0.50f, 0.25f, 0.20f),
    new Color(0.20f, 0.85f, 0.85f), new Color(0.25f, 0.25f, 0.75f), new Color(0.60f, 0.60f, 0.10f), new Color(0.10f, 0.10f, 0.10f)};

    public List<int> scoresAwarder = new List<int> { };
    public List<int> actualScores = new List<int> { };

    // Keep any changes through transitions.
    void Start ()
    {
        if (GameObject.FindGameObjectsWithTag("Container").Length != 2)
        {
            Destroy(this.gameObject);
        }
        else
        {
            EVGameOver = null;
        }
        DontDestroyOnLoad(this);
	}

    public string GetRankName(int rank)
    {
        string retval = scoreNames[rank];
        return retval;
    }

    public Color GetRankColor1(int rank)
    {
        Color retval = scoreColors[rank];
        return retval;
    }

    public Color GetRankColor2(int rank)
    {
        Color retval = textColors[rank];
        return retval;
    }

    public void SetPlayerName(string value)
    {
        playerName = value;
    }

    public string GetPlayerName()
    {
        return playerName;
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

            scoresAwarder.Add(index);
            actualScores.Add(scoretogive);
            int alive = GameObject.FindGameObjectsWithTag("ScoreInstance").Length;

            if (alive != 0 && runner < 10)
            {
                runner += 1;
            }
            else
            {
                runner = 0;
            }

            ScoreIndicator_Effect scoreToShow = Instantiate(scorePrefab, Vector3.zero, Quaternion.Euler(0, 0, 0)).GetComponent<ScoreIndicator_Effect>();

            scoreToShow.gameObject.transform.SetParent(canvas.transform, false);
            scoreToShow.gameObject.GetComponent<RectTransform>().Translate((Screen.width * 3 / 10), (Screen.height / 5) - (runner * 15f), 0);
            scoreToShow.flairtext = scoreNames[index];
            scoreToShow.pointtext = scoretogive;
            scoreToShow.starcolor = textColors[index];
            scoreToShow.textcolor = scoreColors[index];

            ChangePoints(scoretogive);
        }
    }

    public int GetPoints()
    {
        return points;
    }

    public void SetLives(int value)
    {
        if (value >= 0)
        {
            numOfLives = value;
        }
        
    }

    public void ChangeLives(int change)
    {
        numOfLives += change;
        
        if (numOfLives <= 0)
        {
            numOfLives = 0;

            // Subscribe for game over events.
            if (EVGameOver != null)
            {
                SaveResult();
                EVGameOver();
            }
        }
    }

    public int GetNumOfLives()
    {
        return numOfLives;
    }

    public void SetDifficulty(int value)
    {
        if(value > 0)
        {
            difficulty = value;
        }
        if (!cheat)
        {
            MAXSCORE = 250 + (difficulty * 2);
            SCOREDETRIMENT = (difficulty / 2);
        }
        else
        {
            MAXSCORE = 100;
            SCOREDETRIMENT = 10;
        }

    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void DifficultyUp()
    {
        difficulty += DIFFICULTYRAISE;
        if (!cheat)
        {
            MAXSCORE = 250 + (difficulty * 2);
            SCOREDETRIMENT = (difficulty / 2);
        }
        
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
                newNicks.Add(playerName);
                isHighScore = true;
            }
            newScores.Add(scores[i]);
            newNicks.Add(nicknames[i]);
        }
        // If current score not added:
        if (!isHighScore)
        {
            newScores.Add(points);
            newNicks.Add(playerName);
        }
        // Check if capacity is exceeded.
        while (newNicks.Count > SAVESLOTS)
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
        // Clear previous data.
        scores.Clear();
        nicknames.Clear();

        Data save = SaveLoadManager.LoadData();
        // If savedata exists, populate scores and names with them.
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
