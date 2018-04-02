using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public GameObject bg;

    private bool running;
    private bool warning = false;
    Player player;
    
    // Use this for initialization
    void Start()
    {
        slider.value = 100;
    }

    public void BeginLevel2()
    {
        warning = false;
        bg.GetComponent<Image>().color = Color.white;
        fill.color = Color.blue;
        running = true;
        slider.value = 100;
        StartCoroutine(HandleBar());
    }

    public void EndLevel2()
    {
        slider.value = 100;
        running = false;
    }

    IEnumerator HandleBar()
    {
        
        while (player == null)
        {
            player = GameObject.Find("Tarzan").GetComponentInChildren<Player>();
            Debug.Log(player);
            yield return new WaitForSeconds(0.01f);
        }
        while (running)
        {
            if (player.CurrentState == Player.State.State_Inv)
            {
                running = false;
            }
            {
                slider.value -= 1;
                yield return new WaitForSecondsRealtime(0.14f);

                //no more air. dead 
                if (slider.value == 0)
                {
                    player.DeadlyHazard();
                    break;
                }
                // low air. Warning
                else if (slider.value < 35 && !warning)
                {
                    warning = true;
                    bg.GetComponent<Image>().color = Color.red;
                    fill.color = Color.red;
                }
                else if (slider.value < 35 && warning)
                {
                    warning = false;
                    bg.GetComponent<Image>().color = Color.white;
                    fill.color = new Color(1.00f, 0.25f, 0.35f, 0.80f);
                }
                else
                {
                    warning = false;
                    bg.GetComponent<Image>().color = Color.white;
                    fill.color = Color.blue;
                }
            }
        }
    }

    


}