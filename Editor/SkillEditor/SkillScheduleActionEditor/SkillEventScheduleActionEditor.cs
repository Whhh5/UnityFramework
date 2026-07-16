using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Spine;
using UnityEditor;

public partial class SkillEventScheduleActionEditor : SkillEventScheduleAction, ISkillScheduleActionEditor
{

    private HashSet<EnTypeId> _ValidTypeIds = new();

    public void OnSkillEditorGUI()
    {
        if (_ValidTypeIds.Count == 0)
        {
            _ValidTypeIds.Add(EnTypeId.None);
            var idNames = Enum.GetNames(typeof(EnTypeId));
            for (int i = 0; i < idNames.Length; i++)
            {
                var enName = idNames[i];
                var type = Type.GetType($"{SkillEditorDefine.GetEditorName(enName)}, {typeof(ISkillEventScheduleDataEditor).Assembly.FullName}");
                if (type == null)
                    continue;
                if (!typeof(ISkillEventScheduleDataEditor).IsAssignableFrom(type))
                    continue;
                _ValidTypeIds.Add(Enum.Parse<EnTypeId>(enName));
            }
        }


        EditorGUILayout.BeginVertical();
        {
            _Schedule = EditorGUILayout.FloatField(nameof(_Schedule), _Schedule);
            EditorGuiUtil.DrawEnum(_EventId, select => _EventId = select);

            var defClassId = EnTypeId.None;
            if (_EventData != null)
            {
                defClassId = _EventData.GetTypeDefineId();
            }
            EditorGuiUtil.DrawEnum(defClassId, select =>
            {
                if (defClassId == select)
                    return;
                var type = Type.GetType($"{SkillEditorDefine.GetEditorName(select.ToString())}, {typeof(ISkillEventScheduleDataEditor).Assembly.FullName}");

                if (_EventData != null)
                {
                    if (type == null)
                    {
                        if (!EditorUtility.DisplayDialog("提示", "删除数据？", "ok", "cancel"))
                        {
                            _EventData = null;
                            return;
                        }
                    }
                    else
                    {
                        if (!EditorUtility.DisplayDialog("提示", "更换数据？", "ok", "cancel"))
                            return;
                    }
                }
                if (type == null)
                    return;
                var ins = Activator.CreateInstance(type);
                EditorUtil.ChangeEditorStruct((IEditor)ins);
                _EventData = (ISkillEventScheduleDataEditor)ins;

            }, null, (name) =>
            {
                return _ValidTypeIds.Contains(name);
            });

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.Space(20);
                _EventData?.OnSkillEditorGUI();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }



    [EditorField(nameof(_Schedule))]
    private float _Schedule
    {
        get => this.GetFieldValue(nameof(_Schedule), 0f);
        set => this.SetFieldValue(nameof(_Schedule), value);
    }
    [EditorField(nameof(_EventId))]
    private EnSkillEventScheduleId _EventId
    {
        get => this.GetFieldValue(nameof(_EventId), EnSkillEventScheduleId.None);
        set => this.SetFieldValue(nameof(_EventId), value);
    }
    [EditorField(nameof(_EventData))]
    private ISkillEventScheduleDataEditor _EventData
    {
        get => this.GetFieldValue<ISkillEventScheduleDataEditor>(nameof(_EventData), default);
        set => this.SetFieldValue(nameof(_EventData), value);
    }
}
