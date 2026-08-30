using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using Object = UnityEngine.Object;

public class LoadConfigEditor
{
    private static Dictionary<string, string> m_Suffix2NamePrefix = new()
    {
        {".asset", "SO_" },
        {".prefab", "Pre_" },
        {".png", "Sp_" },
        {".FBX", "Anim_" },
        {".anim", "Anim_" },
        {".mask", "AvaMask_" },
        {".json", "Json_" },
        {".txt", "Txt_" },
        {".bytes", "Byte_" },
        {".ttf", "Ttf_" },
        {".mp3", "Audio_" },
        {".wav", "Audio_" },
        {".shader", "Shader_" },
        {".shadergraph", "Shadergraph_" },
        {".mat", "Mat_" },
        {".spriteatlasv2", "Atlas_" },
    };
    //private static string[] m_SearchPaths = new string[]
    //{
    //    "Assets/GameRes/HotfixRes",
    //    "Assets/GameRes/Shaders", 
    //    "Assets/Framework/Hotfix/Res",
    //    "Assets/Framework/Editor/EditorRes",
    //};
    public static bool AssetIsValid(string assetName)
    {
        var fix = Path.GetExtension(assetName);
        if (!m_Suffix2NamePrefix.ContainsKey(fix))
            return false;
        return true;
    }
    public static string GetLoadTargetName(string assetName)
    {
        var fix = Path.GetExtension(assetName);
        if (!m_Suffix2NamePrefix.TryGetValue(fix, out var value))
            return null;
        var name = Path.GetFileNameWithoutExtension(assetName);
        var result = $"{value}{name}"
            .Replace(".", "_")
            .Replace(" ", "")
            .Replace("-", "_")
            .Replace(")", "")
            .Replace("(", "");
        return result;
    }
    private static string m_ConfigJsonPath => ABBUtil.GetFullPathByUnityPath(GlobalConfig.LoadConfigRecordsJson);
    private static string m_EnLoadTargetPath => ABBUtil.GetFullPathByUnityPath(GlobalConfig.LoadTargetEnum);
    [MenuItem("Tools/Load/UpdateConfigJson")]
    public static void CreateLoadConfigJson()
    {
        var curAssetDic = new Dictionary<string, AssetCfg>();
        var assetCfgCount = ExcelEditorUtil.GetCfgCount<AssetCfg>();
        for (int i = 0; i < assetCfgCount; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<AssetCfg>(i);
            var editorName = GetLoadTargetName(cfg.strPath);
            curAssetDic.Add(editorName, cfg);
            if (cfg.strDescEditor != editorName)
            {
                ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.strDescEditor), editorName);
            }
        }

        var searchPaths = AssetbundleBuilderSO.Instance.abPaths;
        foreach (var item in m_Suffix2NamePrefix)
        {
            foreach (var searchPath in searchPaths)
            {
                var fullDir = ABBUtil.GetFullPathByUnityPath(searchPath);
                if (!Directory.Exists(fullDir))
                    continue;
                var dirInfo = new DirectoryInfo(fullDir);
                var fileList = dirInfo.GetFiles($"*{item.Key}", SearchOption.AllDirectories);
                foreach (var fileInfo in fileList)
                {
                    var fullPath = fileInfo.FullName;
                    var strPath = ABBUtil.GetUnityPathByFullPath(fullPath);
                    var strDescEditor = GetLoadTargetName(fullPath);

                    if (string.IsNullOrEmpty(strDescEditor))
                        continue;
                    if (!curAssetDic.TryGetValue(strDescEditor, out var cfg))
                    {
                        cfg = ExcelEditorUtil.CreateTypeInstance<AssetCfg>();
                        var assetID = ExcelEditorUtil.GetNextIndex<AssetCfg>();
                        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.strDescEditor), strDescEditor);
                        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.nAssetID), assetID);
                        curAssetDic.Add(strDescEditor, cfg);
                        ExcelEditorUtil.AddCfg(cfg);
                    }
                    if (cfg.strPath != strPath)
                        ExcelEditorUtil.SetCfgValue(cfg, nameof(cfg.strPath), strPath);
                }
            }
        }
        CheckInvalidAsset();
        WriteEnLoadTargetFile();
        ExcelEditorUtil.SaveExcel<AssetCfg>();
    }
    public static void CheckInvalidAsset()
    {
        var assetCfgCount = ExcelEditorUtil.GetCfgCount<AssetCfg>();
        for (int i = 0; i < assetCfgCount; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<AssetCfg>(i);
            var ass = AssetDatabase.LoadAssetAtPath<Object>(cfg.strPath);
            if (ass != null)
                continue;
            ExcelEditorUtil.RemoveCfgByIndex<AssetCfg>(i--);
            assetCfgCount--;
        }
    }
    private static void WriteEnLoadTargetFile()
    {
        var enumStr = new StringBuilder();
        enumStr.AppendLine($"public enum EnLoadTarget");
        enumStr.AppendLine($"{{");
        enumStr.AppendLine($"\tNone = 0,");


        var count = ExcelEditorUtil.GetCfgCount<AssetCfg>();
        for (int i = 0; i < count; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<AssetCfg>(i);

            enumStr.AppendLine($"\t{cfg.strDescEditor} = {cfg.nAssetID}, // {cfg.strPath}");
        }

        enumStr.AppendLine("}");
        File.WriteAllText(m_EnLoadTargetPath, enumStr.ToString());
        AssetDatabase.Refresh();
    }
}

