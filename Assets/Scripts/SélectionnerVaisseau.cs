using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SélectionnerVaisseau : MonoBehaviour
{
    [SerializeField]
    GameObject[] prefabs;
    [SerializeField]
    GameObject vaisseaux;
    [SerializeField]
    GameObject spawnPlayers;
    SpawnPlayers scriptSpawnPlayers;
    Sélectionneur sélectionneur;

    void Start()
    {
        sélectionneur = vaisseaux.GetComponent<Sélectionneur>();
        scriptSpawnPlayers = spawnPlayers.GetComponent<SpawnPlayers>();
    }

    public void ChoisirVaisseau()
    {
        int indexPrefabs = sélectionneur.index;
        scriptSpawnPlayers.SélectionnerPrefab(prefabs[indexPrefabs]);
        SceneManager.LoadScene("Game");      
    }

}  
