using System.Collections.Generic;
using UnityEngine;

public enum EnEntityAttributeId
{
    None,
    Hp,
    LimitHp,
    Attack,
    PhysicsArmor,
    MagicArmor,
    Critical,
    CriticalDamage,
    MoveSpeed,
    MagicCritical,
    EnumCount,
}
public class EntityAttributeData : IClassPoolDestroy
{
    private float _BaseValue;
    private float _AddtionalValue;
    private float _AddtionalPercentage;
    private float _Result;
    private float _ResultChange;

    public void OnPoolDestroy()
    {
        _BaseValue
            = _AddtionalValue
            = _AddtionalPercentage
            = _Result
            = _ResultChange
            = 0f;
    }
    private float CalculateResult()
    {
        var value = Mathf.Max(0, 1 + _AddtionalPercentage) * Mathf.Max(0, _BaseValue + _AddtionalValue);
        return Mathf.Max(0, value + _ResultChange);
    }
    public void SetBaseValue(in float baseValue)
    {
        this._BaseValue = baseValue;
        _Result = CalculateResult();
    }
    public ref readonly float GetValue()
    {
        return ref _Result;
    }
    public void AddAddtionalValue(in float addtionalValue)
    {
        this._AddtionalValue += addtionalValue;
        _Result = CalculateResult();
    }
    public void RemoveAddtionalValue(in float addtionalValue)
    {
        this._AddtionalValue -= addtionalValue;
        _Result = CalculateResult();
    }
    public void AddAddtionalPercentage(in float addtionalPercentage)
    {
        this._AddtionalPercentage += addtionalPercentage;
        _Result = CalculateResult();
    }
    public void RemoveAddtionalPercentage(in float addtionalPercentage)
    {
        this._AddtionalPercentage -= addtionalPercentage;
        _Result = CalculateResult();
    }
    public void AddResultChange(in float ResultReduce)
    {
        this._ResultChange += ResultReduce;
        _Result = CalculateResult();
    }
    public void RemoveResultChange(in float ResultReduce)
    {
        this._ResultChange -= ResultReduce;
        _Result = CalculateResult();
    }
}
public class SceneEntityAttributeComData : Entity3DComData
{
    public override EnSceneEntityComType GetComType() => EnSceneEntityComType.Attribute;

    private readonly Dictionary<EnEntityAttributeId, EntityAttributeData> _PropertyDatas = new((int)EnEntityAttributeId.EnumCount);

    public override void OnPoolDestroy()
    {
        foreach (var (_, data) in _PropertyDatas)
        {
            ClassPoolMgr.Instance.Push(data);
        }
        base.OnPoolDestroy();
        _PropertyDatas.Clear();
    }

    private EntityAttributeData GetPropertyData(EnEntityAttributeId proId)
    {
        if (!_PropertyDatas.TryGetValue(proId, out var data))
        {
            data = ClassPoolMgr.Instance.Pull<EntityAttributeData>();
            _PropertyDatas.Add(proId, data);
        }
        return data;
    }
    public float GetValue(EnEntityAttributeId proId)
    {
        if (!_PropertyDatas.TryGetValue(proId, out var data))
            return 0;
        var value = data.GetValue();
        return value;
    }
    public int GetValueInt(EnEntityAttributeId proId)
    {
        var value = GetValue(proId);
        return Mathf.CeilToInt(value);
    }
    public void SetBaseValue(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.SetBaseValue(value);
    }
    public void AddAddtionalValue(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.AddAddtionalValue(value);
    }
    public void RemoveAddtionalValue(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.RemoveAddtionalValue(value);
    }
    public void AddAddtionalPercentage(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.AddAddtionalPercentage(value);
    }
    public void RemoveAddtionalPercentage(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.RemoveAddtionalPercentage(value);
    }
    public void AddResultValue(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.AddResultChange(value);
    }
    public void RemoveResultValue(EnEntityAttributeId proId, float value)
    {
        var data = GetPropertyData(proId);
        data.RemoveResultChange(value);
    }
}