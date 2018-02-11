using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeComponent_Image : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Scene_Manager.EVSceneChange += DropImage;
	}

    private void OnDestroy()
    {
        Scene_Manager.EVSceneChange -= DropImage;
    }

    void DropImage()
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Drop");
    }
}
