using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GérerTours : MonoBehaviour
{
    Text texte;
    void Start()
    {
        texte = GetComponent<Text>();
    }

    public void AugmenterTours(int tours, int toursMax)
    {
        if(tours <= toursMax)
            texte.text = $"{tours + 1}/{toursMax}";
    }
}
