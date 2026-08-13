using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomUnityToolbarAttribute("Update ClipCfg", EnCustomUnityToolbarType.Left,
    EnCustomUnityToolbarOrder.UpdateClipCfg)]
public class UpdateClipCfgToolbar : ICustomUnityToolbar
{
    public void OnClick()
    {
        OnClick2();
    }
    public static void OnClick2()
    {
        var isChanged = false;
        var assCfgCount = ExcelEditorUtil.GetCfgCount<AssetCfg>();
        var allClipAssetIds = new HashSet<int>();
        for (int i = 0; i < assCfgCount; i++)
        {
            var assCfg = ExcelEditorUtil.GetCfgByIndex<AssetCfg>(i);
            var assObj = AssetDatabase.LoadAssetAtPath<AnimationClip>(assCfg.strPath);
            if (assObj == null)
                continue;
            allClipAssetIds.Add(assCfg.nAssetID);
        }
        var clipCfgCount = ExcelEditorUtil.GetCfgCount<ClipCfg>();
        var curAsset2ClipId = new Dictionary<int, int>();
        for (var i = 0; i < clipCfgCount; i++)
        {
            var clipCfg = ExcelEditorUtil.GetCfgByIndex<ClipCfg>(i);
            curAsset2ClipId.Add(clipCfg.nAssetID, clipCfg.nClipID);
        }

        foreach (var assetId in allClipAssetIds)
        {
            if (curAsset2ClipId.ContainsKey(assetId))
                continue;
            isChanged = true;
            var assCfg = ExcelEditorUtil.GetCfg<AssetCfg>(assetId);
            var clipId = GetNextClipId();
            var ins = ExcelEditorUtil.CreateTypeInstance<ClipCfg>();
            ExcelEditorUtil.SetCfgValue(ins, nameof(ClipCfg.nClipID), clipId);
            ExcelEditorUtil.SetCfgValue(ins, nameof(ClipCfg.nAssetID), assetId);
            ExcelEditorUtil.SetCfgValue(ins, nameof(ClipCfg.strDescEditor), assCfg.strDescEditor);
            ExcelEditorUtil.SetCfgValue(ins, nameof(ClipCfg.nLayer), (int)EnAnimLayer.Body);
            ExcelEditorUtil.AddCfg(ins);
        }
        var clipCfgCount2 = ExcelEditorUtil.GetCfgCount<ClipCfg>();
        for (var i = 0; i < clipCfgCount2; i++)
        {
            var clipCfg = ExcelEditorUtil.GetCfgByIndex<ClipCfg>(i);
            var assetCfg = ExcelEditorUtil.GetCfg<AssetCfg>(clipCfg.nAssetID);
            var clipAsset = AssetDatabase.LoadAssetAtPath<AnimationClip>(assetCfg.strPath);
            if (clipAsset == null)
            {
                Debug.LogError($"AnimationClip 资源不存在 path：{clipCfg.nAssetID}:{assetCfg.strPath}");
                continue;
            }
            if (clipCfg.bIsLoop != (clipAsset.isLooping ? 1 : 0))
            {
                isChanged = true;
                ExcelEditorUtil.SetCfgValue(clipCfg, nameof(ClipCfg.bIsLoop), clipAsset.isLooping ? 1 : 0);
            }
            if (!Mathf.Approximately(clipCfg.fLength, clipAsset.length))
            {
                isChanged = true;
                ExcelEditorUtil.SetCfgValue(clipCfg, nameof(ClipCfg.fLength), clipAsset.length);
            }

        }

        if (isChanged)
            ExcelEditorUtil.SaveExcel<ClipCfg>();
    }

    private static int GetNextClipId()
    {
        var clipCfgCount = ExcelEditorUtil.GetCfgCount<ClipCfg>();
        var id = clipCfgCount + 1;
        for (var i = 1; i < id; i++)
        {
            if (!ExcelEditorUtil.Contains<ClipCfg>(i))
                return i;
        }
        return id;
    }
}
