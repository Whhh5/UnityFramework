using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugSkillBallWindow : EditorWindow
{
    private void OnGUI()
    {
        DrawSkillBallButton();

    }
    private void DrawSkillBallButton()
    {
        DrawSKillBallSelect(position);
        EditorGuiUtil.DrawLinkHor(10);
        DrawSKillPassivitySelect(position);
    }

    #region skill ball
    private void DrawSKillBallSelect(Rect rect)
    {
        var btnSize = new Vector2(200, 30);
        var colCount = Mathf.FloorToInt(rect.width / btnSize.x);
        if (colCount == 0)
            return;
        EditorGUILayout.BeginVertical();
        {
            var ballCount = ExcelEditorUtil.GetCfgCount<SkillBallCfg>();
            var ballDic = new Dictionary<int, List<int>>();
            for (int i = 0; i < ballCount; i++)
            {
                var ballCfg = ExcelEditorUtil.GetCfgByIndex<SkillBallCfg>(i);
                if (!ballDic.TryGetValue(ballCfg.nBallUseType, out var ballList))
                {
                    ballList = new();
                    ballDic.Add(ballCfg.nBallUseType, ballList);
                }
                ballList.Add(ballCfg.nBallId);
            }
            foreach (var (_, ballList) in ballDic)
            {
                var listCount = ballList.Count;
                var rowCount = Mathf.CeilToInt(listCount / (float)colCount);
                for (int row = 0; row < rowCount; row++)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        for (int col = 0; col < colCount; col++)
                        {
                            var index = row * colCount + col;
                            if (index >= listCount)
                                break;
                            var ballId = ballList[index];
                            var ballCfg = ExcelEditorUtil.GetCfg<SkillBallCfg>(ballId);
                            var level = GetBallLevel(ballCfg.nBallId);
                            var curBgCol = GUI.backgroundColor;
                            if (level > 0)
                                GUI.backgroundColor = level > 0 ? Color.green : curBgCol;

                            var iconAssCfg = ExcelEditorUtil.GetCfg<AssetCfg>(ballCfg.nIconAssetId);
                            var iconImg = iconAssCfg != null ? AssetDatabase.LoadAssetAtPath<Texture>(iconAssCfg.strPath) : null;
                            var content = new GUIContent()
                            {
                                text = $"{ballCfg.strDescEditor}:{ballCfg.nBallId}:{level}",
                                image = iconImg,
                            };
                            if (GUILayout.Button(content, GUILayout.Width(btnSize.x), GUILayout.Height(btnSize.y)))
                            {
                                if (Event.current.button == 0)
                                {
                                    AddBall(ballCfg.nBallId);
                                }
                                else if (Event.current.button == 1)
                                {
                                    RemoveBall(ballCfg.nBallId);
                                }
                            }
                            if (level > 0)
                                GUI.backgroundColor = curBgCol;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGuiUtil.DrawLinkHor(5);
            }
        }
        EditorGUILayout.EndVertical();
    }
    private int GetBallLevel(int ballId)
    {
        if (!Application.isPlaying)
            return 0;
        var star = GameBattleBallMgr.Instance.GetBallStar(ballId);
        return star;
    }
    private void AddBall(int ballId)
    {
        if (!Application.isPlaying)
            return;
        GameBattleBallMgr.Instance.AddBall(ballId);
    }
    private void RemoveBall(int ballId)
    {
        if (!Application.isPlaying)
            return;
        GameBattleBallMgr.Instance.RemoveBall(ballId);
    }

    #endregion
    #region skill passivity
    private void DrawSKillPassivitySelect(Rect rect)
    {
        var btnSize = new Vector2(200, 30);
        var colCount = Mathf.FloorToInt(rect.width / btnSize.x);
        if (colCount == 0)
            return;
        EditorGUILayout.BeginVertical();
        {
            var ballCount = ExcelEditorUtil.GetCfgCount<SkillPassivityCfg>();
            var rowCount = Mathf.CeilToInt(ballCount / (float)colCount);
            for (int row = 0; row < rowCount; row++)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        var index = row * colCount + col;
                        if (index >= ballCount)
                            break;
                        var ballCfg = ExcelEditorUtil.GetCfgByIndex<SkillPassivityCfg>(index);
                        var level = GetSkillPassivityLevel(ballCfg.nSkillPassivityId);
                        var curBgCol = GUI.backgroundColor;
                        if (level > 0)
                            GUI.backgroundColor = level > 0 ? Color.green : curBgCol;

                        var iconAssCfg = ExcelEditorUtil.GetCfg<AssetCfg>(ballCfg.nIconAssetId);
                        var iconImg = iconAssCfg != null ? AssetDatabase.LoadAssetAtPath<Texture>(iconAssCfg.strPath) : null;
                        var content = new GUIContent()
                        {
                            text = $"{ballCfg.strDescEditor}:{ballCfg.nSkillPassivityId}:{level}",
                            image = iconImg,
                        };
                        if (GUILayout.Button(content, GUILayout.Width(btnSize.x), GUILayout.Height(btnSize.y)))
                        {
                            if (Event.current.button == 0)
                            {
                                AddSkillPassivity(ballCfg.nSkillPassivityId);
                            }
                            else if (Event.current.button == 1)
                            {
                                RemoveSkillPassivity(ballCfg.nSkillPassivityId);
                            }
                        }
                        if (level > 0)
                            GUI.backgroundColor = curBgCol;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndVertical();
    }
    private int GetSkillPassivityLevel(int ballId)
    {
        if (!Application.isPlaying)
            return 0;
        var star = GameSkillPassivityMgr.Instance.GetStar(ballId);
        return star;
    }
    private void AddSkillPassivity(int ballId)
    {
        if (!Application.isPlaying)
            return;
        GameSkillPassivityMgr.Instance.AddSkillPassivity(ballId);
    }
    private void RemoveSkillPassivity(int ballId)
    {
        if (!Application.isPlaying)
            return;
        GameSkillPassivityMgr.Instance.RemoveSkillPassivity(ballId);
    }

    #endregion

}
