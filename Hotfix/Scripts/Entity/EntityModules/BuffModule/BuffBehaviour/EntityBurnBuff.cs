using System.Collections.Generic;
using UnityEngine;


public class EntityBurnBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public EnDamageType damageType;
    public float damageValuePercentage;
    public int maxDamageValue;
    public float interval;
    public int maxCount;

    public override EnBuff GetBuff() => EnBuff.Burn;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityBurnBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, damageType);
        ISerializeToIntArray.SerializeToInt(ref list, in duration);
        ISerializeToIntArray.SerializeToInt(ref list, in damageValuePercentage);
        ISerializeToIntArray.SerializeToInt(ref list, in maxDamageValue);
        ISerializeToIntArray.SerializeToInt(ref list, in interval);
        ISerializeToIntArray.SerializeToInt(ref list, in maxCount);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out damageType);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out damageValuePercentage);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxDamageValue);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out interval);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
    }

    public float GetTime()
    {
        return duration;
    }
}
public class EntityBurnBuff : EntityBuff<EntityBurnBuffData>, IUpdate
{
    private EnDamageType _DamageType;
    private float _DamageValuePercentage;
    private int _MaxDamageValue;
    private float _Interval;
    private int _MaxCount;

    private int _BuffEntityId;
    private float _LastFireTime;
    private int _TargetLimitHp = -1;
    public override void OnPoolDestroy()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_BuffEntityId);
        base.OnPoolDestroy();
        _DamageType = EnDamageType.None;
        _Interval
            = _LastFireTime
            = _DamageValuePercentage
            = _BuffEntityId
            = _MaxDamageValue
            = _TargetLimitHp
            = -1;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        _BuffEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_fire, in worldPos);

        UpdateMgr.Instance.Registener(this);
    }
    protected override void AddKey(int addKey, EntityBurnBuffData buffParams)
    {
        _DamageType = buffParams.damageType;
        _DamageValuePercentage = buffParams.damageValuePercentage;
        _Interval = buffParams.interval;
        _MaxCount = buffParams.maxCount;
        _MaxDamageValue = buffParams.maxDamageValue;

        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        _TargetLimitHp = proCom.GetValueInt(EnEntityAttributeId.LimitHp);
    }
    protected override void ReaddKey(int addKey, EntityBurnBuffData buffParams)
    {
        if (buffParams.damageValuePercentage > _DamageValuePercentage)
            _DamageValuePercentage = buffParams.damageValuePercentage;
        if (buffParams.interval < _Interval)
            _Interval = buffParams.interval;
        if (buffParams.maxCount > _MaxCount)
            _MaxCount = buffParams.maxCount;
        if (buffParams.maxDamageValue > _MaxDamageValue)
            _MaxDamageValue = buffParams.maxDamageValue;

        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        var hp = proCom.GetValueInt(EnEntityAttributeId.LimitHp);
        if (hp != _TargetLimitHp)
            _TargetLimitHp = hp;
    }

    public void Update()
    {
        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_BuffEntityId, worldPos);

        var curTime = GameUtility.GetGameTime();
        if (_LastFireTime + _Interval < curTime)
        {
            _LastFireTime = curTime;
            var posX = Random.Range(-0.5f, 0.5f);
            var posY = Random.Range(-0.5f, 0.5f);
            var count = Mathf.Min(Count, _MaxCount);
            var damageValue = Mathf.Min(_MaxDamageValue, Mathf.CeilToInt(_TargetLimitHp * _DamageValuePercentage * count));
            GameAttackMgr.Instance.Attack(_SourceEntityID, _TargetEntityID, damageValue, worldPos + new Vector2(posX, posY), _DamageType);
        }
    }
}
