using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBleedBuffData : EntityBuffData, IBuffTimeInfo
{
    public float duration;
    public EnDamageType damageType;
    public float damageValuePercentage01;
    public int maxDamageValue;
    public float interval;
    public int maxCount;
    public EnEntityAttributeId targetAttribute;

    public override EnBuff GetBuff() => EnBuff.Bleed;

    public override EnTypeId GetTypeDefineId() => EnTypeId.EntityBleedBuffData;

    public override List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, damageType);
        ISerializeToIntArray.SerializeToInt(ref list, in duration);
        ISerializeToIntArray.SerializeToInt(ref list, in damageValuePercentage01);
        ISerializeToIntArray.SerializeToInt(ref list, in maxDamageValue);
        ISerializeToIntArray.SerializeToInt(ref list, in interval);
        ISerializeToIntArray.SerializeToInt(ref list, in maxCount);
        ISerializeToIntArray.SerializeToInt(ref list, targetAttribute);
        return list;
    }
    public override void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out damageType);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out duration);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out damageValuePercentage01);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxDamageValue);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out interval);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out maxCount);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out targetAttribute);
    }

    public float GetTime()
    {
        return duration;
    }

    public override void GetDescriptionList(ref List<DescriptionInfo> descInfos, EnDescriptionLevel level)
    {
        base.GetDescriptionList(ref descInfos, level);

        var attName = LanguageUtility.GetLanguage_AttiributeName(targetAttribute);
        var damageName = LanguageUtility.GetLanguage_DamageTypeName(damageType);

        var info = ClassPoolMgr.Instance.Pull<DescriptionInfo>();
        info.title = LanguageUtility.GetLanguage_BuffStyleName(GetBuff());
        info.level = level;
        info.content = string.Format(LanguageUtility.GetLanguage_BuffDescription(GetBuff()),
            interval, damageValuePercentage01 * 100, attName, maxDamageValue, damageName, duration, maxCount);
        descInfos.Add(info);
    }
}
public class EntityBleedBuff : EntityBuff<EntityBleedBuffData>, IUpdate
{
    private EnDamageType _DamageType;
    private float _DamageValuePercentage;
    private int _MaxDamageValue;
    private float _Interval;
    private int _MaxCount;

    private int _BuffEntityId;
    private float _LastFireTime;
    private EnEntityAttributeId _TargetAttribute;
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
            = -1;
        _TargetAttribute = EnEntityAttributeId.None;
    }
    public override void OnPoolInit(EntityBuffDataUserData userData)
    {
        base.OnPoolInit(userData);

        var entityData = EntityMgr.Instance.GetEntityData<GameBattleMonsterEntityData>(_TargetEntityID);
        var worldPos = entityData.GetCenterPointWorldPos();
        _BuffEntityId = GameEffectMgr.Instance.PlayEffect(EnGameEffectId.Eff_buff_Bleed, in worldPos, entityData.GetVolume());

        UpdateMgr.Instance.Registener(this);
    }
    protected override void AddKey(int addKey, EntityBleedBuffData buffParams)
    {
        _DamageType = buffParams.damageType;
        _DamageValuePercentage = buffParams.damageValuePercentage01;
        _Interval = buffParams.interval;
        _MaxCount = buffParams.maxCount;
        _MaxDamageValue = buffParams.maxDamageValue;
        _TargetAttribute = buffParams.targetAttribute;
    }
    protected override void ReaddKey(int addKey, EntityBleedBuffData buffParams)
    {
        if (buffParams.damageValuePercentage01 > _DamageValuePercentage)
            _DamageValuePercentage = buffParams.damageValuePercentage01;
        if (buffParams.interval < _Interval)
            _Interval = buffParams.interval;
        if (buffParams.maxCount > _MaxCount)
            _MaxCount = buffParams.maxCount;
        if (buffParams.maxDamageValue > _MaxDamageValue)
            _MaxDamageValue = buffParams.maxDamageValue;
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
            var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(_TargetEntityID, EnSceneEntityComType.Attribute);
            var hp = proCom.GetValueInt(_TargetAttribute);
            var damageValue = Mathf.Min(_MaxDamageValue, Mathf.CeilToInt(hp * _DamageValuePercentage * count));
            GameAttackMgr.Instance.AttackMonster(_SourceEntityID, _TargetEntityID, damageValue, worldPos + new Vector2(posX, posY), _DamageType);
        }
    }
}