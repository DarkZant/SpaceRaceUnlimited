using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerationTerrain : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    public int xSize = 20;
    public int zSize = 20;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CalculerVertices();
        CalculerTriangles();
        UpdateMesh();
    }
    void CalculerVertices()
    {
        vertices = new Vector3[(xSize+1) * (zSize+1)];
        for(int z=0, i =0; z < zSize;z++)
        {
            for(int x=0; x < xSize; x++)
            {
                vertices[i]=(new Vector3(x, 0, z));
            }
        }
    }
    void CalculerTriangles()
    {
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int indiceTri = 0;
        for (int z = 0; z < zSize; z++)
        {
            for(int x = 0; x < xSize; x++ )
            {
                triangles[indiceTri] = (vert + 0);
                triangles[indiceTri] = (vert + xSize + 1);
                triangles[indiceTri] = (vert + 1);

                triangles[indiceTri] = (vert + 1);
                triangles[indiceTri] = (vert + xSize + 1);
                triangles[indiceTri] = (vert + xSize + 2);

                vert++;
                indiceTri += 6;
            }
        }
        
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }
}
