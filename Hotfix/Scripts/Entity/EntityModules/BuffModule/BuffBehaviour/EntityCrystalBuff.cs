using System.Collections.Generic;
using UnityEngine;

public class EntityCrystalBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public float speedReducePercentage;
    public int maxCount;

    public override EnBuff GetBuff() => EnBuff.Crystal;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityCrystalBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, speedReducePercentage);
        ISerializeToIntArray.SerializeToInt(ref list, maxCount);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out speedReducePercentage);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
    }

    public float GetTime()
    {
        return duration;
    }
}
public class EntityCrystalBuff : EntityBuff<EntityCrystalBuffData>, IUpdate
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
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_ice, in worldPos);
    }
    protected override void AddKey(int addKey, EntityCrystalBuffData buffParams)
    {
        _ProChangeValue = buffParams.speedReducePercentage;
        _MaxCount = buffParams.maxCount;

        _LastCount = Mathf.Min(_MaxCount, Count);
        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.RemoveAddtionalPercentage(EnEntityAttributeId.MoveSpeed, buffParams.speedReducePercentage * _LastCount);
        UpdateMgr.Instance.Registener(this);
    }
    protected override void ReaddKey(int addKey, EntityCrystalBuffData buffParams)
    {
        if (buffParams.maxCount > _MaxCount)
            _MaxCount = buffParams.maxCount;

        if (buffParams.speedReducePercentage > _ProChangeValue)
        {
            var change = buffParams.speedReducePercentage - _ProChangeValue;
            _ProChangeValue = buffParams.speedReducePercentage;
            var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            proCom.RemoveAddtionalPercentage(EnEntityAttributeId.MoveSpeed, change * _LastCount);
        }

        var count = Mathf.Min(_MaxCount, Count);
        if (_LastCount != count)
        {
            var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            proCom.RemoveAddtionalPercentage(EnEntityAttributeId.MoveSpeed, _ProChangeValue * (count - _LastCount));
            _LastCount = count;
        }

    }
    public override void Clear()
    {
        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.AddAddtionalPercentage(EnEntityAttributeId.MoveSpeed, _ProChangeValue * _LastCount);
        base.Clear();
    }

    public void Update()
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }
}
