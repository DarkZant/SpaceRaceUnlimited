using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class FinirCourse : MonoBehaviour
{
    const int nbToursMax = 3;
    int nbDeTours;
    [SerializeField]
    GameObject canvas;
    ModifierCanvas scriptCanvas;
    public GameObject joueur;
    [SerializeField]
    GameObject cam;
    bool pr�t�Quitter = false;
    int tempsDattente = 45;
    float temps = 0;
    [SerializeField]
    GameObject tours;
    G�rerTours scriptTours;
    void Start()
    {
        scriptCanvas = canvas.GetComponent<ModifierCanvas>();
        scriptTours = tours.GetComponent<G�rerTours>();
    }
    private void Update()
    {
        temps += Time.deltaTime;
        if(pr�t�Quitter)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(GameObject.Find("SpawnPlayers"));
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Loading");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(temps >= tempsDattente)
        {
            temps = 0;
            ++nbDeTours;
            scriptTours.AugmenterTours(nbDeTours ,nbToursMax);
            if (nbDeTours >= nbToursMax)
                TerminerCourse();
        }
    }
    void TerminerCourse()
    {
        scriptCanvas.Pr�parerFin();
        joueur.SetActive(false);
        cam.GetComponent<Cam�raD�placement>().regarde = false;
        cam.transform.position = new Vector3(0, 50, 0);
        cam.transform.rotation = Quaternion.Euler(270, 0, 0);
        pr�t�Quitter = true;
    }
}
