using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour {

    public int CurrentIndex;
    public delegate void bcSceneChange();
    public static event bcSceneChange EVSceneChange;
    private int goingTo;
    private bool midLoad = false;
    private bool sceneAnim = true;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
        CurrentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += LvlLoad;
	}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LvlLoad;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            CurrentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(CurrentIndex);
        }
        else if (Input.GetKeyDown("t"))
        {
            CurrentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(CurrentIndex + 1);
        }
        else if (Input.GetKeyDown("e"))
        {
            CurrentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(CurrentIndex - 1);
        }
        else if (Input.GetKeyDown("q"))
        {
            if (sceneAnim == true)
            {
                sceneAnim = false;
                Debug.Log("Animations toggled, current state: False");
            }
            else
            {
                sceneAnim = true;
                Debug.Log("Animations toggled, current state: True");
            }
        }
    }

    public void ChangeScene(int toScene)
    {
        if (toScene >= (SceneManager.sceneCountInBuildSettings))
        {
            Debug.Log("Invalid Call, Trying to access unexisting build indexes.");
        }
        else if (toScene < 0)
        {
            Debug.Log("Invalid Call, Trying to access unexisting build indexes.");
        }
        else if (midLoad == false)
        {
            midLoad = true;
            goingTo = toScene;

            if (sceneAnim == true)
            {
                if (EVSceneChange != null)
                {
                    EVSceneChange();
                }
            }
            else
            {
                FinishedLoad();
            }
        }
        else
        {
            Debug.Log("Invalid Call, Loading in Progress");
        }
    }

    public void FinishedLoad()
    {
        // Change to accomodate any modifications in loadorder.
        // Should match index of "Start menu"
        if (CurrentIndex == 0)
        {
            GameObject.Find("StartMenu_Button").SetActive(false);
            GameObject.Find("StartMenu_InputField").SetActive(false);
        }
        if (goingTo == 0)
        {
            Destroy(GameObject.Find("OverlayCanvas"));
            Destroy(GameObject.Find("CharacterData"));
        }
        SceneManager.LoadScene(goingTo);
    }

    void LvlLoad(Scene scene, LoadSceneMode sceneMode)
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Raise");
        CurrentIndex = goingTo;
        midLoad = false;
    }
}
