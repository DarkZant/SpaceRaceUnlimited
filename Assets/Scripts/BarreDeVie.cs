using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarreDeVie : MonoBehaviour
{
    [SerializeField]
    Slider slider;
   
    public void ChangerVie(int vie)
    {
        slider.value = vie;
    }
    public void ChangerVieMax(int vie)
    {
        slider.maxValue = vie;
        slider.value = vie;
    }
}
