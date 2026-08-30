// GemEffectTypeCfg
public class GemEffectTypeCfg : ICfg
{
	private GemEffectTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectDescLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectDescLangId2;
	public System.Int32 GetID()
	{
		return nEffectTypeId;
	}
}
