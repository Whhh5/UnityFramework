using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillEventScheduleActionEditor : ISkillScheduleActionDebugModelEditor
{
    private Rect _BoxRect = default;
    private Rect _StageRect = default;
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
    }
}
