using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public interface IEntityBuffParamsEditor : IEntityBuffData, ISkillEditor
{

}

public interface ISkillScheduleActionEditor : ISkillScheduleAction, ISkillEditor
{

}

public interface ISkillEventScheduleDataEditor : ISkillEventScheduleData, ISkillEditor
{

}

public interface ISkillScheduleActionDebugModelEditor: ISkillDebugModelEditor
{
    public void OnGuiDebugModelUpdateEditor(in Rect stageRect, in int key, ref int ctrlId);

    public void OnSceneDebugModelUpdateEditor(in SkillDebugModelTimelineInfo timeInfo);
}

public interface IPhysicsResolveEditor : IPhysicsResolve, ISkillEditor
{
    
}
public interface IPhysicsResolveDebugModelEditor : ISkillEditor, ISkillDebugModelEditor
{
    public void OnSceneUpdateEditor(in Rect stageRect, in Rect boxRect, in SkillDebugModelTimelineInfo timeInfo, in float invokeSchedule);
}