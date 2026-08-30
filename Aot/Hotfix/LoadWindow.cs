using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWindow : MonoBehaviour
{
    public static LoadWindow Instance = null;
    [SerializeField]
    private Text _Text;
    [SerializeField]
    private Text _SlderText;
    [SerializeField]
    private Slider _Slider;

    private void Awake()
    {
        Instance = this;
    }
    public void SetText(string content)
    {
        _Text.text = content;
    }
    public void SetSlider(float value)
    {
        _Slider.value = value;
        _SlderText.text = $"{value * 100:F0}%";
    }
}
