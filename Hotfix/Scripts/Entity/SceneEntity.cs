using System.Collections.Generic;
using UnityEngine;


public abstract class SceneEntityUserData : GameEntityUserData
{
    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
    }
}
public abstract class SceneEntityData<T, TUserData> : SceneEntityData<T>
    where T : SceneEntity
    where TUserData : SceneEntityUserData
{
    protected abstract void OnPoolInit(TUserData userData);
    protected sealed override void OnPoolInit(SceneEntityUserData userData)
    {
        base.OnPoolInit(userData);
        OnPoolInit(userData as TUserData);
    }
}
public abstract class SceneEntityData<T> : SceneEntityData
    where T : SceneEntity
{
    protected T _SceneEntity = null;
    public override void OnGOCreate()
    {
        base.OnGOCreate();
        _SceneEntity = _Entity as T;
    }
    public override void OnGODestroy()
    {
        base.OnGODestroy();
        _SceneEntity = null;
    }
}
public abstract class SceneEntityData : GameEntityData<SceneEntity, SceneEntityUserData>
{

    private readonly Dictionary<EnSceneEntityComType, IEntity3DComData> _EntityComs = new();
    
    private EnSceneEntityControllerType _ControllerType = EnSceneEntityControllerType.None;

    public override void Destroy()
    {
        base.Destroy();

        _ControllerType =  EnSceneEntityControllerType.None;
    }

    protected override void OnPoolInit(SceneEntityUserData userData)
    {

    }



    public void AddControllerType(in EnSceneEntityControllerType controllerType)
    {
        _ControllerType |= controllerType;
    }

    public void RemoveControllerType(in EnSceneEntityControllerType controllerType)
    {
        _ControllerType &= ~controllerType;
    }

    public bool IsCanController(in EnSceneEntityControllerType controllerType)
    {
        return controllerType >= _ControllerType;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        foreach (var item in _EntityComs)
        {
            if (!item.Value.IsActive())
                item.Value.OnEnable();
        }
    }
    public override void OnDisable()
    {
        base.OnDisable();

        foreach (var item in _EntityComs)
        {
            if (item.Value.IsActive())
                item.Value.OnDisable();
        }
    }

    public override void OnGOCreate()
    {
        base.OnGOCreate();
        foreach (var item in _EntityComs)
            item.Value.OnCreateGO();
    }
    public override void OnGODestroy()
    {
        foreach (var item in _EntityComs)
            item.Value.OnDestroyGO();
        base.OnGODestroy();
    }

    #region coms
    public T GetEntityCom<T>(in EnSceneEntityComType comType)
        where T : class, IEntity3DComData
    {
        var entityCom = GetEntityCom(comType);
        return entityCom as T;
    }
    public IEntity3DComData GetEntityCom(in EnSceneEntityComType comType)
    {
        if (!_EntityComs.TryGetValue(comType, out var entityCom))
            return default;
        return entityCom;
    }
    public bool AddEntityCom(in EnSceneEntityComType comType)
    {
        var userData = ClassPoolMgr.Instance.Pull<Entity3DComDataUserData>();
        userData.entityID = _EntityID;
        var entityCom = SceneEntityComUtility.CreateSceneEntityComData(in comType, userData);
        ClassPoolMgr.Instance.Push(userData);
        _EntityComs.Add(entityCom.GetComType(), entityCom);
        if (_IsLoadSuccess)
            entityCom.OnCreateGO();
        if (entityCom.IsCanActive())
            entityCom.OnEnable();
        return true;
    }

    public bool RemoveEntityCom(in EnSceneEntityComType comType)
    {
        if (!_EntityComs.TryGetValue(comType, out var entityCom))
            return false;
        DestroyComContent(entityCom);
        SceneEntityComUtility.DestroySceneEntityComData(entityCom);
        _EntityComs.Remove(comType);
        return true;
    }
    public void RemoveAllEntityCom()
    {
        foreach (var (_, entityCom) in _EntityComs)
        {
            DestroyComContent(entityCom);
            ClassPoolMgr.Instance.Push(entityCom);
        }
        _EntityComs.Clear();
    }
    private void DestroyComContent(IEntity3DComData entityCom)
    {
        if (entityCom.IsActive())
            entityCom.OnDisable();
        if (_IsLoadSuccess)
            entityCom.OnDestroyGO();
        entityCom.OnPoolDestroy();
    }
    public bool ContainsEntityCom(in EnSceneEntityComType comType)
    {
        if (!_EntityComs.ContainsKey(comType))
            return false;
        return true;
    }
    #endregion

}

public abstract class SceneEntity<T> : SceneEntity
    where T : SceneEntityData
{
    public T GetEntityData()
    {
        return _EntityData as T;
    }
}
public abstract class SceneEntity : GameEntity
{
    public static implicit operator int(SceneEntity entity3D)
    {
        return entity3D._Entity3DData.EntityID;
    }
    private SceneEntityData _Entity3DData = null;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void LoadCompeletion()
    {
        _Entity3DData = _EntityData as SceneEntityData;
        base.LoadCompeletion();
    }

}