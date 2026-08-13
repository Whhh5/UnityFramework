using System.Collections.Generic;
using UnityEngine;

public class EntityShatteredArmorBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public int maxCount;
    public float shatteredArmorPercentage01;
    public EnEntityAttributeId attId;

    public override EnBuff GetBuff() => EnBuff.ShatteredArmor;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityShatteredArmorBuffData;


    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, shatteredArmorPercentage01);
        ISerializeToIntArray.SerializeToInt(ref list, maxCount);
        ISerializeToIntArray.SerializeToInt(ref list, attId);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out shatteredArmorPercentage01);
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
            shatteredArmorPercentage01 * 100, attName, duration, maxCount);
    }
}
public class EntityShatteredArmorBuff : EntityBuff<EntityShatteredArmorBuffData>, IUpdate
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
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_ShatteredArmor, in worldPos, entityData.GetVolume());
    }
    protected override void AddKey(int addKey, EntityShatteredArmorBuffData buffParams)
    {
        _ProChangeValue = buffParams.shatteredArmorPercentage01;
        _MaxCount = buffParams.maxCount;
        _AttId = buffParams.attId;

        _LastCount = Mathf.Min(_MaxCount, Count);

        GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, buffParams.shatteredArmorPercentage01 * _LastCount);
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

            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, change);
            _LastCount = count;
        }

        if (buffParams.shatteredArmorPercentage01 > _ProChangeValue)
        {
            var change = buffParams.shatteredArmorPercentage01 - _ProChangeValue;
            _ProChangeValue = buffParams.shatteredArmorPercentage01;

            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, change * _LastCount);
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
