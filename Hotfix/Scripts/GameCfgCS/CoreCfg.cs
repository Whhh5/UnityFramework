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
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrEffectParams;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fQualityCoefficient;
	public System.Int32 GetID()
	{
		return nCoreId;
	}
}
