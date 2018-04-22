using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerTests {

    

    [Test]
	public void ManageStateTests() {
        // Use the Assert class to test conditions.
        
        Player player = new Player();
       

        player.ManageState(Player.State.State_Idle);
        Assert.AreNotEqual(player.CurrentState, Player.State.State_Running);
        Assert.AreEqual(player.CurrentState, Player.State.State_Idle);



        player.ManageState(Player.State.State_Dead);
        Assert.AreEqual(player.CurrentState, Player.State.State_Dead);

        player.ManageState(Player.State.State_Running);
        Assert.AreEqual(player.CurrentState, Player.State.State_Dead);
        
        
    }

    /*

    [Test]
    public void DeadlyHazardTests()
    {
        Player player = new Player();
        

        player.ManageState(Player.State.State_Idle);
        player.DeadlyHazard();
        Assert.AreEqual(player.CurrentState, Player.State.State_Dead);

    }

    [Test]
    public void InTheBubbleTests()
    {
        
        player.ManageState(Player.State.State_Idle);
        player.InTheBubble();
        Assert.AreNotEqual(player.CurrentState, Player.State.State_Helpless);
        player.ManageState(Player.State.State_Swimming);
        Assert.AreEqual(player.CurrentState, Player.State.State_Helpless);
    }

    [Test]
    public void OutOfBubbleTests()
    {
        Player player = new Player();
        player.ManageState(Player.State.State_Idle);
        player.OutOfBubble();
        Assert.AreEqual(player.CurrentState, Player.State.State_Swimming);

    }*/
    
}
