using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGrabbed : MonoBehaviour {

	public bool grabbed = false;

    private float animtime = 1.0f;

    private int score_min = 25;
    private int score_var = 75;
    
    public void IncreaseScore(string objname)
    {
        string digit = objname.Substring(objname.Length - 1, 1);
        int num = int.Parse(digit);
        int score_change = score_min + (score_var * (num - 1));
        GameObject chardata = GameObject.Find("CharacterData");
        if (chardata != null)
        {
            chardata.GetComponent<DataContainer_Character>().ChangePoints(score_change);
        }
    }

    public void CorrelateCameraToThisObject()
    {
        Vector3 target = this.transform.position;
        GameObject.Find("Main Camera").GetComponent<CameraControls>().FollowPlayer(animtime);
    }
}
