using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GérerRecords : MonoBehaviour
{
    string texteRecords;
    Text texte;
    void Awake()
    {
        texte = GetComponent<Text>();
        AfficherRecords();

    }
    public void RéinitialiserRecords()
    {
        PlayerPrefs.DeleteAll();
        texteRecords = "";
        AfficherRecords();
    }

    void AfficherRecords()
    {
        List<float> records = new List<float>();
        for (int i = 0; i < 6; ++i)
        {
            if (PlayerPrefs.HasKey($"record{i}"))
                records.Add(PlayerPrefs.GetFloat($"record{i}"));
            else
                records.Add(300);
        }
        records.Sort();
        for (int i = 0; i < 6; ++i)
        {
            if (PlayerPrefs.GetFloat($"record{i}") == records[records.Count - 1])
                PlayerPrefs.DeleteKey($"record{i}");
        }
        records.RemoveAt(records.Count - 1);
        foreach (float temps in records)
        {
            string tps;
            float secondes = temps % 60;
            string secs = secondes.ToString("0.00");
            int minute = Mathf.FloorToInt(temps / 60);
            if (secondes < 10)
                tps = $"{minute}" + ":0" + secs;
            else
                tps = $"{minute}" + ":" + secs;
            texteRecords += tps + "\n";
        }
        texte.text = texteRecords;
    }



}
