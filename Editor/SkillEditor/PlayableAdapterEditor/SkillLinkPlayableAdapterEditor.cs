using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class SkillLinkPlayableAdapterEditor : SkillLinkPlayableAdapter, ISkillEditor,
    ISkillPlayableAdapterGetStageList, ISkillPlayableAdapterGetBuffEditor
{
    private SkillPlayableAdapterDrawStageEditor _PlayableAdapterEditor = new();
    private SkillPlayableAdapterDrawBuffEditor _BuffEditor = new();

    public void OnSkillEditorGUI()
    {
        _PlayableAdapterEditor.SetDrawDataList(this);
        _BuffEditor.SetSkillBuffParamsEditor(this);
        EditorGUILayout.BeginVertical();
        {
            _BuffEditor.OnSkillEditorGUI();

            GUILayout.Space(SkillEditorDefine.DefineSpace);
            GUILayout.Button("", GUILayout.Height(SkillEditorDefine.LineSize));
            GUILayout.Space(SkillEditorDefine.DefineSpace);

            _PlayableAdapterEditor.OnSkillEditorGUI();

            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }

    public SkillStageInfoEditor[] GetSkillStageInfoEditors()
    {
        return _DataList;
    }

    public void SetSkillStageInfoEditors(SkillStageInfoEditor[] data)
    {
        _DataList = data;
    }

    public IEntityBuffParamsEditor[] GetSkillBuffParamsEditor()
    {
        return _BuffInfoList;
    }

    public void SetSkillBuffParamsEditor(IEntityBuffParamsEditor[] data)
    {
        _BuffInfoList = data;
    }

    [EditorField(nameof(_DataList))]
    private SkillStageInfoEditor[] _DataList
    {
        get => this.GetFieldValue(nameof(_DataList), default(SkillStageInfoEditor[]));
        set => this.SetFieldValue(nameof(_DataList), value);
    }

    [EditorField(nameof(_BuffInfoList))]
    private IEntityBuffParamsEditor[] _BuffInfoList
    {
        get => this.GetFieldValue(nameof(_BuffInfoList), default(IEntityBuffParamsEditor[]));
        set => this.SetFieldValue(nameof(_BuffInfoList), value);
    }

    public override EnAnimLayerBit GetAnimationLayers()
    {
        var result = EnAnimLayerBit.None;
        foreach (var item in _DataList)
        {
            var clipId = item.GetClipID();
            if (clipId <= 0)
                continue;
            var clipCfg = ExcelEditorUtil.GetCfg<ClipCfg>(item.GetClipID());
            if (clipCfg == null)
            {
                Debug.LogError($"clip no exist {item.GetClipID()}");
                continue;
            }
            result |= (EnAnimLayerBit)(1 << clipCfg.nLayer);
        }
        return result;
    }
}