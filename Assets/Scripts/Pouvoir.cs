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
        System.Random générateur = new System.Random();
        valeurPouvoir = générateur.Next(2);
        joueur = collision.gameObject.transform.parent.parent.gameObject;
        PhotonView view = joueur.GetComponent<PhotonView>();
        if(view.IsMine)
        {
            scriptJoueur = joueur.GetComponent<PouvoirJoueur>();
            scriptJoueur.AssignerPouvoir(valeurPouvoir);
        }
        Invoke("Réactiver", 10);
        gameObject.SetActive(false);
    }
    void Réactiver() => gameObject.SetActive(true);
}
