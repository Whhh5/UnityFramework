// GoodsCfg
public class GoodsCfg : ICfg
{
	private GoodsCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGoodsId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLinkCfgId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGoodsTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDescriptLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconFrameAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bShowTopBar;
	public System.Int32 GetID()
	{
		return nGoodsId;
	}
}
