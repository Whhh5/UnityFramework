
using System;
using UnityEngine;

public partial class SkillRandomPlayableAdapterEditor : SkillRandomPlayableAdapter, ISkillEditor, ISkillPlayableAdapterGetStageList
{
    private SkillPlayableAdapterDrawStageEditor _PlayableAdapterEditor = new();
    public void OnSkillEditorGUI()
    {
        _PlayableAdapterEditor.SetDrawDataList(this);
        _PlayableAdapterEditor.OnSkillEditorGUI();
    }
    
    public SkillStageInfoEditor[] GetSkillStageInfoEditors()
    {
        return _SkillStageList;
    }

    public void SetSkillStageInfoEditors(SkillStageInfoEditor[] data)
    {
        _SkillStageList = data;
    }


    [EditorField(nameof(_SkillStageList))]
    private SkillStageInfoEditor[] _SkillStageList
    {
        get => this.GetFieldValue<SkillStageInfoEditor[]>(nameof(_SkillStageList));
        set => this.SetFieldValue(nameof(_SkillStageList), value);
    }

    public override EnAnimLayerBit GetAnimationLayers()
    {
        var result = EnAnimLayerBit.None;
        foreach (var item in _SkillStageList)
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