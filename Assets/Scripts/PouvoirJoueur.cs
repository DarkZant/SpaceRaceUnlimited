using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class PouvoirJoueur : MonoBehaviour
{
    float ForceImpulsionProjectile = 60;
    Transform canon;
    int pouvoir = -1;
    MouvementVaisseau scriptMouv;
    ImagePouvoir imagepouvoir;
    void Start()
    {
        Transform[] mesTransforms = GetComponentsInChildren<Transform>();
        canon = mesTransforms.First(X => X.gameObject.name == "Canon");
        scriptMouv = GetComponent<MouvementVaisseau>();      
    }

    public void AssocierImagePouvoir()
    {
        imagepouvoir = GameObject.Find("ImagePouvoir").GetComponent<ImagePouvoir>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pouvoir == 0)
            {
                scriptMouv.DoublerVitesse(true);
                pouvoir = -1;
                imagepouvoir.ChangerPouvoir(pouvoir);
                Invoke("RemettreVitesse", 5);
            }
            else if(pouvoir == 1)
            {
                pouvoir = -1;
                imagepouvoir.ChangerPouvoir(pouvoir);
                TirerProjectile();
            }
        }
    }
    public void AssignerPouvoir(int pouv)
    {
        if (pouvoir == -1)
        {
            pouvoir = pouv;
            imagepouvoir.ChangerPouvoir(pouvoir);
        }
    }
    void RemettreVitesse() => scriptMouv.DoublerVitesse(false);

    void TirerProjectile()
    {
        GameObject nouvProjectile = PhotonNetwork.Instantiate("Projectile", canon.position, canon.rotation);
        nouvProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * ForceImpulsionProjectile, ForceMode.Impulse);
    }
}
