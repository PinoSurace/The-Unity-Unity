using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour {

    public int CurrentIndex;
    public delegate void bcSceneChange();
    public static event bcSceneChange EVSceneChange;
    private int goingTo;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
        CurrentIndex = SceneManager.GetActiveScene().buildIndex;
	}

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            CurrentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(CurrentIndex);
        }
    }

    public void ChangeScene(int toScene)
    {
        if (true == true)
        {
            goingTo = toScene;

            if (EVSceneChange != null)
            {
                EVSceneChange();
            }

        }
    }

    public void FinishedLoad()
    {
        // Change to accomodate any modifications in loadorder.
        // Should match index of "Start menu"
        if (CurrentIndex == 2)
        {
            GameObject.Find("Button").SetActive(false);
            GameObject.Find("InputField").SetActive(false);
        }
        SceneManager.LoadScene(goingTo);
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Raise");
    }
}
