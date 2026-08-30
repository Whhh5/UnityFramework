// GemTypeCfg
public class GemTypeCfg : ICfg
{
	private GemTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGemTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGrooveAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLightAssetId;
	public System.Int32 GetID()
	{
		return nGemTypeId;
	}
}
