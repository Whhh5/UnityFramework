using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntityMgr : Singleton<SceneEntityMgr>
{
    private readonly Dictionary<int, SceneEntityData> _SceneEntityDatas = new();


    private SceneEntityData GetSceneEntityData(in int entityId)
    {
        return _SceneEntityDatas[entityId];
    }

    public int CreateEntityData(in int typeId)
    {
        var userData = ClassPoolMgr.Instance.Pull<GameEntityUserData>();
        var sceneEntityId = CreateEntityData(in typeId, in userData);
        ClassPoolMgr.Instance.Push(in userData);
        return sceneEntityId;
    }
    public int CreateEntityData<T>()
        where T : GameEntityData, new()
    {
        var userData = ClassPoolMgr.Instance.Pull<GameEntityUserData>();
        var entityID = CreateEntityData<T>(userData);
        ClassPoolMgr.Instance.Push(in userData);
        return entityID;
    }
    public int CreateEntityData<T>(GameEntityUserData userData)
        where T : GameEntityData, new()
    {
        var entityID = EntityMgr.Instance.CreateEntityData<T>(userData);
        var sceneEntity = EntityMgr.Instance.GetEntityData<SceneEntityData>(in entityID);

        _SceneEntityDatas.Add(entityID, sceneEntity);
        return entityID;
    }
    public int CreateEntityData(in int typeId, in GameEntityUserData userData)
    {
        var sceneEntityId = EntityMgr.Instance.CreateEntityData(in typeId, userData);
        var sceneEntity = EntityMgr.Instance.GetEntityData<SceneEntityData>(in sceneEntityId);

        _SceneEntityDatas.Add(sceneEntityId, sceneEntity);
        return sceneEntityId;
    }
    public void DestroyEntityData(in int entityId)
    {
        if (!_SceneEntityDatas.ContainsKey(entityId))
            return;
        _SceneEntityDatas.Remove(entityId);
        EntityMgr.Instance.RecycleEntityData(in entityId);

    }

    #region Com
    public void AddCom(in int entityId, in EnSceneEntityComType comType)
    {
        var entityData = GetSceneEntityData(in entityId);
        entityData.AddEntityCom(comType);
    }
    public void RemoveCom(in int entityId, in EnSceneEntityComType comType)
    {
        var entityData = GetSceneEntityData(in entityId);
        entityData.RemoveEntityCom(in comType);
    }
    public void RemoveAllCom(in int entityId)
    {
        var entityData = GetSceneEntityData(in entityId);
        entityData.RemoveAllEntityCom();
    }
    public T GetEntityCom<T>(in int entityId, in EnSceneEntityComType comType)
        where T : class, IEntity3DComData, new()
    {
        var entityData = GetSceneEntityData(in entityId);
        var com = entityData.GetEntityCom<T>(in comType);
        return com;
    }
    public bool HasEntityCom(in int entityId, in EnSceneEntityComType comType)
    {
        var entityData = GetSceneEntityData(in entityId);
        var result = entityData.ContainsEntityCom(in comType);
        return result;
    }

    #endregion

    #region cmd
    public void AddCmd(in int entityId, in EnEntityCmd cmd)
    {
        var entityData = GetSceneEntityData(in entityId);
        var cmdCom = entityData.GetEntityCom<EntityCmdComData>(EnSceneEntityComType.Cmd);
        cmdCom.AddCmd(in cmd);
    }
    public void RemoveCmd(in int entityId, in EnEntityCmd cmd)
    {
        var entityData = GetSceneEntityData(in entityId);
        var cmdCom = entityData.GetEntityCom<EntityCmdComData>(EnSceneEntityComType.Cmd);
        cmdCom.RemoveCmd(in cmd);
    }
    public void RemoveCmd(in int entityId, in int cmd)
    {
        RemoveCmd(in entityId, (EnEntityCmd)cmd);
    }
    public void AddCmd(in int entityId, in int cmd)
    {
        AddCmd(in entityId, (EnEntityCmd)cmd);
    }

    public bool HasCmd(in int entityId, in int cmd)
    {
        var result = HasCmd(in entityId, (EnEntityCmd)cmd);
        return result;
    }
    public bool HasCmd(in int entityId, in EnEntityCmd cmd)
    {
        var entityData = GetSceneEntityData(in entityId);
        var cmdCom = entityData.GetEntityCom<EntityCmdComData>(EnSceneEntityComType.Cmd);
        var result = cmdCom.ContainsCmd(cmd);
        return result;
    }
    #endregion

    #region skill
    public int GetEntityCurSkillId(int entityID)
    {
        var animCom = GetEntityCom<EntityAnimComData>(in entityID, EnSceneEntityComType.Animation);
        var skillId = animCom.GetCurSkillId();
        return skillId;
    }
    #endregion

    #region controller

    public void AddControllerType(in int entityId, in EnSceneEntityControllerType controllerType)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneEntityData>(in entityId);
        entityData.AddControllerType(in controllerType);
    }

    public void RemoveControllerType(in int entityId, in EnSceneEntityControllerType controllerType)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneEntityData>(in entityId);
        entityData.RemoveControllerType(in controllerType);
    }

    public bool IsCanController(in int entityId, in EnSceneEntityControllerType controllerType)
    {
        var entityData = EntityMgr.Instance.GetEntityData<SceneEntityData>(in entityId);
        var result = entityData.IsCanController(in controllerType);
        return result;
    }
    #endregion
}

