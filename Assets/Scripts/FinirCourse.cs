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
    bool prêtÀQuitter = false;
    int tempsDattente = 45;
    float temps = 0;
    [SerializeField]
    GameObject tours;
    GérerTours scriptTours;
    void Start()
    {
        scriptCanvas = canvas.GetComponent<ModifierCanvas>();
        scriptTours = tours.GetComponent<GérerTours>();
    }
    private void Update()
    {
        temps += Time.deltaTime;
        if(prêtÀQuitter)
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
        scriptCanvas.PréparerFin();
        joueur.SetActive(false);
        cam.GetComponent<CaméraDéplacement>().regarde = false;
        cam.transform.position = new Vector3(0, 50, 0);
        cam.transform.rotation = Quaternion.Euler(270, 0, 0);
        prêtÀQuitter = true;
    }
}
