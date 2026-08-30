// GoodsTypeCfg
public class GoodsTypeCfg : ICfg
{
	private GoodsTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGoodsTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nItemIconAssetId;
	public System.Int32 GetID()
	{
		return nGoodsTypeId;
	}
}
