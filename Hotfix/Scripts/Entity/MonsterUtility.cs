

using UnityEngine;

public static class MonsterUtility
{
    public static MonsterCfg GetMonsterCfg(in int entityId)
    {
        var monsterEntityData = EntityMgr.Instance.GetEntityData<SceneMonsterEntityData>(in entityId);
        ref readonly var monsterId = ref monsterEntityData.GetMonsterCfgId();
        var monsterCfg = GameSchedule.Instance.GetMonsterCfg0(monsterId);
        return monsterCfg;
    }

    public static bool EntityIsValid(in int entityId, in EnSceneEntityStatus checkRules, in EnSceneEntityStatus ignoreRules)
    {
        if (!EntityMgr.Instance.IsValid(entityId))
            return false;
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