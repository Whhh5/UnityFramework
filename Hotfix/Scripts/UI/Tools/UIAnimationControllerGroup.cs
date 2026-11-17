using System.Collections.Generic;
using UnityEngine;

public class UIAnimationControllerGroup : MonoBehaviour, IUIAnimationSliderValue
{
    private readonly List<IUIAnimationController> _Ctls = new();

    public void Register(IUIAnimationController com)
    {
        _Ctls.Add(com);
    }

    public void SetShowSliderValue(float value)
    {
        foreach (var item in _Ctls)
        {
            item.SetShowSliderValue(value);
        }
    }
    public void SetHideSliderValue(float value)
    {
        foreach (var item in _Ctls)
        {
            item.SetHideSliderValue(value);
        }
    }

    public void Unregister(IUIAnimationController com)
    {
        _Ctls.Remove(com);
    }

    public void UnregisterAll()
    {
        _Ctls.Clear();
    }
}
