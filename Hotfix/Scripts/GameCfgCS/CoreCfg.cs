// CoreCfg
public class CoreCfg : ICfg
{
	private CoreCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCoreId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCoreType;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectId;
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
		return nCoreId;
	}
}
