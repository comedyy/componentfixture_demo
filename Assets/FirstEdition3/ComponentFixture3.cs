using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public class ComponentFixture3 : MonoBehaviour
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
}
