using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Next_Scene : MonoBehaviour {

	//Load the next scene. Ĺevel is the index of the scene in build settings

	public void Change_scene (int level)
    {
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().ChangeScene(level);
    }

    public void StartGame()
    {
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().GenerateLevelOrder(false);
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().NextLevel();
    }

}
