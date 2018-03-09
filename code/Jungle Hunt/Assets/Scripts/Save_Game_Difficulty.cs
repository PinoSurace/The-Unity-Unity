using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_Game_Difficulty : MonoBehaviour {

    public Text difficulty;
    public Slider slider;
	// Use this for initialization
	void Start () {
        slider.value = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (slider.value == 1)
        {
            difficulty.text = "Difficulty: Easy";
        }
        else if (slider.value == 2)
        {
            difficulty.text = "Difficulty: Medium";
        }
        else if(slider.value == 3)
        {
            difficulty.text = "Difficulty: Hard";
        }
		
	}

    public void Save()
    {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        if (slider.value == 1)
        {
            data.GetComponent<DataContainer_Character>().SetLives(7);
        }
        else if (slider.value == 2)
        {
            data.GetComponent<DataContainer_Character>().SetLives(5);
        }
        else
        {
            data.GetComponent<DataContainer_Character>().SetLives(3);
        }
        data.SetDifficulty((int)slider.value);
    }
}
