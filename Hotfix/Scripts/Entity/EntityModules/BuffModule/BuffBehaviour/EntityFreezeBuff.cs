using System.Collections.Generic;
using UnityEngine;

public class EntityFreezeBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public float speedReducePercentage01;
    public EnEntityAttributeId attId;

    public override EnBuff GetBuff() => EnBuff.Freeze;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityFreezeBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, in duration);
        ISerializeToIntArray.SerializeToInt(ref list, in speedReducePercentage01);
        ISerializeToIntArray.SerializeToInt(ref list, attId);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out speedReducePercentage01);
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
            speedReducePercentage01 * 100, attName, duration);
    }
}
public class EntityFreezeBuff : EntityBuff<EntityFreezeBuffData>, IUpdate
{
    private int _EffectEntityId = -1;
    private float _ProChangeValue;
    private EnEntityAttributeId _AttId;
    public override void OnPoolDestroy()
    {
        UpdateMgr.Instance.Unregistener(this);
        GameEffectMgr.Instance.DestroyEffect(_EffectEntityId);
        base.OnPoolDestroy();
        _ProChangeValue
            = _EffectEntityId
            = -1;
        _AttId = EnEntityAttributeId.None;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var monsterEntityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        var volume = monsterEntityData.GetVolume();
        _EffectEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_Freeze, in worldPos, volume);
    }
    protected override void AddKey(int addKey, EntityFreezeBuffData buffParams)
    {
        _ProChangeValue = buffParams.speedReducePercentage01;
        _AttId = buffParams.attId;

        GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, _AttId, _ProChangeValue);

        //GameMonsterUtility.EnterStatus(_TargetEntityID, EnSceneEntityStatus.Freeze, null);
        UpdateMgr.Instance.Registener(this);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        entityData.AddMonsterStatus(EnSceneEntityStatus.Freeze);

        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(entityData.GetMonsterCfgId());
        if (monsterCfg.nCmdFreeze > 0)
            SceneEntityMgr.Instance.AddCmd(_TargetEntityID, monsterCfg.nCmdFreeze);
    }
    protected override void ReaddKey(int addKey, EntityFreezeBuffData buffParams)
    {

    }
    public override void Clear()
    {
        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        entityData.RemoveMonsterStatus(EnSceneEntityStatus.Freeze);

        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(entityData.GetMonsterCfgId());
        if (monsterCfg.nCmdFreeze > 0)
            SceneEntityMgr.Instance.RemoveCmd(_TargetEntityID, monsterCfg.nCmdFreeze);
        //GameMonsterUtility.ExitStatus(_TargetEntityID, EnSceneEntityStatus.Freeze, EnGameSpineCompleteStatus.Normal);
        GameAttributeUtility.AddAddtionalPercentage(_TargetEntityID, _AttId, _ProChangeValue);
        base.Clear();
    }

    public void Update()
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<IEntityGridPointInfo>(_TargetEntityID);
        var worldPos = monsterEntityData.GetCenterPointWorldPos();
        EntityMgr.Instance.SetEntityWorldPos(_EffectEntityId, worldPos);
    }
}
