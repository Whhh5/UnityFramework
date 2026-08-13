using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

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

        if (GUILayout.Button("test fussion", GUILayout.Width(100), GUILayout.Height(30)))
        {
            if (Selection.assetGUIDs.Length > 0)
            {
                var logStr = new StringBuilder();
                for (int i = 0; i < Selection.assetGUIDs.Length; i++)
                {
                    var guid = Selection.assetGUIDs[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var hasCode = path.GetHashCode();
                    logStr.AppendLine($"{path}:{hasCode}");
                }
                Debug.Log(logStr);
            }
        }
        if (GUILayout.Button("check temp monster icon", GUILayout.Width(100), GUILayout.Height(30)))
        {
            CheckMonsterTempIcon();
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
                var tempType = typeof(GameBattleSkillBall1001Data);
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<SkillBallCfg>(i);

                    var type = Type.GetType(
                        $"{tempType.FullName}, {tempType.Assembly.FullName}"
                        .Replace("1001", $"{cfg.nBallId}"));
                    if (type == null)
                        continue;
                    var method = type.GetMethod(nameof(GameBattleSkillBall1001Data.GetStarSerializeArray));
                    var result = (List<ISerializeToIntArray>)method.Invoke(null, null);
                    SkillBallStarLevelArray(cfg.nBallId, result);
                }
                ExcelEditorUtil.SaveExcel<SkillBallCfg>();
            }
            if (GUILayout.Button($"update ball part", GUILayout.Width(100), GUILayout.Height(30)))
            {
                var count = ExcelEditorUtil.GetCfgCount<SkillBallPartCfg>();
                var tempType = typeof(GameBattleSkillBallPart1009Data);
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<SkillBallPartCfg>(i);

                    var type = Type.GetType(
                        $"{tempType.FullName}, {tempType.Assembly.FullName}"
                        .Replace("1009", $"{cfg.nPartId}"));
                    if (type == null)
                        continue;
                    var method = type.GetMethod(nameof(GameBattleSkillBallPart1009Data.GetStarSerializeArray));
                    if (method == null)
                        continue;
                    var result = (List<ISerializeToIntArray>)method.Invoke(null, null);
                    SkillBallPartStarLevelArray(cfg.nPartId, result);
                }
                ExcelEditorUtil.SaveExcel<SkillBallPartCfg>();
            }
            if (GUILayout.Button($"update skill passicity", GUILayout.Width(100), GUILayout.Height(30)))
            {
                var count = ExcelEditorUtil.GetCfgCount<SkillPassivityCfg>();
                var tempType = typeof(GameBattleSkillPassivity2001Data);
                for (int i = 0; i < count; i++)
                {
                    var cfg = ExcelEditorUtil.GetCfgByIndex<SkillPassivityCfg>(i);

                    var type = Type.GetType(
                        $"{tempType.FullName}, {tempType.Assembly.FullName}"
                        .Replace("2001", $"{cfg.nSkillPassivityId}"));
                    if (type == null)
                        continue;
                    var method = type.GetMethod(nameof(GameBattleSkillPassivity2001Data.GetStarSerializeArray));
                    if (method == null)
                        continue;
                    var result = (List<ISerializeToIntArray>)method.Invoke(null, null);
                    SkillPassivityStarLevelArray(cfg.nSkillPassivityId, result);
                }
                ExcelEditorUtil.SaveExcel<SkillPassivityCfg>();
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
    private void SkillPassivityStarLevelArray(int cfgId, List<ISerializeToIntArray> list)
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

        var cfg = ExcelEditorUtil.GetCfg<SkillPassivityCfg>(cfgId);
        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.arrStarParams), jsonList.ToArray());
    }
    private async void CheckMonsterTempIcon()
    {
        var monsterCount = ExcelEditorUtil.GetCfgCount<MonsterCfg>();
        for (int i = 0; i < monsterCount; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<MonsterCfg>(i);
            if (cfg.nAssetCfgID <= 0)
                continue;
            var savePath = Path.Combine(Application.dataPath, "../", $"Assets/GameRes/HotfixRes/Images/UI/Icon", $"img_icon_monster_{cfg.nMonsterID}.png");
            if (File.Exists(savePath))
                continue;

            // 复制贴图，开启可读（关键！原贴图不能直接编码）
            var assetCfg = ExcelEditorUtil.GetCfg<AssetCfg>(cfg.nAssetCfgID);
            if (assetCfg == null)
                continue;
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(assetCfg.strPath);
            if (go == null)
                continue;
            var icon = AssetPreview.GetAssetPreview(go);
            var curTime = DateTime.Now;
            await UniTask.WaitUntil(() => icon != null || (DateTime.Now - curTime).Seconds > 5);
            if (icon == null)
            {
                Debug.LogError($"Preview load error: {go}");
                continue;
            }
            Texture2D exportTex = CopyTexture2D(icon);

            // 编码PNG
            byte[] pngData = exportTex.EncodeToPNG();
            System.IO.File.WriteAllBytes(savePath, pngData);

        }
        AssetDatabase.Refresh();
    }

    // 复制Texture，保证可读
    private static Texture2D CopyTexture2D(Texture source)
    {
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
        Graphics.Blit(source, rt);

        Texture2D newTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        newTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        newTex.Apply();

        RenderTexture.ReleaseTemporary(rt);
        return newTex;
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