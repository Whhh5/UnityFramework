using UnityEngine;




public abstract class UIWindowItem : MonoBehaviour
{
    protected int _GoId;

    public void SetGoId(int goId)
    {
        _GoId = goId;
    }
    public int GetGoId()
    {
        return _GoId;
    }

    public virtual void LoadCompeletion()
    {
        transform.localPosition = Vector3.zero;
    }
    public virtual void Unload()
    {
        _GoId = -1;
    }
}
