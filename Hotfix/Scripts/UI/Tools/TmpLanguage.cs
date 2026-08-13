using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TmpLanguage : MonoBehaviour
{
    [SerializeField]
    private int _LangId;

    private void OnEnable()
    {
        UpdateLanguage(_LangId);
    }

    public int GetLangId()
    {
        return _LangId;
    }
    public void SetLangId(in int langId)
    {
        _LangId = langId;
    }
    private void UpdateLanguage(in int langId)
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.SetLanguageText(langId);
    }
}
