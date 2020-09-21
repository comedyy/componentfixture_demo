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

    protected abstract void OnFixtureLoad();
}
