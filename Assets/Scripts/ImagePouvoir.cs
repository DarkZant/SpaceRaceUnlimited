using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePouvoir : MonoBehaviour
{
    [SerializeField]
    Texture speed;
    [SerializeField]
    Texture proj;
    RawImage image;
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    public void ChangerPouvoir(int pouvoir)
    {
        if (pouvoir == -1)
            image.color = new Color(255,255,255,0);
        else if (pouvoir == 0)
        {
            image.texture = speed;
            image.color = new Color(255, 255, 255, 1);
        }
        else if (pouvoir == 1)
        {
            image.texture = proj;
            image.color = new Color(255, 255, 255, 1);
        }
    }
}
