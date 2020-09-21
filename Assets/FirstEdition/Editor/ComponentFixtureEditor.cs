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

[CustomEditor(typeof(ComponentFixture), true)]
public class ComponentFixtureEditor : Editor
{
    private ComponentFixture _target_object;
    List<FiledData> _lst_info;
    Dictionary<string, ListEditor> _dic_list_editor = new Dictionary<string, ListEditor>();
    SerializedProperty p = null;

    // test_data
    List<EditorFiledInfo> lst_test = new List<EditorFiledInfo>()
    {
    };

    internal void OnEnable()
    {
        this._target_object = (ComponentFixture)this.target;
        this._target_object.OnAfterDeserialize();
        _lst_info = this._target_object.ListFiledInfo;
        p = serializedObject.FindProperty("_cshap_file_name");
        GetFieldList();
    }

    private void ApplyModifycation()
    {
        this._target_object.OnBeforeSerialize();
        EditorUtility.SetDirty(this._target_object);
    }

    void UpdateValidate(){
        List<EditorFiledInfo> lst_add = lst_test.Where(m => !_lst_info.Exists(n => n.filed_name == m.field_name)).ToList();
        _lst_info.RemoveAll(m => !lst_test.Exists(n => n.field_name == m.filed_name));
        _lst_info.AddRange(lst_add.Select(m => new FiledData() { filed_name = m.field_name}));
    }

    public override void OnInspectorGUI()
    {
        bool dirty = false;
        string pre_name = _target_object.ScriptFileName;
        _target_object.ScriptFileName = EditorGUILayout.DelayedTextField(_target_object.ScriptFileName);
        if (pre_name != _target_object.ScriptFileName)
        {
            if (GetFieldList())
            {
                dirty = true;
            }
            else
            {
                _target_object.ScriptFileName = pre_name;
            }
        }

        UpdateValidate();
        foreach (var item in lst_test)
        {
            FiledData data = _lst_info.Find(m => m.filed_name == item.field_name);

            if (!item.is_arry)
            {
                Object pre = data.obj;
                data.obj = EditorGUILayout.ObjectField(item.field_name, data.obj, item.type, true);
                if (pre != data.obj)
                {
                    dirty = true;
                }
            }
            else
            {
                data.arr = data.arr ?? new List<Object>();
                if (!_dic_list_editor.TryGetValue(item.field_name, out ListEditor list))
                {
                    list = new ListEditor(data.arr, item.type, item.field_name);
                    _dic_list_editor[item.field_name] = list;
                }

                if (list.DoLayoutList())
                {
                    dirty = true;
                }
            }
        }

        if (dirty)
        {
            ApplyModifycation();
        }
    }

    private bool GetFieldList()
    {
        Type t = null;
        if (!string.IsNullOrEmpty(_target_object.ScriptFileName))
        {
            t = Type.GetType(string.Format("{0},Assembly-CSharp", _target_object.ScriptFileName));
            if (t == null)
            {
                Debug.LogErrorFormat("not find type {0}", _target_object.ScriptFileName);
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
                    type = info.FieldType.IsArray ? info.FieldType.GetElementType() : info.FieldType
                };

                lst_test.Add(editor_field_info);
            }
        }

        return true;
    }
}
