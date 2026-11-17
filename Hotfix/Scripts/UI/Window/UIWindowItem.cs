using System.Threading;
using UnityEngine;




public abstract class UIWindowItem : MonoBehaviour
{
    [SerializeField, AutoReference(SearchScope.Self)]
    protected RectTransform _Rect;


    protected int _GoId;
    private Vector2 _SizeDelta;

    private CancellationTokenSource _TokenSource;
    protected CancellationToken Token
    {
        get
        {
            if (_TokenSource == null)
            {
                _TokenSource = new();
            }
            return _TokenSource.Token;
        }
    }
    public void SetGoId(int goId)
    {
        _GoId = goId;
    }
    public int GetGoId()
    {
        return _GoId;
    }

    public virtual void LoadCompletion()
    {
        _SizeDelta = _Rect.rect.size;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    public virtual void Unload()
    {
        _Rect.sizeDelta = _SizeDelta;
        if (_TokenSource != null)
        {
            _TokenSource.Cancel();
            _TokenSource = null;
        }
        _GoId = -1;
    }
    public void SetParent(RectTransform parent)
    {
        transform.SetParent(parent);
    }
    public void SetAutoSize()
    {
        _Rect.pivot = Vector2.one * 0.5f;
        _Rect.anchorMin = Vector2.zero;
        _Rect.anchorMax = Vector2.one;
        _Rect.anchoredPosition
            = _Rect.sizeDelta
            = Vector2.zero;
    }
    public void SetSizeDelta(in Vector2 sizeDelta)
    {
        _Rect.sizeDelta = sizeDelta;
    }
    public Vector2 GetSizeDelta()
    {
        return _Rect.sizeDelta;
    }
    public void SetWorldPos(in Vector3 worldPos)
    {
        _Rect.position = worldPos;
    }
    public Vector3 GetWorldPos()
    {
        return _Rect.position;
    }
    public void SetSiblingIndex(in int index)
    {
        _Rect.SetSiblingIndex(index);
    }

    public float GetLocalScale()
    {
        return _Rect.localScale.x;
    }
    public virtual void SetLocalScale(float scale)
    {
        _Rect.localScale = Vector2.one * scale;
    }
    public void SetLocalPos(float posX)
    {
        var pos = _Rect.anchoredPosition;
        pos.x = posX;
        _Rect.anchoredPosition = pos;
    }
    public float GetLocalPosX()
    {
        return _Rect.anchoredPosition.x;
    }
}
