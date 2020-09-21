using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ComponentFixtureLoader
{
    internal static void Load(MyBehavior my, ComponentFixture componentFixture)
    {
        componentFixture.OnAfterDeserialize();

        Type type = my.GetType();
        foreach (var item in componentFixture.ListFiledInfo)
        {
            FieldInfo info = type.GetField(item.filed_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (info == null)
            {
                Debug.LogErrorFormat("FieldInfo not Exist:{0}", item.filed_name);
                continue;
            }

            if (info.FieldType.IsArray)
            {
                Array array = Array.CreateInstance(info.FieldType.GetElementType(), item.arr.Count);
                for (int i = 0; i < item.arr.Count; i++)
                    array.SetValue(item.arr[i], i);

                info.SetValue(my, array);
            }
            else
            {
                info.SetValue(my, item.obj);
            }
        }

        my.OnFixtureLoad();
    }

    internal static void Load3(MyBehavior my3, ComponentFixture3 componentFixture)
    {
        componentFixture.OnAfterDeserialize();

        Type type = my3.GetType();
        foreach (var item in componentFixture.ListFiledInfo)
        {
            FieldInfo info = type.GetField(item.filed_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (info == null)
            {
                Debug.LogErrorFormat("FieldInfo not Exist:{0}", item.filed_name);
                continue;
            }

            if (info.FieldType.IsArray)
            {
                Array array = Array.CreateInstance(info.FieldType.GetElementType(), item.arr.Count);
                for (int i = 0; i < item.arr.Count; i++)
                    array.SetValue(item.arr[i], i);

                info.SetValue(my3, array);
            }
            else
            {
                info.SetValue(my3, item.obj);
            }
        }

        my3.OnFixtureLoad();
    }
}
