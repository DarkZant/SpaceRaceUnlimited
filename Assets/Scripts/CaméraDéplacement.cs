using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaméraDéplacement : MonoBehaviour
{

    [SerializeField]
    Transform transformVaisseau;
    [SerializeField]
    Vector3 décalage = new Vector3(0, 6f, -16);
    [SerializeField]
    Space décalagePositionEspace = Space.Self;
    [SerializeField]
    public bool regarde = true;
    Camera cam;
    Rigidbody rbVaisseau;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    public void AffecterCaméraAuVaisseau(GameObject Vaisseau)
    {
        transformVaisseau = Vaisseau.transform;
        rbVaisseau = Vaisseau.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(regarde)
        {
            UpdatePosition();
            if(rbVaisseau != null)
                cam.fieldOfView = 60 + (Mathf.Abs(rbVaisseau.velocity.x) + Mathf.Abs(rbVaisseau.velocity.z)) / 2;
        }

    }
    public void UpdatePosition()
    {
        if (transformVaisseau == null)
            return;
        if (décalagePositionEspace == Space.Self)
            transform.position = transformVaisseau.TransformPoint(décalage);
        else
            transform.position = transformVaisseau.position + décalage;
        if (regarde)
            transform.LookAt(transformVaisseau);
        else
            transform.rotation = transformVaisseau.rotation;
    }
}
