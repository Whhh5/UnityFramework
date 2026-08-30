using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomUnityToolbarAttribute("builf tmp font", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.TMPFontBuilderToolbar)]
public class TMPFontBuilderToolbar : ICustomUnityToolbar
{
    public void OnClick()
    {
        OnClick2();
    }
    public static void OnClick2()
    {
        var cfgCount = ExcelEditorUtil.GetCfgCount<LanguageCfg>();
        var charSet = new HashSet<char>(1000);
        for (int i = 0; i < cfgCount; i++)
        {
            var cfg = ExcelEditorUtil.GetCfgByIndex<LanguageCfg>(i);
            if (cfg.strValueCh != null)
            {
                for (int j = 0; j < cfg.strValueCh.Length; j++)
                    charSet.Add(cfg.strValueCh[j]);
            }
            if (cfg.strValueEn != null)
            {
                for (int j = 0; j < cfg.strValueEn.Length; j++)
                    charSet.Add(cfg.strValueEn[j]);
            }
        }

        // 读取字体资源路径
        var targetFontAsset = TMP_Settings.defaultFontAsset;
        if (targetFontAsset == null)
        {
            Debug.LogError("找不到字体资源");
            return;
        }
        var orgMode = targetFontAsset.atlasPopulationMode;
        if (orgMode == AtlasPopulationMode.Static)
        {
            targetFontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;
        }

        var allChars = new string(charSet.ToArray());

        // 核心API：尝试批量添加字符，返回失败的字符
        var allSuccess = targetFontAsset.TryAddCharacters(allChars, out var missingCharacters, false);

        targetFontAsset.atlasPopulationMode = orgMode;
        // 标记资产修改并永久保存
        EditorUtility.SetDirty(targetFontAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        var msg = allSuccess
        ? $"成功添加 {charSet.Count} 个字符到字库"
            : $"添加失败/已经存在字符  missing characters: {missingCharacters}";
        Debug.Log(msg);
    }
}
