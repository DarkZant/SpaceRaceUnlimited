using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifierCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject[] enfants;
    public void PréparerScène()
    {
        foreach (GameObject enfant in enfants)
        {
            if(enfant.name != "FinText" && enfant.name != "Quitter")
                enfant.SetActive(!enfant.activeSelf);        
        }
    }
    public void PréparerFin()
    {
        foreach (GameObject enfant in enfants)
        {
            if (enfant.name != "Chrono" && enfant.name != "FinText" && enfant.name != "Quitter")
                enfant.SetActive(false);
            else if (enfant.name == "FinText" || enfant.name == "Quitter")
                enfant.SetActive(true);
            else if (enfant.name == "Chrono")
            {
                enfant.GetComponent<RectTransform>().anchorMin = Vector2.one * 0.5f;
                enfant.GetComponent<RectTransform>().anchorMax = Vector2.one * 0.5f;
                enfant.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                enfant.GetComponent<Text>().fontSize = 50;
                enfant.GetComponent<Chrono>().GérerChrono(false);
                enfant.GetComponent<Chrono>().SauvegarderTemps();

            }

        }
    }
}
