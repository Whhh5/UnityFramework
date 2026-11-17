// GemQualityCfg
public class GemQualityCfg : ICfg
{
	private GemQualityCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGemQualityId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strColor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	public System.Int32 GetID()
	{
		return nGemQualityId;
	}
}
