using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMaterialSwitch : UISwitch
{
    [SerializeField]
    private List<Material> _MaterialList = new();

    public override int GetSwitchCount()
    {
        return _MaterialList.Count;
    }

    public override void Switch(int index)
    {
        if (gameObject.TryGetComponent<TextMeshProUGUI>(out var graphic))
        {
            graphic.fontMaterial = _MaterialList[index];
        }
        if (gameObject.TryGetComponent<Image>(out var img))
        {
            img.material = _MaterialList[index];
        }
    }
}
