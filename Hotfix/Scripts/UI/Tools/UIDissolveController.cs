using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#if !UNITY_TEST
//[RequireComponent(typeof(Graphic))]
public class UIDissolveController : MonoBehaviour, IUIAnimationController
{
    [SerializeField]
    private Material _ImgMat = null;
    [SerializeField]
    private Dictionary<TextMeshProUGUI, (Material ori, Material cur)> _TmpMats = new(20);
    [SerializeField, Range(0, 1)]
    private float _SliderValue = 0;

    private void Awake()
    {
        var txt = UIMgr.Instance.GetNoiseTexture();
        {
            var imgs = gameObject.GetComponentsInChildren<Image>(true);
            var shader = UIMgr.Instance.GetImageDissolveShader();
            _ImgMat = new Material(shader);
            _ImgMat.SetTexture("_NoiseTex", txt);
            foreach (var item in imgs)
            {
                item.material = _ImgMat;
            }

        }
        {
            var temps = gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
            var shader = UIMgr.Instance.GetTmpDissolveShader();
            foreach (var item in temps)
            {
                var mat = new Material(item.fontMaterial)
                {
                    shader = shader
                };
                _TmpMats.Add(item, (item.fontMaterial, mat));
                item.fontMaterial = mat;
            }
        }
    }
    private void OnEnable()
    {
        SetSliderValue(_SliderValue);
    }

    private void OnDestroy()
    {
        GameObject.Destroy(_ImgMat);
        foreach (var (tmp, mats) in _TmpMats)
        {
            tmp.fontMaterial = mats.ori;
            GameObject.Destroy(mats.cur);
        }
        _ImgMat = null;
        _TmpMats.Clear();
        _SliderValue = -1;
    }

    private void SetSliderValue(float slider)
    {
        _SliderValue = Mathf.Clamp01(slider);
        if (gameObject.activeSelf)
        {
            _ImgMat.SetFloat("_DissolveThreshold", _SliderValue);
            foreach (var (tmp, mats) in _TmpMats)
            {
                mats.cur.SetFloat("_DissolveThreshold", _SliderValue);
                tmp.ForceMeshUpdate(true, true);
            }
        }
    }

    public void SetShowSliderValue(float value)
    {
        SetSliderValue(1 - value);
    }

    public void SetHideSliderValue(float value)
    {
        SetSliderValue(value);
    }
}
#endif