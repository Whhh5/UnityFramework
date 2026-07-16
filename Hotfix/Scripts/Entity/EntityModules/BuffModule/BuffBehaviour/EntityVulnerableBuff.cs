using System.Collections.Generic;
using UnityEngine;

public enum EnDamageType
{
    None,
    Physic = 1 << 0,
    Magic = 1 << 1,
    True = 1 << 2,
}
public enum EnDamageValueType
{
    None,
    Normal,
    Critical, // 暴击
}
public class EntityVulnerableBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public EnDamageType damageType;
    public float donusDamagePercentage;
    public EnDamageType donusDamageType;
    public int maxCount;

    public override EnBuff GetBuff() => EnBuff.Vulnerable;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityVulnerableBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, damageType);
        ISerializeToIntArray.SerializeToInt(ref list, donusDamagePercentage);
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, donusDamageType);
        ISerializeToIntArray.SerializeToInt(ref list, maxCount);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out damageType);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out donusDamagePercentage);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out donusDamageType);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
    }

    public float GetTime()
    {
        return duration;
    }
}

public class EntityVulnerableBuff : EntityBuff<EntityVulnerableBuffData>, IUpdate, IGameDamageCallback
{
    private int _EffectEntityId = -1;
    private EnDamageType _DamageType;
    private EnDamageType _DonusDamageType;
    private float _DonusDamagePercentage; // 0-100
    private int _MaxCount;

    public override void OnPoolDestroy()
    {
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        UpdateMgr.Instance.Unregistener(this);
        base.OnPoolDestroy();
        _EffectEntityId = -1;
        _DamageType = EnDamageType.None;
        _DonusDamagePercentage = -1;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        UpdateMgr.Instance.Registener(this);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_fangxue, worldPos);
    }
    public override void Clear()
    {
        GameAttackMgr.Instance.UnregisterDamageTargetCallback(_TargetEntityID, _DamageType, this);
        base.Clear();
    }
    protected override void AddKey(int addKey, EntityVulnerableBuffData buffParams)
    {
        _DamageType = buffParams.damageType;
        _DonusDamagePercentage = buffParams.donusDamagePercentage;
        _DonusDamageType = buffParams.donusDamageType;
        _MaxCount = buffParams.maxCount;

        GameAttackMgr.Instance.RegisterDamageTargetCallback(_TargetEntityID, _DamageType, this);
    }
    protected override void ReaddKey(int addKey, EntityVulnerableBuffData buffParams)
    {
        if (buffParams.donusDamagePercentage > _DonusDamagePercentage)
        {
            _DonusDamagePercentage = buffParams.donusDamagePercentage;
        }
        if (buffParams.maxCount > _MaxCount)
        {
            _MaxCount = buffParams.maxCount;
        }
    }

    public void Update()
    {
        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }

    public void DamageCallback(int sourceEntityId, int targetEntityId, int damageValue, EnDamageType damageType, EnDamageValueType valueType, Vector2 worldPos)
    {
        var donusDamage = _DonusDamagePercentage * Mathf.Min(_MaxCount, Count);
        var value = Mathf.CeilToInt(damageValue * donusDamage);
        var finalDamage = GameAttackMgr.Instance.Attack(_SourceEntityID, targetEntityId, value, worldPos, _DonusDamageType);

        var eventUd = ClassPoolMgr.Instance.Pull<ValnerableBuffTriggerEventData>();
        eventUd.finalDamage = finalDamage;
        eventUd.sourceEntityId = sourceEntityId;
        eventUd.targetEntityId = targetEntityId;
        eventUd.point = worldPos;
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_BUFF_TRIGGER, (int)_Buff, 0, eventUd);
        ClassPoolMgr.Instance.Push(eventUd);
    }
}

public class ValnerableBuffTriggerEventData : IClassPoolDestroy
{
    public int finalDamage;
    public int sourceEntityId;
    public int targetEntityId;
    public Vector2 point;
    public void OnPoolDestroy()
    {
        finalDamage = -1;
    }
}