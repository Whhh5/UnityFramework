using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleAnimation : MonoBehaviour, IUIAnimationController
{
    //[SerializeField, AutoReference("root")]
    private RectTransform _Root;

    private bool _IsShow = true;
    private float _SliderValue;
    private void OnDisable()
    {
        _Root = null;
        _SliderValue = 0;
        _IsShow = true;
    }

    private void OnEnable()
    {
        var root = transform.Find("root");
        if (root == null)
            return;
        _Root = root.GetComponent<RectTransform>();
        if (_IsShow)
        {
            SetShowSliderValue(_SliderValue);
        }
        else
        {
            SetHideSliderValue(_SliderValue);
        }
    }

    public void SetHideSliderValue(float value)
    {
        _SliderValue = value;
        _IsShow = false;
        if (_Root == null)
            return;
        _Root.localScale = Vector3.one * Mathf.Lerp(1, 1.2f, value);
    }

    public void SetShowSliderValue(float value)
    {
        _SliderValue = value;
        _IsShow = true;
        if (_Root == null)
            return;
        var addValue = 0.2f;
        var v = Mathf.Sin(Mathf.PI * value) * addValue;
        _Root.localScale = Vector3.one * ((1 - addValue) + Mathf.Lerp(0, addValue, value) + v);
    }

}
