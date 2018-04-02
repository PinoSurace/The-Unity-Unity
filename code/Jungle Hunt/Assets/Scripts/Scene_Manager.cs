using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour {

    public int CurrentIndex;
    public bool scoreboardUp = false;
    public bool inStory = true;

    public delegate void bcSceneChange();
    public static event bcSceneChange EVSceneChange;
    public int goingTo;
    public bool midLoad = false;
    public bool inLoad = false;
    private bool sceneAnim = true;
    private bool endgame = false;
    private int scoreBefore = 0;

    // Carried Data objects, use these for game's global functions.
    private GameObject sm_but;
    private GameObject sm_inp;
    private GameObject overlay;
    private GameObject chardata;
    private GameObject scores;
    private GameObject scores_player;
    private GameObject scores_points;
    private GameObject scores_time;
    private GameObject scores_lives;
    private GameObject sound_system;
    private GameObject oxygenhud;
    private GameObject oxygenbar;

    // How many scenes to offset until levels start:
    private static int levels_at = 2;

    // Level generation order.
    private List<int> levelgenerationorder = new List<int> ();

    // Use this for initialization
    void Start ()
    {
        if (GameObject.FindGameObjectsWithTag("Container").Length != 2)
        {
            Destroy(this.gameObject);
        }
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
        scores_lives = scores.transform.Find("Lives").gameObject;
        sound_system = overlay.transform.Find("SoundSystem").gameObject;
        oxygenhud = overlay.transform.Find("Level2Oxygen").gameObject;
        oxygenbar = oxygenhud.transform.Find("WaterBarSlider").gameObject;
        scores.SetActive(false);
        oxygenhud.SetActive(false);
        DataContainer_Character.EVGameOver += RestartGame;
    }

    // Generate level order.
    // This can be used at the end of each 4 level loop to generate a new order.
    public void GenerateLevelOrder(bool random)
    {
        levelgenerationorder.Clear();
        if (random == false)
        {
            inStory = true;
            levelgenerationorder.Add(1);
            levelgenerationorder.Add(2);
            levelgenerationorder.Add(3);
            levelgenerationorder.Add(4);
        }
        else
        {
            inStory = false;
            levelgenerationorder.Add(Random.Range(1, 5));
            levelgenerationorder.Add(Random.Range(1, 5));
            levelgenerationorder.Add(Random.Range(1, 5));
            levelgenerationorder.Add(Random.Range(1, 5));
        }
    }

    // If the manager is replaced, remove onLoad from unity events.
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
            NextLevel();
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

    private void InitiateUI()
    {
        scores.GetComponent<UI_Script>().Activate(scores_player, scores_points, scores_time, scores_lives, chardata);
        scores.GetComponent<UI_Script>().TimerOn(100);
    }

    // Next level from order.
    // Call to advance to the next level.
    public void NextLevel(bool scoreboard = true)
    {
        sound_system.GetComponent<Sound_System>().FadeOut();
        scoreboardUp = scoreboard;
        scores.GetComponent<UI_Script>().TimerOff();
        // if no more levels, generate more...
        if (levelgenerationorder.Count == 0)
        {
            GenerateLevelOrder(true);
        }
        int to = levelgenerationorder[0] - 1;
        levelgenerationorder.RemoveAt(0);

        // offset of scenes at the start
        ChangeScene(levels_at + to);
    }

    // Scene change event.
    // Call to change scenes when you wish to access non-stage scenes.
    // Also used internally for stage scenes.
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
                sound_system.GetComponent<Sound_System>().PlaySFX(1);
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

    public void StartLoad()
    {
        StartCoroutine(LoadASync());
        if (goingTo == 1)
        {
            if (sm_but.activeSelf == true)
            {
                sm_but.SetActive(false);
                sm_inp.SetActive(false);
            }
        }
        if (goingTo != 3)
        {
            if (oxygenhud.activeSelf == true)
            {
                oxygenhud.SetActive(false);
            }
        }
        if (goingTo > 1 && goingTo < 6)
        {
            if (scores.activeSelf == false)
            {
                scores.SetActive(true);
            }
            sound_system.GetComponent<Sound_System>().ChangeBG(goingTo - 1);
            InitiateUI();
            scores_points.GetComponent<Text>().text = "0 pts.";
        }
        else
        {
            if (scores.activeInHierarchy == true)
            {
                scores.SetActive(false);
            }
            scores.GetComponent<UI_Script>().TimerOff();
        }
        oxygenbar.GetComponent<BarScript>().EndLevel2();
    }

    // A public function to call when finished loading, should probably be protected.
    public void FinishedLoad()
    {
        if (goingTo == 0)
        {
            sm_but.SetActive(true);
            sm_inp.SetActive(true);
        }
        if (goingTo == 3)
        {
            oxygenhud.SetActive(true);
            oxygenbar.GetComponent<BarScript>().BeginLevel2();
        }
        if (goingTo == 6)
        {
            StartCoroutine("FinalSceneTimer");
        }
        else
        {
            chardata.GetComponent<DataContainer_Character>().scoresawarder.Clear();
            chardata.GetComponent<DataContainer_Character>().actualscores.Clear();
        }

        midLoad = false;
        scoreboardUp = false;

    }

    void RestartLevel()
    {
        scores.GetComponent<UI_Script>().TimerOff();
        scores.GetComponent<UI_Script>().ScoreDrop(scoreBefore);
        chardata.GetComponent<DataContainer_Character>().ChangeLives(-1);
        StartCoroutine("DramaticTiming");
    }

    void RestartGame()
    {
        endgame = true;
    }

    IEnumerator DramaticTiming()
    {
        yield return new WaitForSeconds(1.20f);
        if (endgame)
        {
            ChangeScene(0);
        }
        else
        {
            ChangeScene(CurrentIndex);
        }
    }

    IEnumerator LoadASync()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        // This is particularly good for creating loading screens. You could also load the Scene by build //number.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(goingTo);
        inLoad = true;
        yield return new WaitForSeconds(1.00f);
        //Wait until the last operation fully loads to return anything
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        inLoad = false;
    }

    IEnumerator FinalSceneTimer()
    {
        yield return new WaitForSeconds(6.00f);
        NextLevel(true);
    }

    // A event handler to give for unity, used once a level has loaded.
    void LvlLoad(Scene scene, LoadSceneMode sceneMode)
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Raise");
        CurrentIndex = scene.buildIndex;
        if (goingTo > 1)
        {
            Player.EVDeath += RestartLevel;
        }
        scoreBefore = chardata.GetComponent<DataContainer_Character>().GetPoints();
    }
}
