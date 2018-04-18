using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class SceneManagerUnitTests
    {
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
            
        }

        [UnityTest]
        public IEnumerator Test_1_Scene_Manager_Created_On_StartMenu()
        {
            Assert.AreEqual(GameObject.FindGameObjectsWithTag("Container").Length, 2);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_2_Only_One_Copy_Created_Ever()
        {
            SceneManager.LoadScene("Scenes/Backstory");

            yield return null;

            SceneManager.LoadScene("Scenes/Start menu");
            Assert.AreEqual(GameObject.FindGameObjectsWithTag("Container").Length, 2);

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_3_ChangeScene_Method_Values()
        {
            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();
            
            manager.ChangeScene(-2); // Nothing Should Happen
            yield return null;

            manager.ChangeScene(200); // Nothing Should Happen
            yield return null;

            Assert.AreEqual(SceneManager.GetActiveScene().name, "Start menu");

            manager.ChangeScene(7); // Should Change Scene...
            yield return new WaitForSeconds(2);

            Assert.AreNotEqual(SceneManager.GetActiveScene().name, "Start menu");

        }

        [UnityTest]
        public IEnumerator Test_4_ChangeScene_Method_Times()
        {
            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();

            manager.ChangeScene(2); // Changes Scene
            yield return new WaitForSeconds(0.20f);

            manager.ChangeScene(3); // Call Too Fast
            yield return new WaitForSeconds(1.80f);

            Assert.AreEqual(SceneManager.GetActiveScene().name, "Level 1");

            manager.ChangeScene(7); // Should Change Scene...
            yield return new WaitForSeconds(2);

            Assert.AreNotEqual(SceneManager.GetActiveScene().name, "Start menu");

        }

    }
}

