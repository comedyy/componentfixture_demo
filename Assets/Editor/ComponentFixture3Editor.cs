using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;
using System.Reflection;


public struct EditorFiledInfo
{
    public string field_name;
    public Type type;
    public bool is_arry;
}


[CustomEditor(typeof(ComponentFixture3), true)]
public class ComponentFixture3Editor : Editor
{
    private ComponentFixture3 _target_object;
    List<FiledData> _lst_info;
    string _current_file_name;
    Dictionary<string, ListEditor> _dic_list_editor = new Dictionary<string, ListEditor>();
    SerializedProperty _script_name_property;

    // test_data
    List<EditorFiledInfo> lst_test = new List<EditorFiledInfo>();

    internal void OnEnable()
    {
        this._target_object = (ComponentFixture3)this.target;
        _lst_info = this._target_object.ListFiledInfo;
        _script_name_property = serializedObject.FindProperty("_cshap_file_name");
        GetFieldList(_script_name_property.stringValue);
    }

    void UpdateValidate(){
        List<EditorFiledInfo> lst_add = lst_test.Where(m => !_lst_info.Exists(n => n.filed_name == m.field_name)).ToList();
        _lst_info.RemoveAll(m => !lst_test.Exists(n => n.field_name == m.filed_name));
        _lst_info.AddRange(lst_add.Select(m => new FiledData() { filed_name = m.field_name}));
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _target_object.OnAfterDeserialize();

        bool dirty = false;
        string pre_name = _script_name_property.stringValue;
        EditorGUILayout.DelayedTextField(_script_name_property);
        if (pre_name != _script_name_property.stringValue || _current_file_name != _script_name_property.stringValue)
        {
            if (GetFieldList(_script_name_property.stringValue))
            {
                dirty = true;
            }
            else
            {
                _script_name_property.stringValue = pre_name;
            }

            _current_file_name = _script_name_property.stringValue;
        }

        UpdateValidate();

        EditorGUI.BeginChangeCheck();
        foreach (var item in lst_test)
        {
            FiledData data = _lst_info.Find(m => m.filed_name == item.field_name);

            if (!item.is_arry)
            {
                data.obj = EditorGUILayout.ObjectField(item.field_name, data.obj, item.type, true);
            }
            else
            {
                data.arr = data.arr ?? new List<Object>();
                if (!_dic_list_editor.TryGetValue(item.field_name, out ListEditor list))
                {
                    list = new ListEditor(data.arr, item.type, item.field_name);
                    _dic_list_editor[item.field_name] = list;
                }

                list.DoLayoutList();
            }
        }


        if (EditorGUI.EndChangeCheck() || dirty)
        {
            ApplyModifycation();
        }

    }

    Type GetFieldType(FieldInfo info)
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

        Type type_obj = typeof(Object);
        if (!ret.IsSubclassOf(type_obj))
        {
            ret = typeof(ComponentFixture3);
        }

        return ret;
    }

    private bool GetFieldList(string name)
    {
        Type t = null;
        if (!string.IsNullOrEmpty(name))
        {
            t = Type.GetType(string.Format("{0},Assembly-CSharp", name));
            if (t == null)
            {
                Debug.LogErrorFormat("not find type {0}", name);
                return false;
            }
        }

        lst_test.Clear();
        if (t == null)
        {
            return true;
        }

        FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo info = fields[i];
            if (info.GetCustomAttribute<ComponentFixtureFieldAttribute>() != null)
            {
                EditorFiledInfo editor_field_info = new EditorFiledInfo()
                {
                    field_name = info.Name,
                    is_arry = info.FieldType.IsArray,
                    type = GetFieldType(info)
                };

                lst_test.Add(editor_field_info);
            }
        }

        return true;
    }

    private void ApplyModifycation()
    {
        OnBeforeSerialize();
        serializedObject.ApplyModifiedProperties();
    }

    public void OnBeforeSerialize()
    {
        _lst_info.Sort((a, b) => string.Compare(a.filed_name, b.filed_name));
        List<string> lst_names = _lst_info.Select(m => m.filed_name).ToList();

        List<Object> list_value = new List<Object>();
        List<ArrayInfo> list_array_info = new List<ArrayInfo>();
        int value_index = 0;
        foreach (var item in _lst_info)
        {
            if (item.arr != null)
            {
                list_value.AddRange(item.arr);
                list_array_info.Add(new ArrayInfo() { Next = value_index, Count = item.arr.Count });
                value_index += item.arr.Count;
            }
            else
            {
                list_value.Add(item.obj);
                value_index++;
            }
        }

        SerializedProperty property_file_names = serializedObject.FindProperty("_field_names");
        property_file_names.arraySize = lst_names.Count;
        for (int i = 0; i < property_file_names.arraySize; i++)
        {
            property_file_names.GetArrayElementAtIndex(i).stringValue = lst_names[i];
        }

        SerializedProperty property_file_values = serializedObject.FindProperty("_field_values");
        property_file_values.arraySize = list_value.Count;
        for (int i = 0; i < property_file_values.arraySize; i++)
        {
            property_file_values.GetArrayElementAtIndex(i).objectReferenceValue = list_value[i];
        }

        SerializedProperty property_file_array = serializedObject.FindProperty("_field_arrays");
        property_file_array.arraySize = list_array_info.Count;
        for (int i = 0; i < property_file_array.arraySize; i++)
        {
            SerializedProperty pp = property_file_array.GetArrayElementAtIndex(i);
            pp.FindPropertyRelative("Next").intValue = list_array_info[i].Next;
            pp.FindPropertyRelative("Count").intValue = list_array_info[i].Count;
        }

        _dic_list_editor.Clear();
    }
}
