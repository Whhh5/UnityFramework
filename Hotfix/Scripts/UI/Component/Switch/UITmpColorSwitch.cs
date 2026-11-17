using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UITmpColorSwitch : UISwitch
{
    [SerializeField]
    private List<VertexGradient> _ColorList = new();

    public override int GetSwitchCount()
    {
        return _ColorList.Count;
    }

    public override void Switch(int index)
    {
        var grapgic = gameObject.GetComponent<TextMeshProUGUI>();

        var i = index * 4;

        //var ve = grapgic.colorGradient;
        //ve.topLeft = _ColorList[i++];
        //ve.topRight = _ColorList[i++];
        //ve.bottomLeft = _ColorList[i++];
        //ve.bottomRight = _ColorList[i++];
        grapgic.colorGradient = _ColorList[index];
    }
}