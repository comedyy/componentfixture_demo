using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MyBehaviorxxxx my3 = new MyBehaviorxxxx();
        GameObject obj3 = GameObject.Instantiate(Resources.Load<GameObject>("prefab3"));
        ComponentFixtureLoader.Load3(my3, obj3.GetComponent<ComponentFixture3>());
    }
}
