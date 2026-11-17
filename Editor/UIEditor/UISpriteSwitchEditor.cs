using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UISpriteSwitch))]
public class UISpriteSwitchEditor : Editor 
{
    private UISpriteSwitch _Target;
    private void OnEnable()
    {
        _Target = target as UISpriteSwitch;
    }
    public override void OnInspectorGUI()
    {
        DrawTopBtn();
        DrawArrSpriteItem();

    }
    private void DrawTopBtn()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Add", GUILayout.Width(100)))
            {
                _Target.AddSprite();
                EditorUtility.SetDirty(_Target);
            }
        }
        EditorGUILayout.EndHorizontal();
    }


    private void DrawArrSpriteItem()
    {
        for (int i = 0; i < _Target.GetSwitchCount(); i++)
        {
            DrawSpriteItem(i);
        }
    }
    private void DrawSpriteItem(int index)
    {
        var item = _Target.GetArrSprite(index);

        EditorGUILayout.BeginHorizontal();
        {
            if(GUILayout.Button($"{index}", GUILayout.Width(20)))
            {
                _Target.Switch(index);
                // SceneView.RepaintAll();
                // Canvas.ForceUpdateCanvases();
                // AssetDatabase.Refresh();
                EditorUtility.SetDirty(_Target);
            }
            var sprite = EditorGUILayout.ObjectField(item.GetSprite(), typeof(Sprite), false, GUILayout.Width(150)) as Sprite;
            item.SetSprite(sprite);
            var color = EditorGUILayout.ColorField(item.GetColor(), GUILayout.Width(50));
            item.SetColor(color);
            if(GUILayout.Button("✖️", GUILayout.Width(20)))
            {
                _Target.RemoveAt(index);
                EditorUtility.SetDirty(_Target);
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
