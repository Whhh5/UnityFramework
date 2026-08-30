using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class HorizontalLayoutChildAspect : MonoBehaviour
{
    [Header("宽高比 (宽 ÷ 高)")]
    [SerializeField] private float aspectRatio = 1.0f;

    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // 布局系统算完之后，再矫正子物体的宽度
    private void LateUpdate()
    {
        ApplyAspectToChildren();
    }

    private void ApplyAspectToChildren()
    {
        if (_rectTransform == null) return;

        foreach (RectTransform child in _rectTransform)
        {
            if (child == null) continue;

            float height = child.rect.height;
            if (height <= 0) continue;

            float targetWidth = height * aspectRatio;

            // 只有宽度变了才设置，避免无谓的刷新
            if (!Mathf.Approximately(child.rect.width, targetWidth))
            {
                child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                LayoutRebuilder.MarkLayoutForRebuild(_rectTransform);
            }
        }
    }
}