using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    
    Vector3[] positionsSpawn = new Vector3[]
    {
        new Vector3(-8.67f,13.31f,-178.42f),
        new Vector3(-1.61f,13.31f,-186.42f),
        new Vector3(-8.67f,25,-178.42f),
        new Vector3(-1.61f,25,-186.42f),
        new Vector3(-17.63f,12,-182.41f),
        new Vector3(-12.87f,12,-192.1f),
        new Vector3(-17.63f,25,-182.41f),
        new Vector3(-12.87f,25,-192.1f),
    };
    Quaternion rotationSpawn = Quaternion.Euler(0,52,0);
    GameObject playerPrefab;
    public void SélectionnerPrefab(GameObject prefab) => playerPrefab = prefab;
    GameObject joueur;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void SpawnVaisseaux()
    {
        int idJoueur = PhotonNetwork.LocalPlayer.ActorNumber;
        Vector3 position = positionsSpawn[idJoueur - 1];
        joueur = PhotonNetwork.Instantiate(playerPrefab.name, position, rotationSpawn);
        joueur.GetComponent<VieVaisseau>().AssocierCompteur();
        joueur.GetComponent<PouvoirJoueur>().AssocierImagePouvoir();
        GameObject.Find("Fin").GetComponent<FinirCourse>().joueur = joueur;
    }
}
