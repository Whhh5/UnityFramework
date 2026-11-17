using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum EnSpriteMask2DSliderAxis
{
    None,
    x,
    y,
    Count,
}
public enum EnSpriteMask2DSliderChangeModel
{
    None,
    Standard,
    Lerp,
    Count,
}
public class SpriteMask2DSlider : MonoBehaviour
{
    private float _MinValue;
    [SerializeField]
    private float _MaxValue;
    [SerializeField]
    private float _SliderValue;
    [SerializeField]
    private EnSpriteMask2DSliderAxis _Axis;
    [SerializeField]
    private EnSpriteMask2DSliderChangeModel _Model;

    private void OnDisable()
    {
        DOTween.Kill(GetInstanceID());
    }
    public float GetMinValue()
    {
        return _MinValue;
    }
    public void SetMinValue(float value)
    {
        _MinValue = value;
        Apply();
    }
    public float GetMaxValue()
    {
        return _MaxValue;
    }
    public void SetMaxValue(float value)
    {
        _MaxValue = value;
        Apply();
    }
    public float GetSliderValue()
    {
        return _SliderValue;
    }
    public void SetSliderValue(float sliderValue)
    {
        _SliderValue = sliderValue;
        Apply();
    }
    public EnSpriteMask2DSliderAxis GetAxis()
    {
        return _Axis;
    }
    public void SetModel(EnSpriteMask2DSliderChangeModel model)
    {
        if (_Model == model)
            return;
        if (_Model == EnSpriteMask2DSliderChangeModel.Lerp)
        {
            DOTween.Kill(GetInstanceID());
        }
        _Model = model;
        Apply();
    }
    public EnSpriteMask2DSliderChangeModel GetModel()
    {
        return _Model;
    }
    public void SetAxis(EnSpriteMask2DSliderAxis axis)
    {
        _Axis = axis;
        Apply();
    }

    public void Apply()
    {
        var scale = transform.localScale;
        var value = Mathf.Lerp(_MinValue, _MaxValue, _SliderValue);
        var testValue = 0f;
        switch (_Axis)
        {
            case EnSpriteMask2DSliderAxis.x:
                testValue = value - scale.x;
                scale.x = value;
                break;
            case EnSpriteMask2DSliderAxis.y:
                testValue = value - scale.y;
                scale.y = value;
                break;
            default:
                break;
        }

        switch (_Model)
        {
            case EnSpriteMask2DSliderChangeModel.Lerp:
                {
                    if (Application.isPlaying)
                    {
                        DOTween.Kill(GetInstanceID());
                        var time = Mathf.Abs(testValue) / (_MaxValue - _MinValue);
                        DOTween.To(() => 0f, sliderValue =>
                        {
                            var lerpValue = Vector3.Lerp(transform.localScale, scale, sliderValue);
                            transform.localScale = lerpValue;
                        }, 1f, time)
                            .SetUpdate(true)
                            .SetId(GetInstanceID());
                    }
                    else
                    {
                        transform.localScale = scale;
                    }
                }
                break;
            default:
                transform.localScale = scale;
                break;
        }
    }
}
