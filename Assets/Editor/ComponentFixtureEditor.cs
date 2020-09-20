using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Linq;

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

    // test_data
    readonly List<EditorFiledInfo> lst_test = new List<EditorFiledInfo>()
    {
        new EditorFiledInfo(){
            field_name = "aaa",
            is_arry = false,
            type = typeof(GameObject)
        },
        new EditorFiledInfo(){
            field_name = "bbb",
            is_arry = false,
            type = typeof(Camera)
        },
        new EditorFiledInfo(){
            field_name = "eee",
            is_arry = true,
            type = typeof(Camera)
        },
    };

    internal void OnEnable()
    {
        this._target_object = (ComponentFixture)this.target;
        this._target_object.OnAfterDeserialize();
        _lst_info = this._target_object.ListFiledInfo;
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
        UpdateValidate();
        foreach (var item in lst_test)
        {
            FiledData data = _lst_info.Find(m=>m.filed_name == item.field_name);

            if (!item.is_arry)
            {
                Object pre = data.obj;
                data.obj = EditorGUILayout.ObjectField(item.field_name, data.obj, item.type, true);
                if(pre != data.obj){
                    dirty = true;
                }
            }
            else
            {
                data.arr = data.arr ?? new Object[0];
                if (!_dic_list_editor.TryGetValue(item.field_name, out ListEditor list))
                {
                    list = new ListEditor(data.arr, item.type, item.field_name);
                    _dic_list_editor[item.field_name] = list;
                }

                if(list.DoLayoutList()){
                    dirty = true;
                }
            }
        }

        if (dirty)
        {
            ApplyModifycation();
        }
    }
}
