using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class KeepAspectByLayoutHeight : MonoBehaviour, ILayoutSelfController
{
    [SerializeField] private float aspectRatio = 1.0f;

    // 公开属性，外部代码修改时自动触发重建
    public float AspectRatio
    {
        get => aspectRatio;
        set
        {
            if (!Mathf.Approximately(aspectRatio, value))
            {
                aspectRatio = value;
                MarkLayoutForRebuild();
            }
        }
    }

    private RectTransform _rect;
    private RectTransform rectTransform
    {
        get
        {
            if (_rect == null) _rect = GetComponent<RectTransform>();
            return _rect;
        }
    }

    public void SetLayoutHorizontal()
    {
        // 水平布局阶段：根据当前高度设置宽度
        float height = rectTransform.rect.height;
        if (height > 0)
        {
            rectTransform.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                height * aspectRatio
            );
            MarkLayoutForRebuild();
        }
    }

    public void SetLayoutVertical()
    {
        // 垂直阶段不需要处理
    }

    /// <summary>
    /// 标记当前 RectTransform 需要重新布局
    /// </summary>
    private void MarkLayoutForRebuild()
    {
        if (gameObject.activeSelf && _rect != null)
        {
            LayoutRebuilder.MarkLayoutForRebuild(_rect);
        }
    }

    // Inspector 里修改参数时自动触发
    private void OnValidate()
    {
        MarkLayoutForRebuild();
    }

    // 组件启用时也触发一次
    private void OnEnable()
    {
        MarkLayoutForRebuild();
    }
}