using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_Game_Difficulty : MonoBehaviour {

    public Text difficulty;
    public Slider slider;
    public int gamedifficulty = 100;
    public int lives = 0;
	// Use this for initialization
	void Start () {
        slider.value = 3;
	}
	
	// Update is called once per frame
	void Update () {
        switch ((int)slider.value)
        {
            case 1:
                difficulty.text = "Difficulty: Baby";
                gamedifficulty = 50;
                lives = 10;
                break;
            case 2:
                difficulty.text = "Difficulty: Easy";
                gamedifficulty = 75;
                lives = 5;
                break;
            case 3:
                difficulty.text = "Difficulty: Normal";
                gamedifficulty = 100;
                lives = 5;
                break;
            case 4:
                difficulty.text = "Difficulty: Hard";
                gamedifficulty = 150;
                lives = 5;
                break;
            case 5:
                difficulty.text = "Difficulty: Mad";
                gamedifficulty = 200;
                lives = 3;
                break;
            case 6:
                difficulty.text = "Difficulty: Hardcore";
                gamedifficulty = 300;
                lives = 1;
                break;
            default:
                break;
        }
		
	}

    public void Save()
    {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        data.GetComponent<DataContainer_Character>().SetLives(lives);
        data.SetDifficulty(gamedifficulty);

    }
}
