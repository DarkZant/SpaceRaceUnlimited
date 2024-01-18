using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointOrienté
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 tan;

    public PointOrienté(Vector3 pos,Vector3 tan, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
        this.tan = tan;
    }
    public PointOrienté(Vector3 pos, Vector3 tan)
    {
        this.pos = pos;
        this.rot = Quaternion.LookRotation(tan);
        this.tan = tan;
    }
    public Vector3 Normale(Vector3 positionNormales) => pos + rot * positionNormales;
    public Vector3 NormaleDirection(Vector3 positionNormales) => rot * positionNormales;

}
