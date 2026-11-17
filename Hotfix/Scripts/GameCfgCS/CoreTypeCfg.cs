// CoreTypeCfg
public class CoreTypeCfg : ICfg
{
	private CoreTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCoreType;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strColor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFrameAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGrooveAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nActiveGrooveAssetId;
	public System.Int32 GetID()
	{
		return nCoreType;
	}
}
