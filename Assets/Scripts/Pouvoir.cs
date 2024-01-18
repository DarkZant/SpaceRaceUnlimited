using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pouvoir : MonoBehaviour
{
    [SerializeField]
    GameObject joueur;
    [SerializeField]
     PouvoirJoueur scriptJoueur;
    [SerializeField]
    int valeurPouvoir;
    void Update()
    {
        transform.Rotate(new Vector3(0, 30, -30) * Time.deltaTime);
    }
    void OnTriggerEnter(Collider collision)
    {
        System.Random g�n�rateur = new System.Random();
        valeurPouvoir = g�n�rateur.Next(2);
        joueur = collision.gameObject.transform.parent.parent.gameObject;
        PhotonView view = joueur.GetComponent<PhotonView>();
        if(view.IsMine)
        {
            scriptJoueur = joueur.GetComponent<PouvoirJoueur>();
            scriptJoueur.AssignerPouvoir(valeurPouvoir);
        }
        Invoke("R�activer", 10);
        gameObject.SetActive(false);
    }
    void R�activer() => gameObject.SetActive(true);
}
