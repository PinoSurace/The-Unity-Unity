using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Slider slider;
    public Player player;
    
    // Use this for initialization
    void Start()
    {
        slider.value = 100;
        StartCoroutine(HandleBar());
        
    }

    IEnumerator HandleBar()
    {
        while (player.CurrentState != Player.State.State_Inv)
        {
            slider.value -= 1;
            yield return new WaitForSecondsRealtime(0.14f);
            
            //no more air. dead 
            if (slider.value == 0)
            {                
                player.DeadlyHazard();
                break;
            }

        }
    }

    


}