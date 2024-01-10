using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    [SerializeField] PlayerManager Manager;
    [SerializeField] Building Build;

    public void Destroy()
    {

        if (Manager.CurrentBuilding == null)
        {
            return;
        }

        var buildInfos = Manager.CurrentBuilding.name.Split(";");

        Build.BuildSave.RemoveBuild(new BuildData(Manager.CurrentBuilding.transform.localPosition, Int32.Parse(buildInfos[0]), Int32.Parse(buildInfos[1])));
        Destroy(Manager.CurrentBuilding);
        
    }

}
