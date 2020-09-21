using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //MyBehavior my = new MyBehavior();
        //GameObject obj = Resources.Load<GameObject>("prefab");

        //ComponentFixtureLoader.Load(my, obj.GetComponent<ComponentFixture>());

        MyBehaviorxxxx my3 = new MyBehaviorxxxx();
        GameObject obj3 = Resources.Load<GameObject>("prefab3");

        ComponentFixtureLoader.Load3(my3, obj3.GetComponent<ComponentFixture3>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
