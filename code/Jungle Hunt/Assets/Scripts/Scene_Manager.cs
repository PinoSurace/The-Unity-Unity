using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour {

    public int CurrentIndex;
    public delegate void bcSceneChange();
    public static event bcSceneChange EVSceneChange;
    private int goingTo;
    private bool midLoad = false;
    private bool sceneAnim = true;

    private GameObject sm_but;
    private GameObject sm_inp;
    private GameObject overlay;
    private GameObject chardata;
    private GameObject scores;
    private GameObject scores_player;
    private GameObject scores_points;
    private GameObject scores_time;

    private List<int> levelgenerationorder = new List<int> ();

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(this);
        CurrentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += LvlLoad;

        sm_but = GameObject.Find("StartMenu_Button");
        sm_inp = GameObject.Find("StartMenu_InputField");
        overlay = GameObject.Find("OverlayCanvas");
        chardata = GameObject.Find("CharacterData");
        scores = overlay.transform.Find("ScoreBoard").gameObject;
        scores_player = scores.transform.Find("PlayerName").gameObject;
        scores_points = scores.transform.Find("PlayerScore").gameObject;
        scores_time = scores.transform.Find("TimerValue").gameObject;
        scores.SetActive(false);
    }

    public void GenerateLevelOrder(bool random)
    {
        levelgenerationorder.Clear();
        if (random == false)
        {
            levelgenerationorder.Add(1);
            levelgenerationorder.Add(2);
            levelgenerationorder.Add(3);
            levelgenerationorder.Add(4);
        }
        else
        {
            levelgenerationorder.Add(Random.Range(1, 4));
            levelgenerationorder.Add(Random.Range(1, 4));
            levelgenerationorder.Add(Random.Range(1, 4));
            levelgenerationorder.Add(Random.Range(1, 4));
        }
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

    public void NextLevel()
    {
        if (levelgenerationorder.Count == 0)
        {
            GenerateLevelOrder(true);
        }
        int to = levelgenerationorder[0];
        levelgenerationorder.RemoveAt(0);
        ChangeScene(to + 1);
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
            if (sm_but.activeSelf == true)
            {
                sm_but.SetActive(false);
                sm_inp.SetActive(false);
            }
        }
        if (goingTo == 0)
        {
            Destroy(overlay);
            Destroy(chardata);
        }
        if (goingTo > 1)
        {
            if (scores.activeSelf == false)
            {
                scores.SetActive(true);
            }
            string charname = chardata.GetComponent<DataContainer_Character>().GetPlayerName();
            scores_player.GetComponent<Text>().text = charname;
            scores_time.GetComponent<Text>().text = "5:00";
            scores_points.GetComponent<Text>().text = "0 pts.";
        }
        else
        {
            if (scores.activeInHierarchy == true)
            {
                scores.SetActive(false);
            }
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
