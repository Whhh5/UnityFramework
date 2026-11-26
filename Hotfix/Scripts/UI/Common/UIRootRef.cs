using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRootRef : MonoBehaviour
{

    [SerializeField]
    private Canvas _MainCanvas = null;
    public Canvas MainCanvas => _MainCanvas;
    [SerializeField]
    private RectTransform _CanvasRect = null;
    public RectTransform CanvasRect => _CanvasRect;
    [SerializeField]
    private RectTransform _UIRootRect = null;
    public RectTransform UIRootRect => _UIRootRect;
    [SerializeField]
    private RectTransform _UITipsRootRect = null;
    public RectTransform UITipsRootRect => _UITipsRootRect;



    private void Awake()
    {
        UIMgr.Instance.SetUIRootRef(this);
    }
}
