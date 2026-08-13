using UnityEditor;
using UnityEngine;

public partial class SkillAudioScheduleActionEditor : SkillAudioScheduleAction, ISkillScheduleActionEditor
{
    public void OnSkillEditorGUI()
    {
        EditorGUILayout.BeginVertical();
        {
            _Schedule = EditorGUILayout.Slider(_Schedule, 0, 1);
            _AudioId = (EnAudioId)EditorGUILayout.EnumPopup(_AudioId);

            EditorGUILayout.BeginHorizontal();
            {
                if (_AudioId > 0)
                {
                    var audioCfg = ExcelEditorUtil.GetCfg<AudioCfg>((int)_AudioId);
                    var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(audioCfg.nAssetID);
                    var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assCfg.strPath);
                    EditorGUILayout.ObjectField(clip, typeof(AudioClip), false, GUILayout.Width(200));

                    if (GUILayout.Button("Play", GUILayout.Width(100)))
                    {
                        AudioEditorPlayer.PlayClip(clip);
                    }
                    if (GUILayout.Button("Stop", GUILayout.Width(100)))
                    {
                        AudioEditorPlayer.StopAllClips();
                    }

                }
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
    [EditorField(nameof(_AudioId))]
    private EnAudioId _AudioId
    {
        get => this.GetFieldValue(nameof(_AudioId), EnAudioId.None);
        set => this.SetFieldValue(nameof(_AudioId), value);
    }
}
