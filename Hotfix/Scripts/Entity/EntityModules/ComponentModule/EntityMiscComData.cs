using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EnMiscId
{
    None,
    ReboundSpeed,
    BallSpeed,
    BallSpeedPercentage,
    BallFireInterval,
    EnumCount,
}
public class EntityMiscComData : Entity3DComData
{
    public override EnSceneEntityComType GetComType() => EnSceneEntityComType.Misc;

    private readonly Dictionary<EnMiscId, float> _KeyValue = new((int)EnMiscId.EnumCount);

    public void SetValue(EnMiscId key, float value)
    {
        _KeyValue[key] = value;
    }
    public int GetIntValue(EnMiscId key)
    {
        if (!_KeyValue.TryGetValue(key, out var value))
            return 0;
        return (int)value;
    }
    public float GetValue(EnMiscId key)
    {
        if (!_KeyValue.TryGetValue(key, out var value))
            return 0;
        return value;
    }
}

