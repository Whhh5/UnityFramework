using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class LanguageUtility
{

    public static string GetLanguage(int langId)
    {
        var langCfg = GameSchedule.Instance.GetLanguageCfg0(langId);
        return langCfg.strValueCh;
    }
    public static string GetLanguage(int langId, params object[] format)
    {
        var langCfg = GameSchedule.Instance.GetLanguageCfg0(langId);
        return string.Format(langCfg.strValueCh, format);
    }


}
