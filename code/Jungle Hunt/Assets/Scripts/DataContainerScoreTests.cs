using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;


public class DataContainerScoreTests {
    // Ran to SetUp Test.
    [SetUp]
    public virtual void Init()
    {
        SceneManager.LoadScene("Scenes/Start menu");
        // To prevent Sounds from playing.
        UnityEngine.AudioListener.volume = 0;
    }

    [TearDown]
    public virtual void End()
    {
        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        container.SetPoints(0);
    }

    //Starting points should be 0.
    [UnityTest]
    public IEnumerator Test_1_Starting_Points()
    {
        SceneManager.LoadScene("Scenes/Start menu");

        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        Assert.AreEqual(container.GetPoints(), 0);

        yield return null;
    }


    [UnityTest]
    public IEnumerator Test_2_SetPoints()
    {
        int points = 100;
        SceneManager.LoadScene("Scenes/Start menu");

        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        container.SetPoints(points);

        Assert.AreEqual(container.GetPoints(), points);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_3_ChangePoints()
    {
        int points = 100;
        SceneManager.LoadScene("Scenes/Start menu");

        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        container.ChangePoints(points);
        container.ChangePoints(points);

        Assert.AreEqual(container.GetPoints(), 2 * points);
        yield return null;
    }

    //This test won't work if the AwardPoints score calculation changes
    [UnityTest]
    public IEnumerator Test_4_AwardPoints()
    {
        int index = 1;
        SceneManager.LoadScene("Scenes/Start menu");

        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        container.AwardPoints(index);
        Assert.AreEqual(container.GetPoints(),150);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Test_5_Everything()
    {
        int points = 100;
        int index = 1;
        SceneManager.LoadScene("Scenes/Start menu");

        DataContainer_Character container = GameObject.Find("CharacterData").GetComponent<DataContainer_Character>();
        container.ChangePoints(points);
        container.AwardPoints(index);
        container.SetPoints(points);
        Assert.AreEqual(container.GetPoints(), points);
        yield return null;
    }
}
