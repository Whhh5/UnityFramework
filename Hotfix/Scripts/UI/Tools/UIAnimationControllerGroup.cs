using System.Collections;
using System.Collections.Generic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;

public class UIAnimationControllerGroup : MonoBehaviour
{
    private readonly List<IUIAnimationController> _Ctls = new();

    public void Register(IUIAnimationController com)
    {
        _Ctls.Add(com);
        com.Init();
    }

    public void SetSliderValue(float value)
    {
        foreach (var item in _Ctls)
        {
            item.SetSliderValue(value);
        }
    }

    public void Unregister(IUIAnimationController com)
    {
        com.Destroy();
        _Ctls.Remove(com);
    }

    public void UnregisterAll()
    {
        foreach (var item in _Ctls)
        {
            item.Destroy();
        }
        _Ctls.Clear();
    }
}
