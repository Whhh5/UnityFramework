using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class SkillAudioScheduleActionEditor : ISkillScheduleActionDebugModelEditor
{
    private Rect _BoxRect = default;
    private Rect _StageRect = default;
    private bool _IsPlayed = false;
    public void OnDebugModelEnterEditor(GameObject go)
    {

    }

    public void OnDebugModelExitEditor()
    {

    }

    public void OnGuiDebugModelUpdateEditor(in Rect stageRect, in int key, ref int ctrlId)
    {
        var (schedule, boxRect) = SkillDebugModelUtil.DrawStageScheduleActionBox(this, in stageRect, _Schedule, key, ref ctrlId);
        _BoxRect = boxRect;
        _StageRect = stageRect;
        if (schedule != _Schedule)
        {
            _Schedule = schedule;
        }
    }

    public void OnSceneDebugModelUpdateEditor(in SkillDebugModelTimelineInfo timeInfo)
    {
        var curShowTime = timeInfo.skillTime - timeInfo.StageLocalSliderToSkillTime(_Schedule);
        var state = curShowTime > 0 && timeInfo.stageLocalSlider >= _Schedule;
        if (!state)
        {
            _IsPlayed = false;
            return;
        }
        if (_IsPlayed)
            return;
        _IsPlayed = true;

        if (_AudioId > EnAudioId.None)
        {
            var audioCfg = ExcelEditorUtil.GetCfg<AudioCfg>((int)_AudioId);
            var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(audioCfg.nAssetID);
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assCfg.strPath);
            if (clip != null)
                AudioEditorPlayer.PlayClip(clip);
        }
    }
}
