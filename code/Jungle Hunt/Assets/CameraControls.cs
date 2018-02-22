using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    private Vector3 startposition;
    private Vector3 moveposition;
    private Vector3 targetposition;
    private float movetime;
    private bool called = false;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Debug Controls allow you to offset with numpad.
	void Update ()
    {
        
        if (called)
        {
            this.transform.position += moveposition;
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
        targetposition = toPosition;
        movetime = time;
        startposition = this.transform.position;
        float divider = time * (1/Time.deltaTime);
        float x_diff = (targetposition.x - startposition.x) / divider;
        // y or z - does not need to change:
        float y_diff = 0;
        float z_diff = 0;
        moveposition = new Vector3(x_diff, y_diff, z_diff);
        StartCoroutine("Seize");
    }

    IEnumerator Seize()
    {
        called = true;
        yield return new WaitForSeconds(movetime);
        called = false;
    }

    private void GetCommands()
    {
        
    }
}
