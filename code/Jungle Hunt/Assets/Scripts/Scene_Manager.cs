using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager : MonoBehaviour {

    // Accessible Fields:
    public int currentIndex;
    public bool scoreboardUp = false;
    public bool inStory = true;
    public bool inLoad = false;

    public delegate void bcSceneChange();
    public static event bcSceneChange EVSceneChange;

    private int goingTo;
    private bool midLoad = false;
    private bool sceneAnim = true;
    private bool endGame = false;
    private bool endApp = false;
    private int scoreBefore = 0;

    // Carried Data objects, use these for game's global functions.
    [SerializeField] private EventSystem input;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject menuInput;
    [SerializeField] private GameObject menuScores;
    [SerializeField] private GameObject menuQuit;
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject chardata;
    [SerializeField] private GameObject scores;
    [SerializeField] private GameObject scoresPlayer;
    [SerializeField] private GameObject scoresPoints;
    [SerializeField] private GameObject scoresTime;
    [SerializeField] private GameObject scoresLives;
    [SerializeField] private GameObject soundSystem;
    [SerializeField] private GameObject oxygenHud;
    [SerializeField] private GameObject oxygenBar;
    [SerializeField] private SceneChangeComponent_Image transition;

    // How many scenes to offset until levels start:
    private static int levelsStartFrom = 2;

    // Level generation order.
    private List<int> levelGenerationOrder = new List<int> ();


    void Start ()
    {
        // If a set of Carry objects exists, don't create another.
        if (GameObject.FindGameObjectsWithTag("Container").Length != 2)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.sceneLoaded += LvlLoad;

            scores.SetActive(false);
            oxygenHud.SetActive(false);
            DataContainer_Character.EVGameOver += RestartGame;
        }
    }

    // If the manager is replaced, remove onLoad from unity events.
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= LvlLoad;
    }

    private void Update()
    {
        // DEBUG ONLY
        if (Input.GetKeyDown("r") && currentIndex > 1)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(currentIndex);
        }
        else if (Input.GetKeyDown("t") && currentIndex > 1)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            NextLevel();
        }
        else if (Input.GetKeyDown("e") && currentIndex > 1)
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            ChangeScene(currentIndex - 1);
        }
        else if (Input.GetKeyDown("q") && currentIndex > 1)
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
        // Actual update.
        // Check if player wants to quit.
        if (Input.GetButtonDown("Quit"))
        {
            currentIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentIndex != 0)
            {
                ChangeScene(0);
            }
            else
            {
                Quit();
            }
            
        }
        // Relay that the player wishes to skip scoreboard sequence.
        else if (Input.GetButtonDown("Submit"))
        {
            transition.skip = true;
        }
    }

    // Starts Up UI when transferring to Stages.
    private void InitiateUI()
    {
        scores.GetComponent<UI_Script>().Activate(scoresPlayer, scoresPoints, scoresTime, scoresLives, chardata);
        scores.GetComponent<UI_Script>().TimerOn(100);
    }

    // Generate level order.
    // This can be used at the end of each 4 level loop to generate a new order.
    public void GenerateLevelOrder(bool random)
    {
        levelGenerationOrder.Clear();
        if (random == false)
        {
            inStory = true;
            levelGenerationOrder.Add(1);
            levelGenerationOrder.Add(2);
            levelGenerationOrder.Add(3);
            levelGenerationOrder.Add(4);
            levelGenerationOrder.Add(5);
            levelGenerationOrder.Add(6);
        }
        else
        {
            inStory = false;
            levelGenerationOrder.Add(Random.Range(1, 5));
            levelGenerationOrder.Add(Random.Range(1, 5));
            levelGenerationOrder.Add(Random.Range(1, 5));
            levelGenerationOrder.Add(Random.Range(1, 5));
            chardata.GetComponent<DataContainer_Character>().DifficultyUp();
        }
    }

    // Next level from order.
    // Call to advance to the next level.
    public void NextLevel(bool scoreboard = true)
    {
        scoreboardUp = scoreboard;
        scores.GetComponent<UI_Script>().TimerOff();
        // if no more levels, generate more...
        if (levelGenerationOrder.Count == 0)
        {
            GenerateLevelOrder(true);
        }
        int to = levelGenerationOrder[0] - 1;
        levelGenerationOrder.RemoveAt(0);
        soundSystem.GetComponent<Sound_System>().FadeOut();

        // offset of scenes at the start
        ChangeScene(levelsStartFrom + to);
        endGame = false;
    }

    // Quits the program via a transition.
    public void Quit()
    {
        this.transform.Find("Transition").GetComponent<Animator>().SetTrigger("Drop");
        soundSystem.GetComponent<Sound_System>().PlaySFX(1);
        endApp = true;
        soundSystem.GetComponent<Sound_System>().FadeOut();
        StartCoroutine(DramaticTiming());
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

            // Prevent Access if attempting to access new game with a name of "".
            if (goingTo == 1 && chardata.GetComponent<DataContainer_Character>().GetPlayerName() == "")
            {
                soundSystem.GetComponent<Sound_System>().PlaySFX(3);
                midLoad = false;
            }
            else
            {
                if (sceneAnim == true)
                {
                    soundSystem.GetComponent<Sound_System>().PlaySFX(1);
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
            
        }
        else
        {
            Debug.Log("Invalid Call, Loading in Progress");
        }
    }

    // Starts Load of given scene, used by transition component.
    public void StartLoad()
    {
        StartCoroutine(LoadASync());
        // IF Loading To New Game or Scoreboard.
        if (goingTo == 1 || goingTo == 7)
        {
            chardata.GetComponent<DataContainer_Character>().LoadResult();
            if (menuButton.activeSelf == true)
            {
                menuButton.SetActive(false);
                menuInput.SetActive(false);
                menuScores.SetActive(false);
                menuQuit.SetActive(false);
            }
        }
        // IF NOT Loading to LEVEL 2.
        if (goingTo != 3)
        {
            if (oxygenHud.activeSelf == true)
            {
                oxygenHud.SetActive(false);
            }
        }
        // IF Loading to a level.
        if (goingTo > 1 && goingTo < 6)
        {
            if (scores.activeSelf == false)
            {
                scores.SetActive(true);
            }
            soundSystem.GetComponent<Sound_System>().ChangeBG(goingTo - 1);
            InitiateUI();
            scoresPoints.GetComponent<Text>().text = "0 pts.";
        }
        else
        {
            if (goingTo == 7)
            {
                soundSystem.GetComponent<Sound_System>().ChangeBG(5);
            }
            else
            {
                soundSystem.GetComponent<Sound_System>().ChangeBG(0);
            }
            if (scores.activeInHierarchy == true)
            {
                scores.SetActive(false);
            }
            scores.GetComponent<UI_Script>().TimerOff();
        }
        oxygenBar.GetComponent<BarScript>().EndLevel2();
    }

    // A public function to call when finished loading, should probably be protected.
    public void FinishedLoad()
    {
        if (goingTo == 0)
        {
            menuButton.SetActive(true);
            menuInput.SetActive(true);
            menuScores.SetActive(true);
            menuQuit.SetActive(true);
            input.SetSelectedGameObject(menuInput);
        }
        else if (goingTo == 1)
        {
            input.SetSelectedGameObject(GameObject.Find("Slider").gameObject);
        }
        else if (goingTo == 7)
        {
            input.SetSelectedGameObject(GameObject.Find("Button").gameObject);
        }
        if (goingTo == 3)
        {
            oxygenHud.SetActive(true);
            oxygenBar.GetComponent<BarScript>().BeginLevel2();
        }
        if (goingTo == 6)
        {
            StartCoroutine("FinalSceneTimer");
        }
        else
        {
            chardata.GetComponent<DataContainer_Character>().scoresAwarder.Clear();
            chardata.GetComponent<DataContainer_Character>().actualScores.Clear();
        }

        midLoad = false;
        scoreboardUp = false;

    }

    // EVENT HANDLER: Called on Player Death.
    void RestartLevel()
    {
        scores.GetComponent<UI_Script>().TimerOff();
        scores.GetComponent<UI_Script>().ScoreDrop(scoreBefore);
        chardata.GetComponent<DataContainer_Character>().ChangeLives(-1);
        StartCoroutine("DramaticTiming");
    }

    // EVENT HANDLER: Called When Lives reach 0.
    void RestartGame()
    {
        endGame = true;
    }

    // A transition timer to keep scene on until transition has been given enough time.
    IEnumerator DramaticTiming()
    {
        yield return new WaitForSeconds(1.20f);
        if (endApp)
        {
            Application.Quit();
        }
        else if (endGame)
        {
            ChangeScene(7);
        }
        else
        {
            ChangeScene(currentIndex);
        }
    }

    // Coroutine to Load Asynchronously the level.
    IEnumerator LoadASync()
    {
        // The Application loads the Scene in the background at the same time as the current Scene.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(goingTo);
        inLoad = true;
        yield return new WaitForSeconds(1.00f);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        inLoad = false;
    }

    // Coroutine for final scene, creates a timer.
    IEnumerator FinalSceneTimer()
    {
        yield return new WaitForSeconds(6.00f);
        NextLevel(true);
    }

    // A event handler to give for unity, used once a level has loaded.
    void LvlLoad(Scene scene, LoadSceneMode sceneMode)
    {
        GameObject.Find("Transition").GetComponent<Animator>().SetTrigger("Raise");
        currentIndex = scene.buildIndex;
        if (goingTo > 1 && goingTo < 6)
        {
            Player.EVDeath += RestartLevel;
        }
        scoreBefore = chardata.GetComponent<DataContainer_Character>().GetPoints();
    }
}
