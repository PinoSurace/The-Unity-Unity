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
        StartCoroutine("WaitForCompletion");
    }

    IEnumerator WaitForCompletion()
    {
        yield return new WaitForSeconds(3);

        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().FinishedLoad();

        yield return new WaitForSeconds(3);
    }
}
