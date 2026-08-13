


using UnityEngine;

public class SceneMonsterEntityUserData : SceneEntityUserData
{
    public int monsterId;
    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        monsterId = -1;
    }
}
public abstract class SceneMonsterEntityData<T, TUserData> : SceneMonsterEntityData
    where T : SceneMonsterEntity
    where TUserData : SceneMonsterEntityUserData
{
    protected T _SceneMonsterEntity;
    protected abstract void OnPoolInit(TUserData userData);
    protected sealed override void OnPoolInit(SceneMonsterEntityUserData userData)
    {
        base.OnPoolInit(userData);
        OnPoolInit(userData as TUserData);
    }
    public override void OnGOCreate(GameEntity entity)
    {
        base.OnGOCreate(entity);
        _SceneMonsterEntity = entity as T;
    }
    public override void OnGODestroy()
    {
        base.OnGODestroy();
        _SceneMonsterEntity = null;
    }
}
public class SceneMonsterEntityData : SceneEntityData<SceneMonsterEntity, SceneMonsterEntityUserData>
{

    private int _MonsterCfgId;
    private EnSceneEntityStatus _SceneEntityStatus;
    private EnGameLayerInt _EnemyLayerInt;
    private EnGameLayerInt _FriendLayerInt;
    private EnGameLayerInt _LayerInt;
    private float _Volume;
    public override EnLoadTarget LoadTarget
    {
        get
        {
            var cfg = GameSchedule.Instance.GetMonsterCfg0(_MonsterCfgId);
            return (EnLoadTarget)cfg.nAssetCfgID;
        }
    }

    public override void Destroy()
    {
        base.Destroy();

        _Volume
            = _MonsterCfgId
            = -1;
        _EnemyLayerInt
            = _FriendLayerInt
            = _LayerInt
            = EnGameLayerInt.None;
        _SceneEntityStatus = EnSceneEntityStatus.None;
    }
    protected override void OnPoolInit(SceneMonsterEntityUserData userData)
    {
        _MonsterCfgId = userData.monsterId;

        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(userData.monsterId);
        for (int i = 0; i < monsterCfg.arrEnemyLayer.Length; i++)
            _EnemyLayerInt |= (EnGameLayerInt)(1 << monsterCfg.arrEnemyLayer[i]);
        for (int i = 0; i < monsterCfg.arrFriendLayer.Length; i++)
            _FriendLayerInt |= (EnGameLayerInt)(1 << monsterCfg.arrFriendLayer[i]);
        for (int i = 0; i < monsterCfg.arrLayer.Length; i++)
            _LayerInt |= (EnGameLayerInt)(1 << monsterCfg.arrLayer[i]);

        _Volume = monsterCfg.fVolume;
    }
    public ref readonly int GetMonsterCfgId()
    {
        return ref _MonsterCfgId;
    }
    public float GetVolume()
    {
        return _Volume;
    }

    public ref readonly EnSceneEntityStatus GetSceneEntityStatus()
    {
        return ref _SceneEntityStatus;
    }
    public bool ContainsStatus(EnSceneEntityStatus status)
    {
        if ((_SceneEntityStatus & status) == status)
            return true;
        return false;
    }
    public bool IsStatus(EnSceneEntityStatus status)
    {
        if (_SceneEntityStatus == status)
            return true;
        return false;
    }
    public void AddMonsterStatus(EnSceneEntityStatus status)
    {
        if (ContainsStatus(status))
            return;
        _SceneEntityStatus |= status;
    }
    public void RemoveMonsterStatus(EnSceneEntityStatus status)
    {
        if (!ContainsStatus(status))
            return;
        _SceneEntityStatus &= ~status;
    }
    public void SetSceneEntityStatus(EnSceneEntityStatus status)
    {
        if (_SceneEntityStatus == status)
            return;
        _SceneEntityStatus = status;
    }
    public ref readonly EnGameLayerInt GetEnemyLayer()
    {
        return ref _EnemyLayerInt;
    }
    public ref readonly EnGameLayerInt GetFriendLayer()
    {
        return ref _FriendLayerInt;
    }
    public ref readonly EnGameLayerInt GetLayer()
    {
        return ref _LayerInt;
    }
}

public abstract class SceneMonsterEntity : SceneEntity, IEntityAnimCom
{
    private Animator _Animator = null;
    public Animator GetAnimator()
    {
        if (_Animator == null)
        {
            if (!gameObject.TryGetComponent<Animator>(out _Animator))
            {
                _Animator = gameObject.AddComponent<Animator>();
            }
        }
        return _Animator;
    }
}


