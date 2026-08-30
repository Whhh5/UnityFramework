using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TmpLanguage))]
public class TmpLanguageEditor : Editor
{
    private TmpLanguage _Target;
    private void OnEnable()
    {
        _Target = target as TmpLanguage;
    }
    public override void OnInspectorGUI()
    {
        var langId = _Target.GetLangId();
        var col = GUI.color;
        var cfg = ExcelEditorUtil.GetCfg<LanguageCfg>(langId);
        if (cfg == null)
            GUI.color = Color.red;
        var changeLangId = EditorGUILayout.DelayedIntField("Lang Id", langId, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        GUI.color = col;
        if (changeLangId != langId)
        {
            if (ExcelEditorUtil.GetCfg<LanguageCfg>(changeLangId) != null)
            {
                _Target.SetLangId(changeLangId);
                EditorUtility.SetDirty(_Target);
            }
        }
        if (cfg != null && GUILayout.Button("Apply", GUILayout.Height(50)))
        {
            _Target.GetComponent<TextMeshProUGUI>().SetText(cfg.strValueCh);
            EditorUtility.SetDirty(_Target);
        }
    }
}
