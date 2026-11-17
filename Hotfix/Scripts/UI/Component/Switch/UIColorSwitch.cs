using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIColorSwitch : UISwitch
{
    [SerializeField]
    private List<Color> _ColorList = new();

    public override int GetSwitchCount()
    {
        return _ColorList.Count;
    }

    public override void Switch(int index)
    {
        var grapgic = gameObject.GetComponent<Graphic>();
        grapgic.color = _ColorList[index];
    }
}
