using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_PlayerName : MonoBehaviour {

    public DataContainer_Character data;
    public Text nameinput;

	public void Save()
    {
        string playername = nameinput.text;
        data.SetPlayerName(playername);
        Debug.Log(string.Concat("Data sent, here's it's load return: ", data.GetPlayerName()));
	}
}
