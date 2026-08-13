using System.Collections.Generic;
using UnityEngine;


public class EntityElectricBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public int maxCount;
    public float reducePercentage01;
    public EnEntityAttributeId att1;
    public EnEntityAttributeId att2;

    public override EnBuff GetBuff() => EnBuff.Electric;
    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityElectricBuffData;


    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, in duration);
        ISerializeToIntArray.SerializeToInt(ref list, in maxCount);
        ISerializeToIntArray.SerializeToInt(ref list, in reducePercentage01);
        ISerializeToIntArray.SerializeToInt(ref list, att1);
        ISerializeToIntArray.SerializeToInt(ref list, att2);
        return list;
    }

    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out reducePercentage01);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out att1);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out att2);
    }

    public float GetTime()
    {
        return duration;
    }

    public override void GetDescriptionList(ref List<DescriptionInfo> descInfos, EnDescriptionLevel level)
    {
        base.GetDescriptionList(ref descInfos, level);

        var att1Name = LanguageUtility.GetLanguage_AttiributeName(att1);
        var att2Name = LanguageUtility.GetLanguage_AttiributeName(att2);
        var info = ClassPoolMgr.Instance.Pull<DescriptionInfo>();
        descInfos.Add(info);
        info.title = LanguageUtility.GetLanguage_BuffStyleName(GetBuff());
        info.level = level;
        info.content = string.Format(LanguageUtility.GetLanguage_BuffDescription(GetBuff()),
            reducePercentage01 * 100, att1Name, att2Name, duration, maxCount);
    }
}

public class EntityElectricBuff : EntityBuff<EntityElectricBuffData>, IUpdate
{
    private readonly Dictionary<EnEntityAttributeId, float> _AttInfos = new();

    private int _MaxCount = -1;
    private int _LastCount;
    private int _EffectEntityId;
    public float _ReducePercentage01;
    public EnEntityAttributeId _Att1;
    public EnEntityAttributeId _Att2;
    public override void Clear()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        base.Clear();
        _AttInfos.Clear();
        _ReducePercentage01
            = _MaxCount
            = _LastCount
            = _EffectEntityId
            = -1;
        _Att1
            = _Att2
            = EnEntityAttributeId.None;
    }

    public void Update()
    {
        var pointInfo = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(in _TargetEntityID);
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, pointInfo.GetCenterPointWorldPos());
    }

    protected override void AddKey(int addKey, EntityElectricBuffData buffParams)
    {
        _MaxCount = buffParams.maxCount;
        _LastCount = Mathf.Min(Count, _MaxCount);
        _ReducePercentage01 = buffParams.reducePercentage01;
        _Att1 = buffParams.att1;
        _Att2 = buffParams.att2;

        UpdateAttInfos(_Att1, _ReducePercentage01, _LastCount);
        UpdateAttInfos(_Att2, _ReducePercentage01, _LastCount);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(in _TargetEntityID);
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_Electric, entityData.GetCenterPointWorldPos(), entityData.GetVolume());
        UpdateMgr.Instance.Registener(this);
    }

    private void UpdateAttInfos(EnEntityAttributeId attId, float percentage01, int count)
    {
        if (_AttInfos.TryGetValue(attId, out var lastPercentage01))
        {
            if (lastPercentage01 > percentage01)
                return;
            var change = percentage01 - lastPercentage01;

            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, attId, change * count);
            _AttInfos[attId] = percentage01;
        }
        else
        {
            _AttInfos.Add(attId, percentage01);
            GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, attId, percentage01 * count);
        }
    }

    protected override void ReaddKey(int addKey, EntityElectricBuffData buffParams)
    {
        if (buffParams.maxCount != _MaxCount)
            _MaxCount = buffParams.maxCount;

        UpdateAttInfos(buffParams.att1, buffParams.reducePercentage01, _LastCount);
        UpdateAttInfos(buffParams.att2, buffParams.reducePercentage01, _LastCount);

        var count = Mathf.Min(Count, _MaxCount);
        if (count != _LastCount)
        {
            var addCount = count - _LastCount;
            foreach (var (attId, reduceValue) in _AttInfos)
            {
                GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, attId, reduceValue * addCount);
            }

            _LastCount = count;
        }
    }
}
