using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    private Vector3 startposition;
    private Vector3 moveposition;
    private Vector3 targetposition;
    private Vector3 offset = new Vector3(-5.0f, 0, 0);
    private float movetime;
    private bool called = false;
    private bool followplayer = false;
	
	// Debug Controls allow you to offset with numpad.
	void FixedUpdate ()
    {
        
        if (called)
        {
            
            startposition = this.transform.position;
            float divider = movetime;
            if (followplayer == true)
            {
                targetposition = GameObject.Find("Tarzan").transform.position + offset;
            }
            float x_diff = ((targetposition.x - startposition.x) * Time.deltaTime) / divider;
            // y or z - does not need to change:
            float y_diff = 0;
            float z_diff = 0;
            if (x_diff < 0)
            {
                moveposition = new Vector3(x_diff, y_diff, z_diff);
                this.transform.position += moveposition;
            }
            if (this.transform.position.x - targetposition.x < 1.0f && followplayer == false)
            {
                called = false;
            }
        }
		if (Input.GetKey(KeyCode.Keypad4))
        {
            this.transform.position += new Vector3(-1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad6))
        {
            this.transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            this.transform.position += new Vector3(0, -1, 0);
        }
        else if (Input.GetKey(KeyCode.Keypad8))
        {
            this.transform.position += new Vector3(0, 1, 0);
        }
    }

    public void GotoTarget(Vector3 toPosition, float time)
    {
        targetposition = toPosition + offset;
        movetime = time;
        called = true;
    }

    public void FollowPlayer(float time)
    {
        movetime = time;
        called = true;
        followplayer = true;
    }

    private void GetCommands()
    {
        
    }
}
