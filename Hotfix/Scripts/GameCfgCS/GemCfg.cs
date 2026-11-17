// GemCfg
public class GemCfg : ICfg
{
	private GemCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGemId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams1;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams2;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams3;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams4;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams5;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams6;
	public System.Int32 GetID()
	{
		return nGemId;
	}
}
