using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class PisteGeneration : MonoBehaviour
{
    [SerializeField]
    GameObject checkpoint;
    [SerializeField]
    GameObject pouvoir;
    //[SerializeField]
    //float s = 7f;
    DataSpline piste;
    public int pts = 8;
    public int max;
    List<Vector3> verts = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triIndices = new List<int>();
    Vector3[] PointsPiste { get; set; }
    PointOrient�[] listePointsOrient�s { get; set; }
    List<PointOrient�> listePointsDeControle { get; set; }
    List<PointOrient�> listePosPouvoirs { get; set; }


    MeshCollider meshCollider;
    Mesh mesh;
    [SerializeField]Mesh2D Shape2D;

    //[Range(0,399)]
    //[SerializeField] int t = 0;
    private void Update()
    {
        UpdateMesh();
    }
    private void Awake()
    {
        piste = new DataSpline("DataSplineX.txt", "DataSplineY.txt", "DataSplineZ.txt", pts);
        CalculerPointsPiste();
        CalculerPointOrient�();
        mesh = new Mesh();
        meshCollider = new MeshCollider();
        GetComponent<MeshFilter>().sharedMesh = mesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();
        GenerateMesh();
        GetPointsDeControle();
        GetPositionPouvoirs();
        foreach(PointOrient� pt in listePointsDeControle)
        {
             GameObject cp = Instantiate(checkpoint, pt.pos, Quaternion.Euler(0, pt.rot.eulerAngles.y, pt.rot.eulerAngles.z));
             cp.transform.Rotate(0, 90, 0);
        }
        foreach (PointOrient� pt in listePosPouvoirs)
        {
            GameObject cp = Instantiate(pouvoir, pt.pos + Vector3.up * 6, pt.rot);
        }
    }
    void GenerateMesh()
    {
        for (int t = 0; t < listePointsOrient�s.Length; t++) 
        {
            PointOrient� op = GetPO(t);
            for (int i = 0; i < Shape2D.vertices.Length; i++)
            {
                float v = t / listePointsOrient�s.Length;
                normals.Add(op.NormaleDirection(Shape2D.vertices[i].normal));
                verts.Add(op.Normale(Shape2D.vertices[i].point));
                uvs.Add(new Vector2(Shape2D.vertices[i].u,t));
            }
        }
        for(int t = 0; t < listePointsOrient�s.Length; t++)
        {
            int tIndex = t * Shape2D.vertices.Length;
            int tIndexNext = (t+1) * Shape2D.vertices.Length;
            for (int line = 0; line < Shape2D.lineIndices.Length; line+=2)
            {
                if (t == listePointsOrient�s.Length - 1)
                {
                    int lineIndexA = Shape2D.lineIndices[line];
                    int lineIndexB = Shape2D.lineIndices[line + 1];

                    int A = tIndex + lineIndexA;
                    int B = tIndex + lineIndexB;
                    int C = (0 * Shape2D.vertices.Length) + lineIndexA;
                    int D = (1 * Shape2D.vertices.Length) + lineIndexB;

                    triIndices.Add(A);
                    triIndices.Add(C);
                    triIndices.Add(B);

                    triIndices.Add(C);
                    triIndices.Add(D);
                    triIndices.Add(B);
                }
                else
                {
                    int lineIndexA = Shape2D.lineIndices[line];
                    int lineIndexB = Shape2D.lineIndices[line + 1];

                    int indexA = tIndex + lineIndexA;
                    int indexB = tIndex + lineIndexB;
                    int indexC = tIndexNext + lineIndexA;
                    int indexD = tIndexNext + lineIndexB;

                    triIndices.Add(indexA);  
                    triIndices.Add(indexC); 
                    triIndices.Add(indexB);

                    triIndices.Add(indexC);
                    triIndices.Add(indexD);
                    triIndices.Add(indexB);
                }
            }
        }
    }
    void UpdateMesh()
    {
        mesh.Clear();
        mesh.name = "Piste";
        mesh.vertices = verts.ToArray();
        mesh.triangles = triIndices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        meshCollider.sharedMesh = mesh;
        
    }
    void CalculerPointsPiste()
    {
        PointsPiste = piste.GetPointsSpline();
        for (int i = 0; i < PointsPiste.Length; i++)
        {
            PointsPiste[i] = new Vector3(PointsPiste[i].x / 1.5625f, PointsPiste[i].y / 1.5625f, PointsPiste[i].z / 1.5625f);
        }
    }
    private Vector3 CalculerTangeantPoints(int i)
    {
        if (i == PointsPiste.Length - 1)
            return PointsPiste[0] - PointsPiste[i];
        else
            return PointsPiste[i + 1] - PointsPiste[i];
    }
    void CalculerPointOrient�()
    {
        listePointsOrient�s = new PointOrient�[PointsPiste.Length];
        for (int i = 0; i < listePointsOrient�s.Length; i++)
        {
            listePointsOrient�s[i] = new PointOrient�(PointsPiste[i], CalculerTangeantPoints(i));
        }
    }
    PointOrient� GetPO(int i) => listePointsOrient�s[i];
    public void GetPointsDeControle()
    {
        listePointsDeControle = new List<PointOrient�>();
        int t = listePointsOrient�s.Length / 8;
        for (int i = 0; i < t*8; i+=t)
        {
            listePointsDeControle.Add(listePointsOrient�s[i]);          
        }
    }
    public void GetPositionPouvoirs()
    {
        int quantit�pouvoirs = 6;
        listePosPouvoirs = new List<PointOrient�>();
        int t = listePointsOrient�s.Length / quantit�pouvoirs;
        for (int i = t; i < t * quantit�pouvoirs; i += t)
        {
            listePosPouvoirs.Add(listePointsOrient�s[i]);
        }
    }
}
