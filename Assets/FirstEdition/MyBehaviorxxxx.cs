using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBehaviorxxxx : IComponentFixture
{
    [ComponentFixtureField]
    private GameObject objxxx;
    [ComponentFixtureField]
    private Transform[] sdfsd;
    [ComponentFixtureField]
    private Camera cameraYYY;

    private Transform[] no_field1;
    public Transform[] no_field2;

    // Start is called before the first frame update
    public void OnFixtureLoad()
    {
        Debug.LogFormat("obj:{0}", objxxx);       
        Debug.LogFormat("camera:{0}", cameraYYY);       
        Debug.LogFormat("transform:{0}", string.Join(",", sdfsd.ToList()));       
    }
}
