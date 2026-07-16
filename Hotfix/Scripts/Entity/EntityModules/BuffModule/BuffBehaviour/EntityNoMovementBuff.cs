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

        //var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityPropertyComData>(_TargetEntityID, EnSceneEntityComType.Property);
        //proCom.SetIsCanMove(false);
    }

    protected override void ReaddKey(int addKey, EntityNoMovementBuffData buffParams)
    {

        //var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityPropertyComData>(_TargetEntityID, EnSceneEntityComType.Property);
        //proCom.SetIsCanMove(false);
    }

    public override void RemoveKey(int addKey)
    {
        //var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityPropertyComData>(_TargetEntityID, EnSceneEntityComType.Property);
        //proCom.SetIsCanMove(true);
        //base.OnDisable(addKey);
    }
}
