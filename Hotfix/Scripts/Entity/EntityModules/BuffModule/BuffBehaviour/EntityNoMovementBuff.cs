using System.Collections.Generic;
using UnityEngine;


public class EntityNoMovementBuffData : IEntityBuffData
{
    public void OnPoolDestroy()
    {
    }

    public void OnPoolInit(CommonSerializeToIntArrayUserData userData)
    {

    }

    public EnBuff GetBuff() => EnBuff.NoMovement;
    public EnTypeId GetTypeDefineId() => EnTypeId.EntityNoMovementBuffData;

    public List<int> SerializeToIntArray()
    {
        return null;
    }

    public void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {

    }

}

public class EntityNoMovementBuff : EntityBuff<EntityNoMovementBuffData>
{

    protected override void AddKey(int addKey, EntityNoMovementBuffData buffParams)
    {
        GameAttributeUtility.RemoveAddtionalPercentage(_TargetEntityID, EnEntityAttributeId.MoveSpeed, 1);
    }

    public override void Clear()
    {
        GameAttributeUtility.AddAddtionalPercentage(_TargetEntityID, EnEntityAttributeId.MoveSpeed, 1);
        base.Clear();
    }

    protected override void ReaddKey(int addKey, EntityNoMovementBuffData buffParams)
    {
        
    }
}
