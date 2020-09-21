using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBehavior
{
    [ComponentFixtureField]
    private GameObject obj;
    [ComponentFixtureField]
    private Camera camera;
    [ComponentFixtureField]
    private Object[] transforms;

    private Transform[] no_field1;
    public Transform[] no_field2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogFormat("obj:{0}", obj);       
        Debug.LogFormat("camera:{0}", camera);       
        Debug.LogFormat("transform:{0}", string.Join(",", transforms.ToList()));       
    }
}
