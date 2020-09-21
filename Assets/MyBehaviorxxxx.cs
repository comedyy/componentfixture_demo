using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBehaviorxxxx : ComponentScript
{
    #pragma warning disable CS0649
    [ComponentFixtureField]
    private GameObject objxxx;
    [ComponentFixtureField]
    private Transform[] sdfsd;
    [ComponentFixtureField]
    private Camera cameraYYY;
    [ComponentFixtureField]
    private MyBehavior _other_script;
    [ComponentFixtureField]
    private MyBehavior[] _other_scripts;

    private Transform[] no_field1;
    public Transform[] no_field2;
    #pragma warning restore CS0649 

    // Start is called before the first frame update
    protected override  void OnFixtureLoad()
    {
        Debug.Log("output ----: MyBehaviorxxxx");
        Debug.LogFormat("obj:{0}", objxxx);       
        Debug.LogFormat("camera:{0}", cameraYYY);       
        Debug.LogFormat("_other_script:{0}", _other_script);
        Debug.LogFormat("_other_scripts:{0}", string.Join(",", _other_scripts.ToList()));
        Debug.LogFormat("transform:{0}", string.Join(",", sdfsd.ToList()));       
    }
}
