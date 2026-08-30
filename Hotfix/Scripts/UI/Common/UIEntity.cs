using UnityEngine;

public abstract class UIEntityData<TEntity, TUserData> : UIEntityData
    where TEntity : UIEntity
    where TUserData : GameEntityUserData
{
    protected TEntity _UIEntity = null;
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UIEntity = entity as TEntity;
    }
    public override void OnGODestroy()
    {
        base.OnGODestroy();
        _UIEntity = null;
    }

    public sealed override void OnPoolInit(GameEntityUserData userData)
    {
        base.OnPoolInit(userData);
        OnPoolInit(userData as TUserData);
    }

    protected abstract void OnPoolInit(TUserData userData);
}
public abstract class UIEntityData : GameEntityData
{
    private UIEntity _UIEntity = null;
    private Vector2 _AnchoredPos2D;
    public Vector2 AnchoredPos2D => _AnchoredPos2D;
    public override void OnGODestroy()
    {
        _AnchoredPos2D = Vector2.zero;
        _UIEntity = null;
        base.OnGODestroy();
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _UIEntity = entity as UIEntity;

        _UIEntity.SetAnchoredPos2D(in _AnchoredPos2D);
    }
    public void SetAnchoredPos2D(in Vector2 anchorPos)
    {
        _AnchoredPos2D = anchorPos;
        if (_IsLoadSuccess)
            _UIEntity.SetAnchoredPos2D(in anchorPos);
    }
}

public abstract class UIEntity: GameEntity
{
    private RectTransform m_Rect = null;
    public override void LoadCompletion()
    {
        base.LoadCompletion();
        m_Rect = GetComponent<RectTransform>();
    }

    public void SetAnchoredPos2D(in Vector2 localPos)
    {
        m_Rect.anchoredPosition3D = localPos;
    }
}
