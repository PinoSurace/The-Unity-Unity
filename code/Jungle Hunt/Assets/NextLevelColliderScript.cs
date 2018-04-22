using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelColliderScript : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>().NextLevel();
    }
}
