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

    // Store data.
    string PlayerName = "";
}
