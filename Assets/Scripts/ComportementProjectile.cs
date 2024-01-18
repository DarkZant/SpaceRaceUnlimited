using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementProjectile : MonoBehaviour
{
    [SerializeField]
    GameObject ModèleExplosion;
    [SerializeField]
    float DuréeDeVie = 3f;
    const float DuréeExplosion = 0.2f;
    float tempsÉcoulé = 0;

    void Update()
    {
        tempsÉcoulé += Time.deltaTime;
        if (tempsÉcoulé >= DuréeDeVie)
            DétruireProj();
    }
    private void OnCollisionEnter(Collision collision)
    {
        DétruireProj();
    }
    private void DétruireProj()
    {
        GameObject explosion = Instantiate(ModèleExplosion, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, DuréeExplosion);
        Destroy(gameObject);
    }
}
