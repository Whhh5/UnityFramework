using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum EnGoodsId
{
    None,
    Coin = 10000001,
    Diamond = 10000002,
    Energy = 20000001,
    Exp = 30000001,
}
public enum EnGoodsTypeId
{
    None,
    Currenty = 1, // 货币
    Consumables = 2, // 消耗品
    Const = 3, // 静态物品
    Core = 11, // 核心
    Gem = 12, // 宝石
}
public class PersistentGoodsData : PersistentDataBase
{
    private class GoodsData
    {
        public int count;
    }
    [JsonProperty]
    private readonly Dictionary<int, GoodsData> _GoodsDatas = new();

    private GoodsData GetGoodsData(int goodsCfgId)
    {
        if (!_GoodsDatas.TryGetValue(goodsCfgId, out var data))
        {
            data = new();
            _GoodsDatas.Add(goodsCfgId, data);
        }
        return data;
    }
    public int GetGoodsCount(int goodsCfgId)
    {
        if (!_GoodsDatas.TryGetValue(goodsCfgId, out var data))
            return 0;
        return data.count;
    }
    public void SetGoodsCount(int goodsCfgId, int count)
    {
        var data = GetGoodsData(goodsCfgId);
        data.count = count;
        SetDirty(true);
    }
}
