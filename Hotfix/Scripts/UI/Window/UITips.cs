using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITipsUserData : UIWindowUserData
{

}
public abstract class UITipsData<TEntity, TUserData> : UITipsData
    where TEntity : UITips
    where TUserData : UITipsUserData
{
    protected TEntity _UITips = null;

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _UITips = null;
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UITips = entity as TEntity;
    }
    protected override void OnPoolInit(UIWindowUserData userData)
    {
        base.OnPoolInit(userData);
        OnPoolInit(userData as TUserData);
    }
    protected abstract void OnPoolInit(TUserData userData);
}
public abstract class UITipsData<T> : UITipsData
    where T : UITips
{
    protected T _UITips = null;

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        _UITips = null;
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UITips = entity as T;
    }
}

public abstract class UITipsData : UIWindowData
{
    public sealed override EnUIWindowType WindowType => EnUIWindowType.Tips;
}
public abstract class UITips : UIWindow
{
    public virtual bool IsClickClose()
    {
        return true;
    }
    protected override void Update()
    {
        base.Update();

        if (IsClickClose())
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UIMgr.Instance.IsValid(_WindowId))
                {
                    UIMgr.Instance.HideWindow(_WindowId);
                }
            }
        }
    }
}
