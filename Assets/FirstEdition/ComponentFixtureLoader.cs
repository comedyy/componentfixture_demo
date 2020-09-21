using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

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


    public static Type GetFieldType(FieldInfo info)
    {
        Type ret;
        if (info.FieldType.IsArray)
        {
            ret = info.FieldType.GetElementType();
        }
        else
        {
            ret = info.FieldType;
        }

        return ret;
    }

    static bool IsNull(object o)
    {
        if((o is Object)){
            return (o as Object) == null;
        }
    
        return o == null;
    }

    static object ConvertItem(object o, Type t)
    {
        if (IsNull(o))
        {
            return null;
        }

        Type type_obj = typeof(Object);
        if (t.IsSubclassOf(type_obj))
        {
            return o;
        }

        ComponentFixture3 com = o as ComponentFixture3;
        if (com.Script != null || com.IsLoading)
        {
            if (com.IsLoading)
            {
                Debug.LogWarning("ComponentFixture3 Loop Connect");
            }

            return com.Script;
        }
        else
        {
            object ot = Activator.CreateInstance(t);
            Load3(ot as IComponentFixture, com);
            return ot;
        }
    }

    internal static void Load3(IComponentFixture my3, ComponentFixture3 componentFixture)
    {
        componentFixture.OnAfterDeserialize();
        ComponentFixture3.PreLoad(my3, componentFixture);

        Type type = my3.GetType();
        foreach (var item in componentFixture.ListFiledInfo)
        {
            FieldInfo info = type.GetField(item.filed_name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (info == null)
            {
                Debug.LogErrorFormat("FieldInfo not Exist:{0}", item.filed_name);
                continue;
            }

            Type sub_type = GetFieldType(info);
            if (info.FieldType.IsArray)
            {
                Array array = Array.CreateInstance(sub_type, item.arr.Count);
                for (int i = 0; i < item.arr.Count; i++)
                    array.SetValue(ConvertItem(item.arr[i], sub_type), i);

                info.SetValue(my3, array);
            }
            else
            {
                object o = ConvertItem(item.obj, sub_type);
                if (IsNull(o))
                {
                    info.SetValue(my3, null);
                }
                else{
                    info.SetValue(my3, o);
                }
            }
        }

        ComponentFixture3.PostLoad(my3, componentFixture);
    }
}
