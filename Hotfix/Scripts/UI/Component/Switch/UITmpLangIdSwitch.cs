using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]

public class UITmpLangIdSwitch : UISwitch
{
    [SerializeField]
    private List<int> _AttLangId = new();

    public override void Switch(int index)
    {
        if(index >= _AttLangId.Count)
            return;
        var itemData = _AttLangId[index];

        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.SetLanguageText(itemData);
    }

    public override int GetSwitchCount()
    {
        return _AttLangId.Count;
    }
}