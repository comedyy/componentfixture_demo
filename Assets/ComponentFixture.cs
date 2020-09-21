using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
struct ArrayInfo
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
    void OnFixtureLoad();
}

public class ComponentFixture : MonoBehaviour
{
    [SerializeField][HideInInspector]
    private string _cshap_file_name;
    [SerializeField][HideInInspector]
    private string[] _field_names;
    [SerializeField][HideInInspector]
    private Object[] _field_values;
    [SerializeField][HideInInspector]
    private ArrayInfo[] _field_arrays;

    public List<FiledData> ListFiledInfo { get; } = new List<FiledData>();

    public string ScriptFileName {
        get
        {
            return _cshap_file_name;
        }
        set
        {
            _cshap_file_name = value;
        }
    }

    #region serialize
    public void OnAfterDeserialize()
    {
        if (_field_names == null)
        {
            return;
        }
        ListFiledInfo.Clear();

        int value_index = 0;
        int array_index = 0;
        foreach (var item in _field_names)
        {
            FiledData info = new FiledData() { filed_name = item };
            ListFiledInfo.Add(info);

            if (_field_arrays.Length > array_index && _field_arrays[array_index].Next == value_index)
            {
                List<Object> arr = _field_values.ToList().GetRange(value_index, _field_arrays[array_index].Count).ToList();

                value_index += _field_arrays[array_index].Count;
                info.arr = arr;
                array_index++;
            }
            else
            {
                info.obj = _field_values[value_index];
                value_index++;
            }
        }
    }

    public void OnBeforeSerialize()
    {
        ListFiledInfo.Sort((a, b) => string.Compare(a.filed_name, b.filed_name));
        _field_names = ListFiledInfo.Select(m => m.filed_name).ToArray();

        List<Object> list_value = new List<Object>();
        List<ArrayInfo> list_array_info = new List<ArrayInfo>();
        int value_index = 0;
        foreach (var item in ListFiledInfo)
        {
            if (item.arr != null)
            {
                list_value.AddRange(item.arr);
                list_array_info.Add(new ArrayInfo() { Next = value_index, Count = item.arr.Count});
                value_index += item.arr.Count;
            }
            else
            {
                list_value.Add(item.obj);
                value_index++;
            }
        }

        _field_values = list_value.ToArray();
        _field_arrays = list_array_info.ToArray();
    }
    #endregion
}
