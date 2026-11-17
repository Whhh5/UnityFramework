using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UISpriteSwitch : UISwitch
{
    [SerializeField]
    private List<UISpriteSwitchItem> _ArrSprite = new();

    public override void Switch(int index)
    {
        if(index >= _ArrSprite.Count)
            return;
        var itemData = _ArrSprite[index];

        var img = GetComponent<Image>();
        img.sprite = itemData.GetSprite();
        img.color = itemData.GetColor();
    }

    public override int GetSwitchCount()
    {
        return _ArrSprite.Count;
    }
    public UISpriteSwitchItem GetArrSprite(int index)
    {
        return _ArrSprite[index];
    }
    public void RemoveAt(int index)
    {
        _ArrSprite.RemoveAt(index);
    }
    public void AddSprite()
    {
        _ArrSprite.Add(new());
    }
}