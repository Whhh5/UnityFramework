using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;
using UnityEngine.TextCore.Text;
using TMPro;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

[CustomUnityToolbarAttribute("builf tmp font", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.TMPFontBuilderToolbar)]
public class TMPFontBuilderToolbar : ICustomUnityToolbar
{
    public void OnClick()
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
        string fontPath = "Assets/TextMesh Pro/Resources/Fonts & Materials/LXGWWenKai-Medium SDF.asset";
        TMP_FontAsset targetFontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(fontPath);
        if (targetFontAsset == null)
        {
            EditorUtility.DisplayDialog("错误", "找不到字体资源", "确定");
            return;
        }

        string allChars = new string(charSet.ToArray());

        // 核心API：尝试批量添加字符，返回失败的字符
        bool allSuccess = targetFontAsset.TryAddCharacters(allChars, false);

        // 标记资产修改并永久保存
        EditorUtility.SetDirty(targetFontAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        string msg = allSuccess
        ? $"成功添加 {charSet.Count} 个字符到字库"
            : $"添加失败";

        EditorUtility.DisplayDialog("执行完成", msg, "确定");
    }
}
