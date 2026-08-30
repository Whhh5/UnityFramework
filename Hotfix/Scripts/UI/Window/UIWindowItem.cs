using System.Threading;
using UnityEngine;




public abstract class UIWindowItem : MonoBehaviour
{
    protected int _GoId;

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
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    public virtual void Unload()
    {
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
}
