// GemCfg
public class GemCfg : ICfg
{
	private GemCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGemId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fQualityCoefficient;
	public System.Int32 GetID()
	{
		return nGemId;
	}
}
