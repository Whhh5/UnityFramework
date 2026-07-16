using System.Collections.Generic;
using UnityEngine;

public class EntityFreezeBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public float speedReducePercentage;

    public override EnBuff GetBuff() => EnBuff.Freeze;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityFreezeBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, duration);
        ISerializeToIntArray.SerializeToInt(ref list, speedReducePercentage);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out speedReducePercentage);
    }

    public float GetTime()
    {
        return duration;
    }
}
public class EntityFreezeBuff : EntityBuff<EntityFreezeBuffData>, IUpdate
{
    private int _EffectEntityId = -1;
    private float _ProChangeValue;
    public override void OnPoolDestroy()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        base.OnPoolDestroy();
        _ProChangeValue
            = _EffectEntityId
            = -1;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_Freeze, in worldPos);
    }
    protected override void AddKey(int addKey, EntityFreezeBuffData buffParams)
    {
        _ProChangeValue = buffParams.speedReducePercentage;

        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.RemoveAddtionalPercentage(EnEntityAttributeId.MoveSpeed, _ProChangeValue);

        GameMonsterUtility.EnterStatus(_TargetEntityID, EnSceneEntityStatus.Freeze, null);
        UpdateMgr.Instance.Registener(this);
    }
    protected override void ReaddKey(int addKey, EntityFreezeBuffData buffParams)
    {

    }
    public override void Clear()
    {
        GameMonsterUtility.ExitStatus(_TargetEntityID, EnSceneEntityStatus.Freeze, EnGameSpineCompleteStatus.Normal);
        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
        proCom.AddAddtionalPercentage(EnEntityAttributeId.MoveSpeed, _ProChangeValue);
        base.Clear();
    }

    public void Update()
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }
}
