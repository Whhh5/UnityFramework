using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteMask2DSlider))]
public class SpriteMask2DSliderEditor : Editor
{
    private SpriteMask2DSlider _Target = null;
    private void OnEnable()
    {
        _Target = target as SpriteMask2DSlider;
    }
    private void OnDisable()
    {
        _Target = null;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        var minValue = _Target.GetMinValue();
        var maxValue = _Target.GetMaxValue();
        var sliderValue = _Target.GetSliderValue();
        var axis = _Target.GetAxis();
        var model = _Target.GetModel();

        EditorGUILayout.BeginHorizontal();
        {
            for (var i = EnSpriteMask2DSliderAxis.None + 1; i < EnSpriteMask2DSliderAxis.Count; i++)
            {
                if (i == axis)
                {
                    var col = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    GUILayout.Button($"{i}", GUILayout.Height(30));
                    GUI.backgroundColor = col;
                }
                else if (GUILayout.Button($"{i}", GUILayout.Height(30)))
                {
                    Undo.RegisterCompleteObjectUndo(_Target, "Set Axis");
                    _Target.SetAxis(i);
                    EditorUtility.SetDirty(_Target);
                    Undo.RegisterCompleteObjectUndo(_Target, "Set Axis");
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            for (var i = EnSpriteMask2DSliderChangeModel.None + 1; i < EnSpriteMask2DSliderChangeModel.Count; i++)
            {
                if (i == model)
                {
                    var col = GUI.backgroundColor;
                    GUI.backgroundColor = Color.green;
                    GUILayout.Button($"{i}", GUILayout.Height(30));
                    GUI.backgroundColor = col;
                }
                else if (GUILayout.Button($"{i}", GUILayout.Height(30)))
                {
                    Undo.RegisterCompleteObjectUndo(_Target, "Set Model");
                    _Target.SetModel(i);
                    EditorUtility.SetDirty(_Target);
                    Undo.RegisterCompleteObjectUndo(_Target, "Set Model");
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
            var width = EditorUtil.GetTextWidth($"{minValue}") + 20;
            var minValue2 = EditorGUILayout.DelayedFloatField(minValue, GUILayout.Width(50));
            if (minValue2 != minValue)
            {
                Undo.RegisterCompleteObjectUndo(_Target, "Set Min Value");
                _Target.SetMinValue(minValue2);
                EditorUtility.SetDirty(_Target);
                Undo.RegisterCompleteObjectUndo(_Target, "Set Min Value");
            }
            var sliderValue2 = EditorGUILayout.Slider(sliderValue, 0, 1);
            if (sliderValue2 != sliderValue)
            {
                Undo.RegisterCompleteObjectUndo(_Target, "Set Slider Value");
                _Target.SetSliderValue(sliderValue2);
                EditorUtility.SetDirty(_Target);
                Undo.RegisterCompleteObjectUndo(_Target, "Set Slider Value");
            }
            var width2 = EditorUtil.GetTextWidth($"{maxValue}") + 20;
            var maxValue2 = EditorGUILayout.DelayedFloatField(maxValue, GUILayout.Width(50));
            if (maxValue2 != maxValue)
            {
                Undo.RegisterCompleteObjectUndo(_Target, "Set Max Value");
                _Target.SetMaxValue(maxValue2);
                EditorUtility.SetDirty(_Target);
                Undo.RegisterCompleteObjectUndo(_Target, "Set Max Value");
            }
        }
        EditorGUILayout.EndHorizontal();

    }
}
