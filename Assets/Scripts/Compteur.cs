using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Compteur : MonoBehaviour
{
    const float vitesseMax = 260f;

    Rigidbody CorpsVaisseau;
    float vitesse;
    [SerializeField]
    float vitesseflechemax;
    [SerializeField]
    float vitesseflechemin;
    [SerializeField]
    Text texteVitesse;
    [SerializeField]
    RectTransform fleche;

    public void AssocierVaisseau(GameObject vaisseau)
    {
        CorpsVaisseau = vaisseau.GetComponent<Rigidbody>();   
    }
    void Update()
    {
        Vector3 vélocité = new Vector3(Mathf.Abs(CorpsVaisseau.velocity.x), 0 , Mathf.Abs(CorpsVaisseau.velocity.z));
        vitesse = vélocité.magnitude * 3.6f * 2;
        texteVitesse.text = ((int)vitesse) + " km/h";
        fleche.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(vitesseflechemin, vitesseflechemax, vitesse / vitesseMax));
    }
}
