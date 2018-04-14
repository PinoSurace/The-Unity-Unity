using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    // Use this for initialization
    public Text Score_1;
    public Text Score_2;
    public Text Names_1;
    public Text Names_2;
    public Text Score_Now;

	void Start ()
    {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        string left_scores = "";
        string left_names = "";
        string right_scores = "";
        string right_names = "";
        for (int a = 0; a < data.scores.Count; a++)
        {
            if (a < (DataContainer_Character.SAVESLOTS / 2))
            {
                left_scores = string.Concat(left_scores, string.Format("{0}\n", data.scores[a]));
                left_names = string.Concat(left_names, string.Format("{0} :\n", data.nicknames[a]));
            }
            else
            {
                right_scores = string.Concat(right_scores, string.Format("{0}\n", data.scores[a]));
                right_names = string.Concat(right_names, string.Format("{0} :\n", data.nicknames[a]));
            }
        }
        Score_1.text = left_scores;
        Score_2.text = right_scores;
        Names_1.text = left_names;
        Names_2.text = right_names;
        Score_Now.text = string.Format("{0}", data.GetPoints());
	}
}
