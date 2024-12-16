using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LKP
{
    public Vector3 position;
    public LKP()
    {
        position = new Vector3();
    }
    public LKP(Vector3 position)
    {
        this.position = position;
    }
    public static implicit operator LKP(Vector3 position)
    {
        return new LKP(position);
    }
    public static implicit operator Vector3(LKP lkp)
    {
        return lkp.position;
    }
}
