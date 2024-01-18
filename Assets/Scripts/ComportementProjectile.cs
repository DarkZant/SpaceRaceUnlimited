using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportementProjectile : MonoBehaviour
{
    [SerializeField]
    GameObject Mod�leExplosion;
    [SerializeField]
    float Dur�eDeVie = 3f;
    const float Dur�eExplosion = 0.2f;
    float temps�coul� = 0;

    void Update()
    {
        temps�coul� += Time.deltaTime;
        if (temps�coul� >= Dur�eDeVie)
            D�truireProj();
    }
    private void OnCollisionEnter(Collision collision)
    {
        D�truireProj();
    }
    private void D�truireProj()
    {
        GameObject explosion = Instantiate(Mod�leExplosion, gameObject.transform.position, Quaternion.identity);
        Destroy(explosion, Dur�eExplosion);
        Destroy(gameObject);
    }
}
