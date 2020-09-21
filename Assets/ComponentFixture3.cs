using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public struct ArrayInfo
{
    public int Next;
    public int Count;
}

public class FiledData
{
    public string filed_name;
    public List<Object> arr;
    public Object obj;
}

public interface IComponentFixture
{
    void FixtureLoad(ComponentFixture3 comp);
    void TriggerStart();
    void Enable();
    void Disable();
    void TriggerDestroy();
}

public class ComponentFixture3 : MonoBehaviour
{
#region serialized_fields
    #pragma warning disable CS0649
    [SerializeField][HideInInspector]
    private string _cshap_file_name;
    [SerializeField][HideInInspector]
    private string[] _field_names;
    [SerializeField][HideInInspector]
    private Object[] _field_values;
    [SerializeField][HideInInspector]
    private ArrayInfo[] _field_arrays;
    #pragma warning restore CS0649
#endregion 

#region runtime
    public IComponentFixture Script { get; private set; }
    public bool IsLoading { get; private set; }
    public static void PostLoad(IComponentFixture script, ComponentFixture3 component_object)
    {
        script.FixtureLoad(component_object);
        if (component_object._need_enable_afterload)
        {
            script.Enable();
            component_object._need_enable_afterload = false;
        }
    }
    public static void PreLoad(IComponentFixture script, ComponentFixture3 component_object)
    {
        component_object.Script = script;
        component_object.IsLoading = true;
    }
#endregion

    #region serialize
    public List<FiledData> ListFiledInfo { get; } = new List<FiledData>();
    public void OnAfterDeserialize()
    {
        if (_field_names == null)
        {
            return;
        }

        int value_index = 0;
        int array_index = 0;
        foreach (var item in _field_names)
        {
            FiledData info = ListFiledInfo.Find(m=>m.filed_name == item);
            if (info == null)
            {
                info = new FiledData() { filed_name = item };
                ListFiledInfo.Add(info);
            }

            if (_field_arrays.Length > array_index && _field_arrays[array_index].Next == value_index)
            {
                if (info.arr == null)
                {
                    info.arr = new List<Object>();
                }
                else
                {
                    info.arr.Clear();
                }
                info.arr.AddRange(_field_values.ToList().GetRange(value_index, _field_arrays[array_index].Count));

                value_index += _field_arrays[array_index].Count;
                array_index++;
            }
            else
            {
                info.obj = _field_values[value_index];
                value_index++;
            }
        }

        ListFiledInfo.RemoveAll(m=>!_field_names.Contains(m.filed_name));
    }
    #endregion

    #region unity Event
    void Start(){
        Script.TriggerStart();
    }

    void Destroy(){
        Script.TriggerDestroy();
    }

    bool _need_enable_afterload;
    void OnEnable(){
        if (Script != null && !IsLoading)
        {
            Script.Enable();
        }
        else
        {
            _need_enable_afterload = true;
        }
    }

    void OnDisable(){
        Script.Disable();
    }

    #endregion
}
