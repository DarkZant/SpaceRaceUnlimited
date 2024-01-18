using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointsOrientés 
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 tan;

    public PointsOrientés(Vector3 pos,Vector3 tan, Quaternion rot)
    {
        this.pos = pos;
        this.rot = rot;
        this.tan = tan;
    }
    public PointsOrientés(Vector3 pos, Vector3 tan)
    {
        this.pos = pos;
        this.rot = Quaternion.LookRotation(tan);
        this.tan = tan;
    }
    public Vector3 Normale(Vector3 poisitionNormales, float scale) => pos + (Vector3.Cross(tan, poisitionNormales).normalized*scale);
       
    
}
