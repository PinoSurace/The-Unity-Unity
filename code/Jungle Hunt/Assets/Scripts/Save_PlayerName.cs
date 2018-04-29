using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_PlayerName : MonoBehaviour {

	public void Save()
    {
        DataContainer_Character data = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        string playername = this.GetComponent<Text>().text;
        data.SetPlayerName(playername);
        data.SetPoints(0);
	}
}
