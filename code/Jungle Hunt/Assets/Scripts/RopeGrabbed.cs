using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGrabbed : MonoBehaviour {

    public bool grabbed = false;
    
    public void IncreaseScore(string objname)
    {
        string digit = objname.Substring(objname.Length - 1, 1);
        int num = int.Parse(digit);
        GameObject chardata = GameObject.Find("CharacterData");
        if (chardata != null)
        {
            chardata.GetComponent<DataContainer_Character>().AwardPoints(num);
        }
    }
}
