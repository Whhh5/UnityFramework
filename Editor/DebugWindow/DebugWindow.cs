using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class DebugWindow : EditorWindow
{
    private Dictionary<int, GameObject> _GameObjects = new();

    private List<int> _TempEntityID = new();
    private Vector3 pos;
    private float radius;

    private Vector2 inDir;
    private Vector2 normalDir;

    private int _EntityId;
    private EnEntityCmd _CmdId;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            _EntityId = EditorGUILayout.IntField(_EntityId, GUILayout.Width(100));
            _CmdId = (EnEntityCmd)EditorGUILayout.EnumPopup(_CmdId, GUILayout.Width(100));
            if (GUILayout.Button("exe", GUILayout.Width(100), GUILayout.Height(30)))
            {
                SceneEntityMgr.Instance.AddCmd(_EntityId, _CmdId);
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("create wave info", GUILayout.Width(200), GUILayout.Height(30)))
        {
            var list = new List<GameBattleWaveMonsterInfo>()
            {
                //new GameBattleWaveMonsterInfo()
                //{
                //    monsterId = 1000101,
                //    backRow = 1,
                //    col = 0,
                //    attValuesId = 201,
                //},
                //new GameBattleWaveMonsterInfo()
                //{
                //    monsterId = 1000102,
                //    backRow = 1,
                //    col = 1,
                //    attValuesId = 201,
                //},
                //new GameBattleWaveMonsterInfo()
                //{
                //    monsterId = 1000103,
                //    backRow = 1,
                //    col = 2,
                //    attValuesId = 201,
                //},
                //new GameBattleWaveMonsterInfo()
                //{
                //    monsterId = 1000104,
                //    backRow = 1,
                //    col = 4,
                //    attValuesId = 201,
                //},
                //new GameBattleWaveMonsterInfo()
                //{
                //    monsterId = 1000201,
                //    backRow = 1,
                //    col = 5,
                //    attValuesId = 201,
                //},
                new GameBattleWaveMonsterInfo()
                {
                    monsterId = 1000301,
                    backRow = 1,
                    col = 3,
                    attValuesId = 201,
                },
            };
            var arrValue = new List<int>();
            ISerializeToIntArray.SerializeToInt(ref arrValue, in list);
            var array = arrValue.ToArray();
            for (int i = 1; i <= 80; i++)
            {
                var cfg = ExcelEditorUtil.GetCfg<BattleWaveCfg>(i);
                ExcelEditorUtil.SetCfgValue(cfg, nameof(BattleWaveCfg.arrMonsterInfos), array);
            }
            ExcelEditorUtil.SaveExcel<BattleWaveCfg>();
        }
        if (GUILayout.Button("add", GUILayout.Width(100), GUILayout.Height(30)))
        {

        }

        inDir = EditorGUILayout.Vector3Field("inDir", inDir);
        normalDir = EditorGUILayout.Vector3Field("normalDir", normalDir);
        if (GUILayout.Button("Reflect", GUILayout.Width(100), GUILayout.Height(30)))
        {
            var dir = Vector2.Reflect(inDir.normalized, normalDir.normalized);
            Debug.Log(dir);
        }

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button($"update ball", GUILayout.Width(100), GUILayout.Height(30)))
            {
                var count = ExcelEditorUtil.GetCfgCount<SkillBallCfg>();
                var tempType = typeof(GameBattleSkillBall_1001Data);
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<SkillBallCfg>(i);

                    var type = Type.GetType(
                        $"{tempType.FullName}, {tempType.Assembly.FullName}"
                        .Replace("1001", $"{cfg.nBallId}"));
                    if (type == null)
                        continue;
                    var method = type.GetMethod(nameof(GameBattleSkillBall_1001Data.GetStarSerializeArray));
                    var result = (List<ISerializeToIntArray>)method.Invoke(null, null);
                    SkillBallStarLevelArray(cfg.nBallId, result);
                }
                ExcelEditorUtil.SaveExcel<SkillBallCfg>();
            }
            if (GUILayout.Button($"update ball part", GUILayout.Width(100), GUILayout.Height(30)))
            {
                var count = ExcelEditorUtil.GetCfgCount<SkillBallPartCfg>();
                var tempType = typeof(GameBattleSkillBallPart_1009Data);
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<SkillBallPartCfg>(i);

                    var type = Type.GetType(
                        $"{tempType.FullName}, {tempType.Assembly.FullName}"
                        .Replace("1009", $"{cfg.nPartId}"));
                    if (type == null)
                        continue;
                    var method = type.GetMethod(nameof(GameBattleSkillBallPart_1009Data.GetStarSerializeArray));
                    if (method == null)
                        continue;
                    var result = (List<ISerializeToIntArray>)method.Invoke(null, null);
                    SkillBallPartStarLevelArray(cfg.nPartId, result);
                }
                ExcelEditorUtil.SaveExcel<SkillBallPartCfg>();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SkillBallStarLevelArray(int cfgId, List<ISerializeToIntArray> list)
    {
        var jsonList = new List<int>();
        for (int i = 0; i < list?.Count; i++)
        {
            var ps = list[i].SerializeToIntArray();
            jsonList.Add(ps.Count);
            jsonList.AddRange(ps);
        }
        var json = JsonConvert.SerializeObject(jsonList);
        Debug.Log(json);

        var cfg = ExcelEditorUtil.GetCfg<SkillBallCfg>(cfgId);
        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.arrStarParams), jsonList.ToArray());
    }
    private void SkillBallPartStarLevelArray(int cfgId, List<ISerializeToIntArray> list)
    {
        var jsonList = new List<int>();
        for (int i = 0; i < list?.Count; i++)
        {
            var ps = list[i].SerializeToIntArray();
            jsonList.Add(ps.Count);
            jsonList.AddRange(ps);
        }
        var json = JsonConvert.SerializeObject(jsonList);
        Debug.Log(json);

        var cfg = ExcelEditorUtil.GetCfg<SkillBallPartCfg>(cfgId);
        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.arrStarParams), jsonList.ToArray());
    }
    private void CreateScene()
    {
    }
    private void Load2()
    {
    }
    private void Unload2()
    {
    }
    private void CreatePlayer()
    {
    }
    private void DestroyPlayer()
    {
    }
    private void ClearScene()
    {
    }
}