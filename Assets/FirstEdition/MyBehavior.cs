﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MyBehavior : IComponentFixture
{
    [ComponentFixtureField]
    private GameObject obj;
    [ComponentFixtureField]
    private Camera camera;
    [ComponentFixtureField]
    private Transform[] transforms;
    [ComponentFixtureField]
    private Transform[] transforms1;
    [ComponentFixtureField]
    private MyBehaviorxxxx myxxx;

    private Transform[] no_field1;
    public Transform[] no_field2;

    // Start is called before the first frame update
    public void OnFixtureLoad()
    {
        Debug.Log("output ----: MyBehavior");
        Debug.LogFormat("obj:{0}", obj);       
        Debug.LogFormat("myxxx:{0}", myxxx);
        Debug.LogFormat("camera:{0}", camera);       
        Debug.LogFormat("transform:{0}", string.Join(",", transforms.ToList()));       
        Debug.LogFormat("transform:{0}", string.Join(",", transforms1.ToList()));       
    }
}
