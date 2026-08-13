using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageUtility
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

    // 获得
    public static string GetLanguage_10001()
    {
        return GetLanguage(10001);
    }
    // 配件
    public static string GetLanguage_10002()
    {
        return GetLanguage(10002);
    }
    // {0}碎片
    public static string GetLanguage_10003()
    {
        return GetLanguage(10003);
    }
    public static string GetLanguage_10004()
    {
        return GetLanguage(10004);
    }
    public static string GetLanguage_10005()
    {
        return GetLanguage(10005);
    }
    public static string GetLanguage_10006()
    {
        return GetLanguage(10006);
    }
    public static string GetLanguage_10007()
    {
        return GetLanguage(10007);
    }
    public static string GetLanguage_10008()
    {
        return GetLanguage(10008);
    }
    // {0}/{1}
    public static string GetLanguage_1009(object p1, object p2)
    {
        return string.Format(GetLanguage(10009), p1, p2);
    }
    public static string GetLanguage_10010()
    {
        return GetLanguage(10010);
    }
    public static string GetLanguage_1011(object p2)
    {
        return string.Format(GetLanguage(10011), p2);
    }
    public static string GetLanguage_10013()
    {
        return GetLanguage(10013);
    }

    public static string GetLanguage_DamageTypeName(EnDamageType damageType)
    {
        var damageCfg = GameSchedule.Instance.GetDamageTypeCfg0((int)damageType);
        var styleName = GetLanguage(damageCfg.nStyleNameLangId);
        var value = string.Format(styleName, damageCfg.strColor);
        return value;
    }


    public static string GetLanguage_BallName(int ballId)
    {
        var ballCfg = GameSchedule.Instance.GetSkillBallCfg0(ballId);
        var langStr = LanguageUtility.GetLanguage(ballCfg.nNameLangId);
        return langStr;
    }
    public static string GetLanguage_BallDescription(int ballId)
    {
        var ballCfg = GameSchedule.Instance.GetSkillBallCfg0(ballId);
        var langStr = LanguageUtility.GetLanguage(ballCfg.nDescLangId);
        return langStr;
    }
    public static string GetLanguage_BuffStyleName(EnBuff buffId)
    {
        var buffCfg = GameSchedule.Instance.GetBuffCfg0((int)buffId);
        var bffName = LanguageUtility.GetLanguage(buffCfg.nStyleNameLangId);
        return bffName;
    }
    public static string GetLanguage_BuffDescription(EnBuff buffId)
    {
        var buffCfg = GameSchedule.Instance.GetBuffCfg0((int)buffId);
        var bffName = LanguageUtility.GetLanguage(buffCfg.nDescLangId);
        return bffName;
    }

    public static string GetLanguage_AttiributeName(EnEntityAttributeId attId)
    {
        var attCfg = GameSchedule.Instance.GetAttributeCfg0((int)attId);
        var attName = LanguageUtility.GetLanguage(attCfg.nNameLangId);
        return attName;
    }
    public static string GetLanguage_AttributeValue(EnEntityAttributeId attId, float value)
    {
        var attCfg = GameSchedule.Instance.GetAttributeCfg0((int)attId);
        var result = (EnValueType)attCfg.nValueType switch
        {
            EnValueType.Percentage => value * 100,
            _ => value,
        };
        var valueStr = string.Format(GetLanguage(attCfg.nValueDescLangId), result);
        return valueStr;
    }

    public static string GetLanguage_BallPartName(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillBallPartCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nNameLangId);
        return desc;
    }
    public static string GetLanguage_BallPartStyleName(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillBallPartCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nStyleNameLangId);
        return desc;
    }
    public static string GetLanguage_BallPartDescription(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillBallPartCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nDescLangId);
        return desc;
    }
    public static string GetLanguage_SkillPassivityName(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillPassivityCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nNameLangId);
        return desc;
    }
    public static string GetLanguage_SkillPassivityDescription(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillPassivityCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nDescLangId);
        return desc;
    }
    public static string GetLanguage_SkillPassivityTipsDescription(int partId)
    {
        var partCfg = GameSchedule.Instance.GetSkillPassivityCfg0(partId);
        var desc = LanguageUtility.GetLanguage(partCfg.nTipsDescLangId);
        return desc;
    }

}
