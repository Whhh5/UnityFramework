// CoreQualityCfg
public class CoreQualityCfg : ICfg
{
	private CoreQualityCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCoreQuality;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strColor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGemGrooveCount;
	public System.Int32 GetID()
	{
		return nCoreQuality;
	}
}
