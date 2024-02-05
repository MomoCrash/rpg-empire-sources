using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MobStats : MonoBehaviour
{

    public void SetValues(MobData mobData)
    {
        gameObject.transform.position = new Vector3(mobData.Position[0], mobData.Position[1], mobData.Position[2]);
    }

}

[System.Serializable]
public class MobData
{

    public float[] Position = new float[3];

    public MobData(MobStats mob)
    {

        this.Position[0] = mob.gameObject.transform.position.x;
        this.Position[1] = mob.gameObject.transform.position.y;
        this.Position[2] = mob.gameObject.transform.position.z;

    }

}
