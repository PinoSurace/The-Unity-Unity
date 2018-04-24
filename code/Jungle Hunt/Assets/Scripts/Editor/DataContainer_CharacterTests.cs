using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class DataContainer_CharacterTests {

    [Test]
    public void PlayerNameTests()
    {
        // Use the Assert class to test conditions.

        //Player player = new Player();

        DataContainer_Character player_data = new DataContainer_Character();

        player_data.SetPlayerName("Test");

        Assert.AreEqual(player_data.GetPlayerName(), "Test");

        player_data.SetPlayerName("Test2");

        Assert.AreNotEqual(player_data.GetPlayerName(), "Test");

        Assert.AreEqual(player_data.GetPlayerName(), "Test2");

    }

    [Test]
    public void PlayerLivesTests()
    {
        DataContainer_Character player_data = new DataContainer_Character();

        player_data.SetLives(3);
        Assert.AreEqual(player_data.GetNumOfLives(), 3);

        player_data.SetLives(-1);
        Assert.AreNotEqual(player_data.GetNumOfLives(), -1);
        Assert.AreEqual(player_data.GetNumOfLives(), 3);

        player_data.SetLives(0);
        Assert.AreEqual(player_data.GetNumOfLives(), 0);

        player_data.ChangeLives(3);
        Assert.AreEqual(player_data.GetNumOfLives(), 3);

        player_data.ChangeLives(-4);
        Assert.AreEqual(player_data.GetNumOfLives(), 0);

    }

    [Test]
    public void PlayerPointsTests()
    {
        DataContainer_Character player_data = new DataContainer_Character();

        player_data.SetPoints(10);
        Assert.AreEqual(player_data.GetPoints(), 10);

        player_data.ChangePoints(10);
        Assert.AreEqual(player_data.GetPoints(), 20);

    }


    [Test]
    public void DifficultyTests()
    {
        DataContainer_Character player_data = new DataContainer_Character();

        player_data.SetDifficulty(10);
        Assert.AreEqual(player_data.GetDifficulty(), 10);

        player_data.SetDifficulty(-10);
        Assert.AreNotEqual(player_data.GetDifficulty(), -10);
        Assert.AreEqual(player_data.GetDifficulty(), 10);

        player_data.DifficultyUp();
        Assert.AreEqual(player_data.GetDifficulty(), 25);


    }
}
