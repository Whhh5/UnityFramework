// AttributeCfg
public class AttributeCfg : ICfg
{
	private AttributeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAttributeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strDescEditor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 值/百分比
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nValueType;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nValueDescLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	public System.Int32 GetID()
	{
		return nAttributeId;
	}
}
