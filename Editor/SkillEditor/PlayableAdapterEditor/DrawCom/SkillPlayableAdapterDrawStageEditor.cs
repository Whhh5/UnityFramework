

using System.Linq;
using UnityEditor;
using UnityEngine;

public class SkillPlayableAdapterDrawStageEditor
{
    private ISkillPlayableAdapterGetStageList _SatgeController = null;
    private int _DrawDataIndex = -1;
    public void SetDrawDataList(ISkillPlayableAdapterGetStageList drawEditor)
    {
        _SatgeController = drawEditor;
    }
    public void OnSkillEditorGUI()
    {
        var stageInfoEditors = _SatgeController.GetSkillStageInfoEditors();
        EditorGUILayout.BeginVertical();
        {
            if (GUILayout.Button("Add Data"))
            {
                var dataList = stageInfoEditors?.ToList() ?? new();
                dataList.Add(new SkillStageInfoEditor());
                stageInfoEditors = dataList.ToArray();
                _SatgeController.SetSkillStageInfoEditors(stageInfoEditors);
                _DrawDataIndex = dataList.Count - 1;
            }

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Button("", GUILayout.Width(3), GUILayout.ExpandHeight(true));
                GUILayout.Space(SkillEditorDefine.TabWidth);
                var dataList = stageInfoEditors;
                var length = dataList?.Length ?? 0;
                if (length == 0)
                {
                    EditorGUILayout.HelpBox("select item", MessageType.Warning);
                }
                else
                {
                    DrawDataMenuList();

                    GUILayout.Space(10);
                    if (_DrawDataIndex >= 0 && _DrawDataIndex < dataList?.Length)
                        dataList[_DrawDataIndex].OnSkillEditorGUI();

                    GUILayout.FlexibleSpace();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
    private void DrawDataMenuList()
    {
        var stageInfoEditors = _SatgeController.GetSkillStageInfoEditors();
        EditorGUILayout.BeginVertical();
        {
            for (var i = 0; i < stageInfoEditors?.Length; i++)
            {
                var index = i;
                var style = SkillEditorUtil.GetButtonStyle(_DrawDataIndex == index);
                if (!GUILayout.Button($"{index}", style, GUILayout.Width(SkillEditorDefine.MenuWidth)))
                    continue;
                switch (Event.current.button)
                {
                    case 0:
                        _DrawDataIndex = index;
                        break;
                    case 1:
                        {
                            var menu = new GenericMenu();
                            menu.AddItem(new GUIContent($"remove {index}"), false, () =>
                            {
                                if (!EditorUtility.DisplayDialog("remove", $"index: {index}", "ok", "cancel"))
                                    return;
                                if (index <= _DrawDataIndex)
                                    _DrawDataIndex--;
                                var tempList = stageInfoEditors.ToList();
                                tempList.RemoveAt(index);
                                stageInfoEditors = tempList.ToArray();
                                _SatgeController.SetSkillStageInfoEditors(stageInfoEditors);
                            });
                            menu.ShowAsContext();
                        }
                        break;
                }
            }
        }
        EditorGUILayout.EndVertical();
    }
}