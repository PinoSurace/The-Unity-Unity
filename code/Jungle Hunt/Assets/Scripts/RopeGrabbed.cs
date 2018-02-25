using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGrabbed : MonoBehaviour {

	public bool grabbed = false;

    private float animtime = 1.0f;
    

    public void CorrelateCameraToThisObject()
    {
        Vector3 target = this.transform.position;
        GameObject.Find("Main Camera").GetComponent<CameraControls>().FollowPlayer(animtime);
    }
}
