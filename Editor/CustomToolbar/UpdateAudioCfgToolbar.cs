using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;


[CustomUnityToolbarAttribute("Update AudioCfg", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.UpdateAudioCfg)]
public class UpdateAudioCfgToolbar : ICustomUnityToolbar
{
    public  void OnClick()
    {
        OnClick2();
    }
    public static void OnClick2()
    {
        var isChanged = false;
        var assCfgCount = ExcelEditorUtil.GetCfgCount<AssetCfg>();
        var allAudioAssetIds = new HashSet<int>();
        for (int i = 0; i < assCfgCount; i++)
        {
            var assCfg = ExcelEditorUtil.GetCfgByIndex<AssetCfg>(i);
            var assObj = AssetDatabase.LoadAssetAtPath<AudioClip>(assCfg.strPath);
            if (assCfg.strPath.Contains("BallPart1008_Effect1"))
            {

            }
            if (assObj == null)
                continue;
            allAudioAssetIds.Add(assCfg.nAssetID);
        }
        var audioCfgCount = ExcelEditorUtil.GetCfgCount<AudioCfg>();
        var curAsset2ClipId = new Dictionary<int, int>();
        for (var i = 0; i < audioCfgCount; i++)
        {
            var clipCfg = ExcelEditorUtil.GetCfgByIndex<AudioCfg>(i);
            curAsset2ClipId.Add(clipCfg.nAssetID, clipCfg.nAudioID);
        }

        foreach (var assetId in allAudioAssetIds)
        {
            if (curAsset2ClipId.ContainsKey(assetId))
                continue;
            isChanged = true;
            var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(assetId);
            var layer = assCfg.strPath.Contains("bgm", System.StringComparison.OrdinalIgnoreCase)
                ? EnAudioLayer.Bgm
                : EnAudioLayer.Tips;
            var clipId = GetNextClipId();
            var ins = ExcelEditorUtil.CreateTypeInstance<AudioCfg>();
            ExcelEditorUtil.SetCfgValue(ins, nameof(AudioCfg.nAudioID), clipId);
            ExcelEditorUtil.SetCfgValue(ins, nameof(AudioCfg.nAssetID), assetId);
            ExcelEditorUtil.SetCfgValue(ins, nameof(AudioCfg.strDescEditor), assCfg.strDescEditor);
            ExcelEditorUtil.SetCfgValue(ins, nameof(AudioCfg.nLayer), (int)layer);
            ExcelEditorUtil.AddCfg(ins);
        }


        var csContent = new StringBuilder();
        csContent.AppendLine($"public enum {typeof(EnAudioId)}");
        csContent.AppendLine("{");
        csContent.AppendLine($"\tNone = 0,");
        var audioCfgCount2 = ExcelEditorUtil.GetCfgCount<AudioCfg>();
        for (var i = 0; i < audioCfgCount2; i++)
        {
            var audioCfg = ExcelEditorUtil.GetCfgByIndex<AudioCfg>(i);
            csContent.AppendLine($"\t{audioCfg.strDescEditor.Replace("Audio_", "")} = {audioCfg.nAudioID},");

            var assetCfg = ExcelEditorUtil.GetCfg<AssetCfg>(audioCfg.nAssetID);
            var clipAsset = AssetDatabase.LoadAssetAtPath<AudioClip>(assetCfg.strPath);
            if (clipAsset == null)
            {
                Debug.LogError($"AnimationClip 资源不存在 path：{assetCfg.strPath}");
                continue;
            }
            var isLoop = audioCfg.nLayer == (int)EnAudioLayer.Bgm ? 1 : 0;
            if (audioCfg.bIsLoop != isLoop)
            {
                isChanged = true;
                ExcelEditorUtil.SetCfgValue(audioCfg, nameof(AudioCfg.bIsLoop), isLoop);
            }
            if (!Mathf.Approximately(audioCfg.fLength, clipAsset.length))
            {
                isChanged = true;
                ExcelEditorUtil.SetCfgValue(audioCfg, nameof(AudioCfg.fLength), clipAsset.length);
            }

        }
        csContent.AppendLine("}");

        File.WriteAllText(Path.Combine(Application.dataPath,"../", "Assets/Framework/Hotfix/Scripts/System/Audio/EnAudioId.cs"), csContent.ToString());

        AssetDatabase.Refresh();
        if (isChanged)
            ExcelEditorUtil.SaveExcel<AudioCfg>();
    }

    private static int GetNextClipId()
    {
        var audioCfgCount = ExcelEditorUtil.GetCfgCount<AudioCfg>();
        var id = audioCfgCount + 1;
        for (var i = 1; i < id; i++)
        {
            if (!ExcelEditorUtil.Contains<AudioCfg>(i))
                return i;
        }
        return id;
    }
}
