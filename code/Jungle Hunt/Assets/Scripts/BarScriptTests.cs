using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tests
{
    public class BarScriptTests
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
        public IEnumerator Test_1_InitializeBarScript()
        {
            GameObject.Find("StartMenu_InputField").GetComponent<InputField>().text = "Test";

            GameObject.Find("StartMenu_Button").gameObject.GetComponent<Button>().onClick.Invoke();

            yield return new WaitForSeconds(2.50f);

            GameObject.Find("Button").gameObject.GetComponent<Button>().onClick.Invoke();

            yield return new WaitForSeconds(2.50f);

            Scene_Manager manager = GameObject.Find("OverlayCanvas").GetComponent<Scene_Manager>();

            manager.ChangeScene(3);

            yield return new WaitForSeconds(2.50f);

            Assert.NotNull(GameObject.Find("WaterBarSlider").gameObject);
        }

    }
}
