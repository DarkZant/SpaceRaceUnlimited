using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VieVaisseau : MonoBehaviour
{
    [SerializeField]
    int ptVieMax = 100;
    int ptvie;
    [SerializeField]
    GameObject compteur;
    [SerializeField]
    GameObject ModèleExplosion;
    public Vector3 PointDeRéapparition = new Vector3(-8.67f, 13.31f, -178.42f);
    public Quaternion Rotation = Quaternion.Euler(0, 52, 0);
    [SerializeField]
    GameObject barreDeVie;
    BarreDeVie scriptBarreDeVie;
    Rigidbody CorpsVaisseau;
    float temps = 0;
    int tempsDégats = 1;
    void Awake()
    {
        CorpsVaisseau = GetComponent<Rigidbody>();
    }
    public void AssocierCompteur()
    {
        compteur = GameObject.Find("Compteur");
        compteur.GetComponent<Compteur>().AssocierVaisseau(gameObject);
    }
    private void Update()
    {
        temps += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (barreDeVie == null)
        {
            barreDeVie = GameObject.Find("HealthBar");
            scriptBarreDeVie = barreDeVie.GetComponent<BarreDeVie>();
            scriptBarreDeVie.ChangerVieMax(ptVieMax);
            ptvie = ptVieMax;
        }
        if(temps > tempsDégats)
        {
            temps = 0;
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile")
            {
                ptvie -= (int)(Mathf.Abs(CorpsVaisseau.velocity.x) + Mathf.Abs(CorpsVaisseau.velocity.z));
                Rigidbody rbobject = collision.gameObject.GetComponent<Rigidbody>();
                ptvie -= (int)(Mathf.Abs(rbobject.velocity.x) + Mathf.Abs(rbobject.velocity.z));
                scriptBarreDeVie.ChangerVie(ptvie);
            }
            else
            {
                CorpsVaisseau.angularVelocity = CorpsVaisseau.angularVelocity / 4;
                ptvie -= (int)(Mathf.Abs(CorpsVaisseau.velocity.x) + Mathf.Abs(CorpsVaisseau.velocity.z));
                scriptBarreDeVie.ChangerVie(ptvie);
            }
        }
        if (ptvie <= 0)
        {
            gameObject.SetActive(false);
            GameObject explosion = Instantiate(ModèleExplosion, gameObject.transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
            ptvie = ptVieMax;
            CorpsVaisseau.angularVelocity = Vector3.zero;
            CorpsVaisseau.velocity = Vector3.zero;
            Invoke("Respawn", 2.5f);
        }

    }
    private void Respawn()
    {
        scriptBarreDeVie.ChangerVie(ptVieMax);
        gameObject.transform.position = PointDeRéapparition;
        gameObject.transform.rotation = Rotation;
        gameObject.SetActive(true);
    }
}
