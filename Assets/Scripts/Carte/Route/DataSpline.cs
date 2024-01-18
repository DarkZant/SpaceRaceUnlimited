using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using System;

public class DataSpline 
{
    private int NbPtsIntermédiaire;
    private int Nbligne { get; set; } = 0;
    private int nbT { get; set; }
    private double[] X { get; set; }
    private double[] Y { get; set; }
    private double[] Z { get; set; }
    public Vector3[] PtsTrajectoire { get; set; }
    private string[] TableauLignesX { get; set; }
    private string[] TableauLignesY { get; set; }
    private string[] TableauLignesZ { get; set; }
    private List<string[]> ListeCoefficientX { get; set; }
    private List<string[]> ListeCoefficientY { get; set; }
    private List<string[]> ListeCoefficientZ { get; set; }
    private List<double[]> ListeCoefficientXDouble { get; set; }
    private List<double[]> ListeCoefficientYDouble { get; set; }
    private List<double[]> ListeCoefficientZDouble { get; set; }


    public DataSpline(string dataX, string dataY, string dataZ, int pts)
    {
        NbPtsIntermédiaire = pts;
        string DataX = $"Assets/Resources/Data/{dataX}";
        string DataY = $"Assets/Resources/Data/{dataY}";
        string DataZ = $"Assets/Resources/Data/{dataZ}";
        TableauLignesX = LireLesDonnées(DataX);
        TableauLignesY = LireLesDonnées(DataY);
        TableauLignesZ = LireLesDonnées(DataZ);
        CompterLesÉquations(DataZ);
    }
    string[] LireLesDonnées(string data)
    {
        string[] tableau;
        using (var lecteur = new StreamReader(data))
        {
            tableau = lecteur.ReadToEnd().Split('\n');
        }
        return tableau;
    }
    void CompterLesÉquations(string data)
    {
        using (var lecteur = new StreamReader(data))
        {
            string ligne = lecteur.ReadLine();
            while (ligne != null)
            {
                ++Nbligne;
                ligne = lecteur.ReadLine();
            }
            nbT = Nbligne * NbPtsIntermédiaire;
        }
    }
    List<string[]> SplitterListe(string[] tableau)
    {
        List<string[]> listeCoefficient = new List<string[]>(Nbligne);
        for (int i = 0; i < listeCoefficient.Capacity; i++)
        {
            listeCoefficient.Add(tableau[i].Split('\t'));
        }
        return listeCoefficient;
    }
    void SplitterLesListe()
    {
        ListeCoefficientX = SplitterListe(TableauLignesX);
        ListeCoefficientY = SplitterListe(TableauLignesY);
        ListeCoefficientZ = SplitterListe(TableauLignesZ);
    }
    void ConvertirLesListes()
    {
        ListeCoefficientXDouble = new List<double[]>(ListeCoefficientX.Capacity);
        for (int i = 0; i < ListeCoefficientXDouble.Capacity; i++)
        {
            ListeCoefficientXDouble.Add(new double[ListeCoefficientX[i].Length]);
            for (int j = 0; j < ListeCoefficientXDouble[i].Length; j++)
            {
                ListeCoefficientXDouble[i][j] = double.Parse(ListeCoefficientX[i][j], CultureInfo.InvariantCulture);
            }
        }
        ListeCoefficientYDouble = new List<double[]>(ListeCoefficientY.Capacity);
        for (int i = 0; i < ListeCoefficientYDouble.Capacity; i++)
        {
            ListeCoefficientYDouble.Add(new double[ListeCoefficientY[i].Length]);
            for (int j = 0; j < ListeCoefficientYDouble[i].Length; j++)
            {
                ListeCoefficientYDouble[i][j] = double.Parse(ListeCoefficientY[i][j], CultureInfo.InvariantCulture);
            }
        }
        ListeCoefficientZDouble = new List<double[]>(ListeCoefficientZ.Capacity);
        for (int i = 0; i < ListeCoefficientZDouble.Capacity; i++)
        {
            ListeCoefficientZDouble.Add(new double[ListeCoefficientZ[i].Length]);
            for (int j = 0; j < ListeCoefficientXDouble[i].Length; j++)
            {
                ListeCoefficientZDouble[i][j] = double.Parse(ListeCoefficientZ[i][j], CultureInfo.InvariantCulture);
            }
        }
    }
    void TrouverPtsX()
    {
        X = new double[nbT];
        int ligne = 0;
        for(int index = 0; index < X.Length;)
        {
            for (double t = 0; t < Nbligne; t += (double)1 / NbPtsIntermédiaire)
            {
                if (t < ligne + 1)
                {
                    for (int c = 0; c < ListeCoefficientX[ligne].Length; c++)
                        X[index] += (ListeCoefficientXDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
                else
                {
                    ligne++;
                    for (int c = 0; c < ListeCoefficientX[ligne].Length; c++)
                        X[index] += (ListeCoefficientXDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
            }
        }
        
    }
    void TrouverPtsY()
    {
        Y = new double[nbT];
        
        int ligne = 0;
        for (int index = 0; index < Y.Length;)
        {
            for (double t = 0; t < Nbligne; t += (double)1 / NbPtsIntermédiaire)
            {
                if (t < ligne + 1)
                {
                    for (int c = 0; c < ListeCoefficientY[ligne].Length; c++)
                        Y[index] += (ListeCoefficientYDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
                else
                {
                    ligne++;
                    for (int c = 0; c < ListeCoefficientY[ligne].Length; c++)
                        Y[index] += (ListeCoefficientYDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
            }
        }
    }
    void TrouverPtsZ()
    {
        Z = new double[nbT];
        
        int ligne = 0;
        for (int index = 0; index < Z.Length;)
        {
            for (double t = 0; t < Nbligne; t += (double)1 / NbPtsIntermédiaire)
            {
                if (t < ligne + 1)
                {
                    for (int c = 0; c < ListeCoefficientZ[ligne].Length; c++)
                        Z[index] += (ListeCoefficientZDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
                else
                {
                    ligne++;
                    for (int c = 0; c < ListeCoefficientZ[ligne].Length; c++)
                        Z[index] += (ListeCoefficientZDouble[ligne][c] * Math.Pow(t, c));
                    index++;
                }
            }
        }
    }
    public Vector3[] GetPointsSpline()
    {
        SplitterLesListe();
        ConvertirLesListes();
        TrouverPtsX();
        TrouverPtsY();
        TrouverPtsZ();

        PtsTrajectoire = new Vector3[nbT];
        for (int index = 0; index < PtsTrajectoire.Length; index++)
            PtsTrajectoire[index] = new Vector3((float)X[index], (float)Y[index], (float)Z[index]);
        return PtsTrajectoire;
    }
}