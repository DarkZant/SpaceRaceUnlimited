using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int hauteur = 25;
    public int longueur = 512;
    public int largeur = 512;
    public float scale = 10f;
    DataSpline piste;
    public int pts = 0;
    Vector3[] vecteurs;
    Vector3[] lilVecteurs;
    private void Awake()
    {
        piste = new DataSpline("DataSplineX.txt", "DataSplineY.txt", "DataSplineZ.txt", pts);
    }

    private void Start()
    {
        CalculerSpline();
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
    void CalculerSpline()
    {
        vecteurs = piste.GetPointsSpline();
        lilVecteurs = new Vector3[vecteurs.Length];
        for (int i = 0; i < vecteurs.Length; i++)
        {
            lilVecteurs[i] = new Vector3(vecteurs[i].x / 1.5625f, vecteurs[i].y, vecteurs[i].z / 1.5625f);
        }
    }
    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = longueur + 1;
        terrainData.size = new Vector3(longueur, hauteur, largeur);

        terrainData.SetHeights(0, 0, GenerateHeights());

        return terrainData;
    }

    float[,] GenerateHeights()
    {

        float[,] heights = new float[longueur, largeur];
        for (int x = 0; x < longueur; x++)
        {
            for (int y = 0; y < largeur; y++)
            {
                heights[x, y] = CalculateHeights(x, y);
            }
        }
        return heights;
    }
    float CalculateHeights(int x, int y)
    {
        float xCoord = (float)x / longueur * scale;
        float yCoord = (float)y / largeur * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}
