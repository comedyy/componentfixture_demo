using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class ListEditor
{
    readonly ReorderableList _init_talk;
    bool _dirty = false;

    public ListEditor(SerializedObject serializedObject, string property_name, string title, int height = 0)
    {
        SerializedProperty init_talk_obj = serializedObject.FindProperty(property_name);
        _init_talk = new ReorderableList(serializedObject, init_talk_obj, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, title);
            },
            drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                EditorGUI.PropertyField(rect, init_talk_obj.GetArrayElementAtIndex(index), true);
            }
        };

        if (height > 0)
        {
            _init_talk.elementHeight = height;
        }
    }

    public ListEditor(IList list, System.Type type, string title, int height = 0)
    {
        _init_talk = new ReorderableList(list, type, true, true, true, true)
        {
            drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, title);
            },
            drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                Object pre = list[index] as Object;
                list[index] = EditorGUI.ObjectField(rect, (Object)list[index], type, true);
                if (pre != (Object)list[index])
                {
                    _dirty = true;
                }
            },
            onChangedCallback = (ReorderableList l) =>
            {
                _dirty = true;
            },
        };

        if (height > 0)
        {
            _init_talk.elementHeight = height;
        }
    }

    public bool DoLayoutList()
    {
        _init_talk.DoLayoutList();
        bool dirty = _dirty;
        _dirty = false;
        return dirty;
    }
}
