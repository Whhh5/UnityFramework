

using UnityEngine;

public static class MonsterUtility
{
    public static int CreateMonster(in int monsterId)
    {
        var userData = ClassPoolMgr.Instance.Pull<SceneMonsterEntityUserData>();
        var entityId = CreateMonster(in monsterId, userData);

        ClassPoolMgr.Instance.Push(userData);
        return entityId;
    }
    public static int CreateMonster(in int monsterId, SceneMonsterEntityUserData userData)
    {
        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(monsterId);
        userData.monsterId = monsterId;
        var entityId = SceneEntityMgr.Instance.CreateEntityData(in monsterCfg.nEntityTypeId, userData);

        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Camera);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Rigidbody);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Animation);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Attribute);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Buff);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.AI);
        SceneEntityMgr.Instance.AddCom(in entityId, EnSceneEntityComType.Cmd);

        if (monsterCfg.nIdleCmdID > 0)
        {
            SceneEntityMgr.Instance.AddCmd(in entityId, in monsterCfg.nIdleCmdID);
        }

        var proCom = SceneEntityMgr.Instance.GetEntityCom<SceneEntityAttributeComData>(in entityId, EnSceneEntityComType.Attribute);
        proCom.SetBaseValue(EnEntityAttributeId.MoveSpeed, 3);
        //proCom.SetBaseValue(EnEntityPropertyId.RotationSpeed, 5);

        return entityId;
    }

    public static void DestroyMonster(in int monsterId)
    {
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Camera);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Rigidbody);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Animation);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Attribute);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Buff);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.AI);
        SceneEntityMgr.Instance.RemoveCom(in monsterId, EnSceneEntityComType.Cmd);
        SceneEntityMgr.Instance.DestroyEntityData(monsterId);
    }

    public static MonsterCfg GetMonsterCfg(in int entityId)
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);
        ref readonly var monsterId = ref monsterEntityData.GetMonsterCfgId();
        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(monsterId);
        return monsterCfg;
    }

    public static bool EntityIsValid(in int entityId, in EnSceneEntityStatus checkRules, in EnSceneEntityStatus ignoreRules)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);

        ref readonly var entityState = ref entityData.GetSceneEntityStatus();
        var checkResult = checkRules & entityState & ~ignoreRules;
        if (checkResult != EnSceneEntityStatus.None)
            return false;
        return true;
    }
    public static bool EntityIsValid(in int entityId)
    {
        var result = EntityIsValid(entityId, EnSceneEntityStatus.Die, EnSceneEntityStatus.None);
        return result;
    }
    public static ref readonly EnGameLayerInt GetEntityEnemyLayer(in int entityId)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);
        ref readonly var enemyLayer = ref entityData.GetEnemyLayer();
        return ref enemyLayer;
    }
    public static ref readonly EnGameLayerInt GetEntityFriendLayer(in int entityId)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);
        ref readonly var friendLayer = ref entityData.GetFriendLayer();
        return ref friendLayer;
    }
    public static ref readonly EnGameLayerInt GetEntityLayer(in int entityId)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);
        ref readonly var layer = ref entityData.GetLayer();
        return ref layer;
    }

}