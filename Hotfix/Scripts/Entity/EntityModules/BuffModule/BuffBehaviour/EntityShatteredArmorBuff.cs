using System.Collections.Generic;
using UnityEngine;

public class EntityShatteredArmorBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public int maxCount;
    public float shatteredArmorPercentage;

    public override EnBuff GetBuff() => EnBuff.ShatteredArmor;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityShatteredArmorBuffData;


    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, shatteredArmorPercentage);
        ISerializeToIntArray.SerializeToInt(ref list, maxCount);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out shatteredArmorPercentage);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
    }

    public float GetTime()
    {
        return duration;
    }
}
public class EntityShatteredArmorBuff : EntityBuff<EntityShatteredArmorBuffData>, IUpdate
{
    private int _EffectEntityId = -1;
    private float _ProChangeValue;
    private int _MaxCount;
    private int _LastCount;
    public override void OnPoolDestroy()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        base.OnPoolDestroy();
        _ProChangeValue
            = _EffectEntityId
            = _MaxCount
            = _LastCount
            = -1;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_ShatteredArmor, in worldPos);
    }
    protected override void AddKey(int addKey, EntityShatteredArmorBuffData buffParams)
    {
        _ProChangeValue = buffParams.shatteredArmorPercentage;
        _MaxCount = buffParams.maxCount;

        _LastCount = Mathf.Min(_MaxCount, Count);
        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.RemoveAddtionalPercentage(EnEntityAttributeId.PhysicsArmor, buffParams.shatteredArmorPercentage * _LastCount);
        UpdateMgr.Instance.Registener(this);
    }
    protected override void ReaddKey(int addKey, EntityShatteredArmorBuffData buffParams)
    {
        if (buffParams.maxCount > _MaxCount)
            _MaxCount = buffParams.maxCount;

        var count = Mathf.Min(_MaxCount, Count);
        if (count != _LastCount)
        {
            var change = _ProChangeValue * (count - _LastCount);
            var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            proCom.RemoveAddtionalPercentage(EnEntityAttributeId.PhysicsArmor, change);
            _LastCount = count;
        }

        if (buffParams.shatteredArmorPercentage > _ProChangeValue)
        {
            var change = buffParams.shatteredArmorPercentage - _ProChangeValue;
            _ProChangeValue = buffParams.shatteredArmorPercentage;
            var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            proCom.RemoveAddtionalPercentage(EnEntityAttributeId.PhysicsArmor, change * _LastCount);
        }

    }
    public override void Clear()
    {
        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.AddAddtionalPercentage(EnEntityAttributeId.PhysicsArmor, _ProChangeValue * _LastCount);
        base.Clear();
    }

    public void Update()
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }
}
