using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    public Slider slider;  
    
    // Use this for initialization
    void Start()
    {
        slider.value = 100;
        StartCoroutine(HandleBar());
        
    }

    IEnumerator HandleBar()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            slider.value -= 1;
            
            //no more air. dead 
            if (slider.value == 0)
            {                
                Player player = GameObject.Find("Tarzan").GetComponent<Player>();
                player.DeadlyHazard();
                break;
            }

        }
    }

    


}