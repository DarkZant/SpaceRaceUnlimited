using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Décompte : MonoBehaviour
{
    const int tempsdécompte = 3;
    const int tempsAttente = 2;
    Text texte;
    float temps;
    [SerializeField]
    GameObject vaisseau;
    GameObject chrono;
    void Start()
    {
        texte = GetComponent<Text>();
        chrono = GameObject.Find("Chrono");
    }
    public void AssocierVaisseau(GameObject joueur) => vaisseau = joueur;
    void Update()
    {
        temps += Time.deltaTime;
        if (temps >= tempsAttente)
        {
            if (temps >= tempsAttente + tempsdécompte + 1)
                gameObject.SetActive(false);
            else if (temps >= tempsAttente + tempsdécompte)
            {
                texte.text = "GO!";
                vaisseau.GetComponent<InputManager>().GérerContrôle(true);
                chrono.GetComponent<Chrono>().GérerChrono(true);
            }
            else
                texte.text = $"{Mathf.FloorToInt(-temps + tempsAttente + tempsdécompte + 1)}";
            
        }
    }
}
