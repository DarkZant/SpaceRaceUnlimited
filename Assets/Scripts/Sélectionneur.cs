using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sélectionneur : MonoBehaviour
{
    Transform[] enfants;
    public int index = 0;
    void Start()
    {
        enfants = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        for(int i = 1; i < enfants.Length; ++i)
            enfants[i].Rotate(new Vector3(0, -15, 0) * Time.deltaTime);  
    }
    public void Prochain()
    {
        if (transform.position.x == -245)
        {
            transform.position = new Vector3(0, 0, 0);
            index = 0;
        }
        else
        {
            transform.position -= new Vector3(35, 0, 0);
            ++index;
        }
    }
    public void Précédent()
    {
        if (transform.position.x == 0)
        {
            transform.position = new Vector3(-245, 0, 0);
            index = 7;
        }
        else
        {
            transform.position += new Vector3(35, 0, 0);
            --index;
        }
    }

}
