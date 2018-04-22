using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class BarUnitTests
    {
        // Ran to SetUp Test.
        [SetUp]
        public virtual void Init()
        {
            SceneManager.LoadScene("Scenes/Start menu");
            //Scene_Manager manager = GameObject.Find("WaterBarSlider").GetComponent<BarScript>();
            BarScript bar = GameObject.Find("WaterBarSlider").GetComponent<BarScript>();

            //manager.ChangeScene(7); // Nothing Should Happen
            // To prevent Sounds from playing.
            UnityEngine.AudioListener.volume = 0;
        }

        [TearDown]
        public virtual void End()
        {

        }

        [UnityTest]
        public IEnumerator Test_1_BarScript_Created_On_StartMenu()
        {
            Assert.AreNotEqual(GameObject.Find("WaterBarSlider"), null);          

            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_2_Only_One_Copy_Created_Ever()
        {
            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();

            manager.ChangeScene(2);
            Assert.AreNotEqual(GameObject.Find("WaterBarSlider"), null);



            yield return null;
        }

        [UnityTest]
        public IEnumerator Test_3_Begin_and_End_different()
        {
            BarScript bar = GameObject.Find("WaterBarSlider").GetComponent<BarScript>();
            bar.EndLevel2();

            //Assert.AreNotEqual(, null);

            yield return null;
            /*
            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();

            manager.ChangeScene(-2); // Nothing Should Happen
            yield return null;

            manager.ChangeScene(200); // Nothing Should Happen
            yield return null;

            Assert.AreEqual(SceneManager.GetActiveScene().name, "Start menu");

            manager.ChangeScene(7); // Should Change Scene...
            yield return new WaitForSeconds(2);

            Assert.AreNotEqual(SceneManager.GetActiveScene().name, "Start menu");*/

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


