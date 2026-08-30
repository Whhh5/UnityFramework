using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[CustomUnityToolbarAttribute("Update Load Target", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.UpdateLoadTarget)]
public class UpdateLoadTargetToolbar : ICustomUnityToolbar
{
    public void OnClick()
    {
        ExcelEditorUtil.ClearCache();
        LoadConfigEditor.CreateLoadConfigJson();
        UpdateClipCfgToolbar.OnClick2();
        UpdateAudioCfgToolbar.OnClick2();
        TMPFontBuilderToolbar.OnClick2();
        CreateGlobalFile();
        UnityEditor.AssetDatabase.Refresh();
    }

    private void CreateGlobalFile()
    {
        var globalCfgUtilityFilePath = ABBUtil.GetFullPathByUnityPath("Assets/Framework/Hotfix/Scripts/Cfg/GlobalCfgUtility.cs");
        var fileContent = new StringBuilder();
        fileContent.AppendLine("public static class GlobalCfgUtility");
        fileContent.AppendLine("{");

        var count = ExcelEditorUtil.GetCfgCount<GlobalCfg>();
        for (int i = 0; i < count; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<GlobalCfg>(i);
            var type = cfg.nIntValue.GetType();
            fileContent.AppendLine($"\t/// <summary>");
            fileContent.AppendLine($"\t/// {cfg.strDescEditor}");
            fileContent.AppendLine($"\t/// </summary>");
            fileContent.AppendLine($"\t/// <returns>{type.FullName}</returns>");
            fileContent.AppendLine($"\tpublic static {type.FullName} Get{cfg.nId}_{type.Name}()");
            fileContent.AppendLine($"\t{{");
            fileContent.AppendLine($"\t\tvar cfg = {nameof(GameSchedule)}.{nameof(GameSchedule.Instance)}.{nameof(GameSchedule.GetGlobalCfg0)}({cfg.nId});");
            fileContent.AppendLine($"\t\treturn cfg.{nameof(GlobalCfg.nIntValue)};");
            fileContent.AppendLine($"\t}}");
        }
        fileContent.AppendLine("}");
        File.WriteAllText(globalCfgUtilityFilePath, fileContent.ToString());
    }
}