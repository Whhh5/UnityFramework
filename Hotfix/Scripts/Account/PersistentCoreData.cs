using System.Collections.Generic;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Random = UnityEngine.Random;

public class PersistentCoreData : PersistentDataBase
{
    private class GemData : IClassPoolDestroy
    {
        public EnGemTypeId gemType;
        public int gemId;

        public void OnPoolDestroy()
        {
            gemType = EnGemTypeId.None;
            gemId = -1;
        }
    }
    private class CoreData : IClassPoolDestroy
    {
        public int goodsCfgId;
        public float effectValue;
        public EnCoreQuality quality;

        public bool isWear;
        public int wearIndex;
        public readonly List<GemData> gemDatas = new();
        public void Setting(int goodsCfgId, float effectValue, EnCoreQuality quality, int gemGrooveCount)
        {
            this.goodsCfgId = goodsCfgId;
            this.effectValue = effectValue;
            this.quality = quality;

            for (int i = 0; i < gemGrooveCount; i++)
            {
                var gemType = (EnGemTypeId)Random.Range((int)EnGemTypeId.None + 1, (int)EnGemTypeId.EnumCount);
                var gemData = ClassPoolMgr.Instance.Pull<GemData>();
                gemData.gemType = gemType;
                gemDatas.Add(gemData);
            }
        }

        public void OnPoolDestroy()
        {
            foreach (var item in gemDatas)
            {
                ClassPoolMgr.Instance.Push(item);
            }
            effectValue
                = goodsCfgId
                = wearIndex
                = -1;
            quality = EnCoreQuality.None;
            isWear = false;
            gemDatas.Clear();
        }
    }
    [JsonProperty]
    private readonly Dictionary<int, CoreData> _CoreDatas = new();

    private int NewCoreId()
    {
        for (int i = _CoreDatas.Count + 1; i > 0; i--)
        {
            if (_CoreDatas.ContainsKey(i))
                continue;
            return i;
        }
        throw new System.Exception("create core id error");
    }
    public int AddCoreData(int coreGoodsCfgId, float effectValue, EnCoreQuality quality)
    {
        var coreId = NewCoreId();
        var coreData = ClassPoolMgr.Instance.Pull<CoreData>();
        var qualityCfg = GameSchedule.Instance.GetCoreQualityCfg0((int)quality);
        coreData.Setting(coreGoodsCfgId, effectValue, quality, qualityCfg.nGemGrooveCount);
        _CoreDatas.Add(coreId, coreData);
        SetDirty(true);
        return coreId;
    }
    public bool RemoveCoreData(int coreId)
    {
        if (!_CoreDatas.TryGetValue(coreId, out var data))
            return false;
        ClassPoolMgr.Instance.Push(data);
        _CoreDatas.Remove(coreId);
        SetDirty(true);
        return true;
    }
    public int GetCoreGoodsCfgId(int coreId)
    {
        var coreData = _CoreDatas[coreId];
        return coreData.goodsCfgId;
    }
    public float GetEffectValue(int coreId)
    {
        var coreData = _CoreDatas[coreId];
        return coreData.effectValue;
    }
    public void SetEffectValue(int coreId, float effectValue)
    {
        var coreData = _CoreDatas[coreId];
        coreData.effectValue = effectValue;
        SetDirty(true);
    }
    public void GetCoreIds(ref List<int> coreIds)
    {
        foreach (var item in _CoreDatas)
            coreIds.Add(item.Key);
    }
    public int GetCoreCount()
    {
        return _CoreDatas.Count;
    }
    public bool IsWear(int coreId)
    {
        var coreData = _CoreDatas[coreId];
        return coreData.isWear;
    }
    public void SetIsWear(int coreId, bool isWear)
    {
        var coreData = _CoreDatas[coreId];
        coreData.isWear = isWear;
        SetDirty(true);
    }
    public int GetWearIndex(int coreId)
    {
        var coreData = _CoreDatas[coreId];
        return coreData.wearIndex;
    }
    public EnCoreQuality GetQuality(int coreId)
    {
        var coreData = _CoreDatas[coreId];
        return coreData.quality;
    }
    public void SetWearIndex(int coreId, int index)
    {
        var coreData = _CoreDatas[coreId];
        coreData.wearIndex = index;
        SetDirty(true);
    }
    public bool ContainsGem(int coreId, int index)
    {
        var coreData = _CoreDatas[coreId];
        var gemData = coreData.gemDatas[index];
        return gemData.gemId > 0;
    }
    public int GetGemId(int coreId, int index)
    {
        var coreData = _CoreDatas[coreId];
        var gemData = coreData.gemDatas[index];
        return gemData.gemId;
    }
    public void SetGemId(int coreId, int index, int gemId)
    {
        var coreData = _CoreDatas[coreId];
        var gemData = coreData.gemDatas[index];
        gemData.gemId = gemId;
    }
    public EnGemTypeId GetGemType(int coreId, int index)
    {
        var coreData = _CoreDatas[coreId];
        var gemData = coreData.gemDatas[index];
        return gemData.gemType;
    }
}
