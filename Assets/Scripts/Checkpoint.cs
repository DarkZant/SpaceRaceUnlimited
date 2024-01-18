using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    VieVaisseau scriptjoueur;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            scriptjoueur = other.gameObject.transform.parent.parent.gameObject.GetComponent<VieVaisseau>();
            scriptjoueur.PointDeRéapparition = transform.position + Vector3.up * 10;
            scriptjoueur.Rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - 90, transform.rotation.eulerAngles.z);

        }
    }
}
