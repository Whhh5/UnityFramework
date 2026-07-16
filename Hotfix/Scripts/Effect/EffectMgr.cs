using System.Collections.Generic;
using UnityEngine;


public class EffectMgr : Singleton<EffectMgr>
{
    private class EffectInfo : IClassPoolDestroy
    {
        public int effectID = -1;
        public void OnPoolDestroy()
        {
            effectID = -1;
        }
    }
    private class DelayDestroyInfo : IClassPoolDestroy
    {
        public int effectEntityID;
        public float endTime;
        public void OnPoolDestroy()
        {
            endTime = -1;
        }
    }
    public override EnManagerFuncType FuncType => base.FuncType | EnManagerFuncType.Update;
    private readonly Dictionary<int, EffectInfo> _EffectInfos = new();
    private readonly List<DelayDestroyInfo> _DelayDestroyList = new();
    private readonly HashSet<int> _DelayEntityID = new();
    private int _DelayListIndex = 0;

    public bool IsValid(int entityID)
    {
        if (!_EffectInfos.ContainsKey(entityID))
            return false;
        if (_DelayEntityID.Contains(entityID))
            return false;
        return true;
    }

    public int PlayEffectOnce(in int effectID, in Vector3 worldPos, in Vector3 rot, in Vector3 scale)
    {
        var entityId = PlayEffectOnce<EffectEntityData>(in effectID, in worldPos, in rot, in scale);
        return entityId;
    }
    public int PlayEffectOnce<T>(in int effectID, in Vector3 worldPos, in Vector3 rot, in Vector3 scale)
        where T : EffectEntityData, new()
    {
        var entityID = PlayEffect<T>(effectID);
        var entityData = EntityMgr.Instance.GetEntityData<EffectEntityData>(entityID);
        entityData.SetPosition(worldPos);
        entityData.SetLocalRotation(rot);
        entityData.SetLocalScale(scale);
        var time = entityData.GetMaxTime();
        AddDelayDesytroyList(entityID, time);
        return entityID;
    }
    public int PlayEffectOnce(in int effectID, in Vector3 worldPos)
    {
        var entityID = PlayEffect(effectID);
        var entityData = EntityMgr.Instance.GetEntityData<EffectEntityData>(entityID);
        entityData.SetPosition(worldPos);
        var time = entityData.GetMaxTime();
        AddDelayDesytroyList(entityID, time);
        return entityID;
    }
    public int PlayEffect<T>(in int effectID, in Vector3 worldPos, in Vector3 rot, in Vector3 scale)
        where T : EffectEntityData, new()
    {
        var entityID = PlayEffect<T>(effectID);
        var entityData = EntityMgr.Instance.GetEntityData(entityID);
        entityData.SetPosition(worldPos);
        entityData.SetLocalRotation(rot);
        entityData.SetLocalScale(scale);
        return entityID;
    }
    public int PlayEffect(in int effectID, in Vector3 worldPos, in Vector3 rot, in Vector3 scale)
    {
        var entityID = PlayEffect<EffectEntityData>(effectID, worldPos, rot, scale);
        return entityID;
    }
    public int PlayEffect(int effectID, Vector3 worldPos)
    {
        var entityID = PlayEffect(effectID);
        var entityData = EntityMgr.Instance.GetEntityData(entityID);
        entityData.SetPosition(worldPos);
        return entityID;
    }
    public int PlayEffect<T>(in int effectID)
        where T: EffectEntityData, new()
    {
        var userData = ClassPoolMgr.Instance.Pull<EffectEntityUserData>();
        userData.effectCfgID = effectID;
        var entityID = EntityMgr.Instance.CreateEntityData<T>(userData);
        ClassPoolMgr.Instance.Push(userData);

        var effectData = EntityMgr.Instance.GetEntityData<T>(entityID);
        effectData.Play();

        EntityMgr.Instance.LoadEntity(entityID);

        var effectInfo = ClassPoolMgr.Instance.Pull<EffectInfo>();
        effectInfo.effectID = effectID;
        _EffectInfos.Add(entityID, effectInfo);
        return entityID;
    }
    public int PlayEffect(in int effectID)
    {
        var entityId = PlayEffect<EffectEntityData>(effectID);
        return entityId;
    }
    public void DestroyEffect(in int entityID)
    {
        if (!_EffectInfos.TryGetValue(entityID, out var _))
            return;
        var effectData = EntityMgr.Instance.GetEntityData<EffectEntityData>(entityID);
        effectData.Stop();
        var time = effectData.GetMaxTime();
        AddDelayDesytroyList(entityID, time);
    }
    private void AddDelayDesytroyList(in int entityID, in float time)
    {
        var desInfo = ClassPoolMgr.Instance.Pull<DelayDestroyInfo>();
        desInfo.effectEntityID = entityID;
        desInfo.endTime = ABBUtil.GetGameTimeSeconds() + time;
        _DelayDestroyList.Add(desInfo);
        _DelayEntityID.Add(entityID);
    }
    public void KillEffect(int entityId)
    {
        if (_DelayEntityID.Contains(entityId))
        {
            var desInfo = _DelayDestroyList.Find(item => item.effectEntityID == entityId);
            _DelayDestroyList.Remove(desInfo);
            _DelayEntityID.Remove(entityId);
            ClassPoolMgr.Instance.Push(desInfo);
        }
        InnerKillEffect(entityId);
    }
    private void InnerKillEffect(int entityID)
    {
        if (!_EffectInfos.TryGetValue(entityID, out var effectInfo))
            return;
        _EffectInfos.Remove(entityID);
        EntityMgr.Instance.RecycleEntityData(entityID);
        ClassPoolMgr.Instance.Push(effectInfo);
    }

    public override void Update()
    {
        base.Update();

        var count = _DelayDestroyList.Count;
        if (count > 0)
        {
            _DelayListIndex %= count;
            var desInfo = _DelayDestroyList[_DelayListIndex++];
            if (desInfo.endTime < ABBUtil.GetGameTimeSeconds())
            {
                _DelayDestroyList.RemoveAt(--_DelayListIndex);
                _DelayEntityID.Remove(desInfo.effectEntityID);
                InnerKillEffect(desInfo.effectEntityID);
                ClassPoolMgr.Instance.Push(desInfo);
            }
        }
    }
}
