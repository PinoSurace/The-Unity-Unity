using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water_Script : MonoBehaviour
{
    private Slider slider;
    private int regenrate = 2;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (slider == null)
        {
            slider = GameObject.Find("WaterBarSlider").GetComponent<Slider>();
        }
        //Debug.Log("collision name = " + other.gameObject.name);
        if (other.gameObject.name == "Tarzan")
        {
            if (slider.value + (regenrate / 2) > 100)
            {
                slider.value = 100;
            }
            else
            {
                slider.value += (regenrate / 2);
            }
        }

        if (other.gameObject.name == "bubble(Clone)")
        {
            Destroy(other.gameObject);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Tarzan")
        {
            if (slider.value + regenrate > 100)
            {
                slider.value = 100;
            }
            else
            {
                slider.value += regenrate;
            }
        }
    }
}
