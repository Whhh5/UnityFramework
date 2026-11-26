using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

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



//public partial class UIGoodsMgr
//{
//    public void AddGoods(EnGoodsId goodsId, int exp)
//    {
//        UIGoodsMgr.Instance.AddGoods((int)goodsId, exp);
//    }
//    public int GetGoodsCount(EnGoodsId goodsId)
//    {
//        var exp = UIGoodsMgr.Instance.GetGoodsCount((int)goodsId);
//        return exp;
//    }
//}
