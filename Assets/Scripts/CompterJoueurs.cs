using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CompterJoueurs : MonoBehaviour
{
    const int nbJoueursMax = 8;
    Text texte;
    
    void Start()
    {
        texte = GetComponent<Text>();
    }
   
    void Update()
    {
        int nbJoueurs = PhotonNetwork.CurrentRoom.PlayerCount;
        texte.text = $"{nbJoueurs}" + " / " + $"{nbJoueursMax}";
    }
}
