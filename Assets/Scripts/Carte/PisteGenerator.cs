using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PisteGenerator : MonoBehaviour
{
    public GameObject prefab;
    DataSpline spline;
    Vector3[] points { get; set; }
    public int ptsIntermédiaire;

    private void Awake()
    {
        spline = new DataSpline("DataSplineX.txt", "DataSplineY.txt", "DataSplineZ.txt", ptsIntermédiaire);
        points = spline.GetPointsSpline();
        CréerPiste();
    }
    void CréerPiste()
    {
        for(int i = 0; i < points.Length; i++)
        {
            Instantiate(prefab, points[i], Quaternion.identity);
        }
    }
}
