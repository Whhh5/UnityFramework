using System.Collections.Generic;
using UnityEngine;

public class EntityCrystalBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public float speedReducePercentage01;
    public int maxCount;
    public EnEntityAttributeId attId;

    public override EnBuff GetBuff() => EnBuff.Crystal;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityCrystalBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, speedReducePercentage01);
        ISerializeToIntArray.SerializeToInt(ref list, maxCount);
        ISerializeToIntArray.SerializeToInt(ref list, attId);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out speedReducePercentage01);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out attId);
    }

    public float GetTime()
    {
        return duration;
    }

    public override void GetDescriptionList(ref List<DescriptionInfo> descInfos, EnDescriptionLevel level)
    {
        base.GetDescriptionList(ref descInfos, level);

        var attName = LanguageUtility.GetLanguage_AttiributeName(attId);
        var info = ClassPoolMgr.Instance.Pull<DescriptionInfo>();
        descInfos.Add(info);
        info.title = LanguageUtility.GetLanguage_BuffStyleName(GetBuff());
        info.level = level;
        info.content = string.Format(LanguageUtility.GetLanguage_BuffDescription(GetBuff()),
            speedReducePercentage01 * 100, attName, duration, maxCount);
    }
}
public class EntityCrystalBuff : EntityBuff<EntityCrystalBuffData>, IUpdate
{
    private int _EffectEntityId = -1;
    private float _ProChangeValue;
    private int _MaxCount;
    private int _LastCount;
    private EnEntityAttributeId _AttId;
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
        _AttId = EnEntityAttributeId.None;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_ice, in worldPos, entityData.GetVolume());
    }
    protected override void AddKey(int addKey, EntityCrystalBuffData buffParams)
    {
        _ProChangeValue = buffParams.speedReducePercentage01;
        _MaxCount = buffParams.maxCount;
        _AttId = buffParams.attId;

        _LastCount = Mathf.Min(_MaxCount, Count);
        GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, buffParams.speedReducePercentage01 * _LastCount);
        UpdateMgr.Instance.Registener(this);
    }
    protected override void ReaddKey(int addKey, EntityCrystalBuffData buffParams)
    {
        if (buffParams.maxCount > _MaxCount)
            _MaxCount = buffParams.maxCount;

        if (buffParams.speedReducePercentage01 > _ProChangeValue)
        {
            var change = buffParams.speedReducePercentage01 - _ProChangeValue;
            _ProChangeValue = buffParams.speedReducePercentage01;
            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, change * _LastCount);
        }

        var count = Mathf.Min(_MaxCount, Count);
        if (_LastCount != count)
        {
            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, _ProChangeValue * (count - _LastCount));
            _LastCount = count;
        }

    }
    public override void Clear()
    {
        GameAttributeUtility.AddAddtionalPercentage(_TargetEntityID, _AttId, _ProChangeValue * _LastCount);
        base.Clear();
    }

    public void Update()
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }
}
