using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentScript : IComponentFixture
{
    protected ComponentFixture3 _component;
    public void FixtureLoad(ComponentFixture3 comp)
    {
        _component = comp;
        OnFixtureLoad();
    }

    public void TriggerStart(){
        Start();
    }
    public void Enable()
    {
        OnEnable();
    }
    public void Disable()
    {
        OnDisable();
    }
    public void TriggerDestroy()
    {
        OnDestroy();
    }

    protected virtual void OnFixtureLoad(){}
    protected virtual void Start(){}
    protected virtual void OnEnable(){}
    protected virtual void OnDisable(){}
    protected virtual void OnDestroy(){}
}
