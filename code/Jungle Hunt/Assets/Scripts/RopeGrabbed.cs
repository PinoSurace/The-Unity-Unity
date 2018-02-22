using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeGrabbed : MonoBehaviour {

	public bool grabbed = false;

    private float animtime = 1.5f;
    private Vector3 offset = new Vector3(-1.2f, 0, 0);

    public void CorrelateCameraToThisObject()
    {
        Vector3 target = this.transform.position + offset;
        GameObject.Find("Main Camera").GetComponent<CameraControls>().GotoTarget(target, animtime);
    }
}
