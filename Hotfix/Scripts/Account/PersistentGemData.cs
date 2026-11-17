using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PersistentGemData : PersistentDataBase
{
    private class GemData : IClassPoolDestroy
    {
        public int goodsCfgId;
        public float effectValue;
        public EnGemQuality quality;
        public EnGemTypeId type;
        public bool isInlay;
        public void OnPoolDestroy()
        {
            effectValue
                = goodsCfgId
                = -1;
            isInlay = false;
            quality = EnGemQuality.None;
            type = EnGemTypeId.None;
        }
        public void Setting(int goodsCfgId, float effectValue, EnGemQuality quality, EnGemTypeId type)
        {
            this.goodsCfgId = goodsCfgId;
            this.effectValue = effectValue;
            this.quality = quality;
            this.type = type;
        }
    }
    [JsonProperty]
    private readonly Dictionary<int, GemData> _Datas = new();

    private int NewGemId()
    {
        var id = _Datas.Count + 1;
        for (int i = id; i > 0; i--)
        {
            if (_Datas.ContainsKey(i))
                continue;
            return i;
        }
        throw new System.Exception("calculate gem id error");
    }
    private GemData GetData(int gemId)
    {
        return _Datas[gemId];
    }
    public int AddGemData(int gemGoodsCfgId, float effectValue, EnGemQuality quality, EnGemTypeId type)
    {
        var gemId = NewGemId();
        var gemData = ClassPoolMgr.Instance.Pull<GemData>();
        gemData.Setting(gemGoodsCfgId, effectValue, quality, type);
        _Datas.Add(gemId, gemData);
        SetDirty(true);
        return gemId;
    }
    public void RemoveGemData(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var gemData))
            return;
        ClassPoolMgr.Instance.Push(gemData);
        _Datas.Remove(gemId);
        SetDirty(true);
    }
    public float GetEffectValue(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var data))
            return 0;
        return data.effectValue;
    }
    public void SetEffectValue(int gemId, float effectValue)
    {
        var data = GetData(gemId);
        data.effectValue = effectValue;
        SetDirty(true);
    }
    public void SetIsInlay(int gemId, bool isInlay)
    {
        var data = GetData(gemId);
        data.isInlay = isInlay;
        SetDirty(true);
    }
    public EnGemTypeId GetGemType(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var data))
            return 0;
        return data.type;
    }
    public int GetGoodsCfgId(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var data))
            return 0;
        return data.goodsCfgId;
    }
    public bool GetIsInlay(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var data))
            return false;
        return data.isInlay;
    }
    public EnGemQuality GetQuality(int gemId)
    {
        if (!_Datas.TryGetValue(gemId, out var data))
            return 0;
        return data.quality;
    }
    public void GetGemIds(ref List<int> ids)
    {
        foreach (var item in _Datas)
            ids.Add(item.Key);
    }
}
