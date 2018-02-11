using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeComponent_Particles : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Scene_Manager.EVSceneChange += DropLeaves;

    }

    private void OnDestroy()
    {
        Scene_Manager.EVSceneChange -= DropLeaves;
    }

    void DropLeaves()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        StartCoroutine("WaitForCompletion");
    }

    IEnumerator WaitForCompletion()
    {
        yield return new WaitForSeconds(3);
        
        GameObject.Find("Canvas").GetComponent<Scene_Manager>().FinishedLoad();
        yield return new WaitForSeconds(3);
        gameObject.GetComponent<ParticleSystem>().Clear();
    }
}
