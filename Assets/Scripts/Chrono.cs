using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chrono : MonoBehaviour
{
    Text texte;
    float temps = 0;
    [SerializeField]
    bool chronoEnMarche = false;
    void Start()
    {
        texte = GetComponent<Text>();
    }

    void Update()
    {
        if(chronoEnMarche)
        {
            temps += Time.deltaTime;
            float secondes = (float)Mathf.Round(temps % 60);
            int minute = Mathf.FloorToInt(temps / 60);
            if (secondes < 10)
                texte.text = $"{minute}" + ":0" + $"{secondes}";
            else
                texte.text = $"{minute}" + ":" + $"{secondes}";
        }
    }
    public void GérerChrono(bool activé)
    {
        chronoEnMarche = activé;
        if(!activé)
        {
            float secondes = temps % 60;
            string secs = secondes.ToString("0.00");
            int minute = Mathf.FloorToInt(temps / 60);
            if (secondes < 10)
                texte.text = $"{minute}" + ":0" + secs;
            else
                texte.text = $"{minute}" + ":" + secs;
        }
    }   


    public void SauvegarderTemps()
    {
        for(int i = 0; i < 6; ++i)
        {
            if (!PlayerPrefs.HasKey($"record{i}")) 
            {
                PlayerPrefs.SetFloat($"record{i}", temps);
                break;
            }
        }
        PlayerPrefs.Save();
    }
}
