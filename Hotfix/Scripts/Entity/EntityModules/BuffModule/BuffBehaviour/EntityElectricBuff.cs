using System.Collections.Generic;
using UnityEngine;


public class EntityElectricBuffAttributeReduceData : ISerializeToIntArray<ISerializeToIntArrayUserData>
{
    public EnTypeId GetTypeDefineId() => EnTypeId.EntityElectricBuffAttributeReduceData;

    public EnEntityAttributeId attributeId;
    public float reducePercentage01;
    public void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out attributeId);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out reducePercentage01);
    }

    public List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, attributeId);
        ISerializeToIntArray.SerializeToInt(ref list, in reducePercentage01);
        return list;
    }


    public void OnPoolDestroy()
    {
        
    }

    public void OnPoolInit(ISerializeToIntArrayUserData userData)
    {
        DeserializeFromIntArray(userData.arrParams, userData.startIndex, userData.count, userData);
    }
}
public class EntityElectricBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public int maxCount;
    public List<EntityElectricBuffAttributeReduceData> reduceAttributeList;

    public override EnBuff GetBuff() => EnBuff.Electric;
    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityElectricBuffData;

    public override void OnPoolDestroy()
    {
        if (reduceAttributeList != null)
        {
            foreach (var item in reduceAttributeList)
            {
                ClassPoolMgr.Instance.Push(item);
            }
        }
        base.OnPoolDestroy();
        reduceAttributeList = null;
    }
    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, in duration);
        ISerializeToIntArray.SerializeToInt(ref list, in maxCount);
        ISerializeToIntArray.SerializeToInt(ref list, reduceAttributeList);
        return list;
    }

    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out reduceAttributeList, userData);
    }

    public float GetTime()
    {
        return duration;
    }
}

public class EntityElectricBuff : EntityBuff<EntityElectricBuffData>, IUpdate
{
    private readonly Dictionary<EnEntityAttributeId, float> _AttInfos = new();

    private int _MaxCount = -1;
    private int _LastCount;
    private int _EffectEntityId;
    public override void Clear()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        base.Clear();
        _AttInfos.Clear();
        _MaxCount
            = _LastCount
            = _EffectEntityId
            = -1;
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
        for (int i = 0; i < buffParams.reduceAttributeList.Count; i++)
        {
            var data = buffParams.reduceAttributeList[i];
            _AttInfos.Add(data.attributeId, data.reducePercentage01);

            var attCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            attCom.RemoveAddtionalPercentage(data.attributeId, data.reducePercentage01 * _LastCount);
        }

        var pointInfo = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(in _TargetEntityID);
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_Electric, pointInfo.GetCenterPointWorldPos());
        UpdateMgr.Instance.Registener(this);
    }

    protected override void ReaddKey(int addKey, EntityElectricBuffData buffParams)
    {
        if (buffParams.maxCount != _MaxCount)
            _MaxCount = buffParams.maxCount;

        for (int i = 0; i < buffParams.reduceAttributeList.Count; i++)
        {
            var data = buffParams.reduceAttributeList[i];
            if (_AttInfos.TryGetValue(data.attributeId, out var value))
            {
                if (value > data.reducePercentage01)
                    continue;
                var change = data.reducePercentage01 - value;
                var attCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
                attCom.RemoveAddtionalPercentage(data.attributeId, change * _LastCount);
                _AttInfos[data.attributeId] = data.reducePercentage01;
            }
            else
            {
                _AttInfos.Add(data.attributeId, data.reducePercentage01);
                var attCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
                attCom.RemoveAddtionalPercentage(data.attributeId, data.reducePercentage01 * _LastCount);
            }
        }

        var count = Mathf.Min(Count, _MaxCount);
        if (count != _LastCount)
        {
            var addCount = count - _LastCount;
            foreach (var (attId, reduceValue) in _AttInfos)
            {
                var attCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
                attCom.RemoveAddtionalPercentage(attId, reduceValue * addCount);
            }

            _LastCount = count;
        }
    }
}
