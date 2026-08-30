// CoreEffectCfg
public class CoreEffectCfg : ICfg
{
	private CoreEffectCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCoreEffectId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectDescLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEffectDescLangId2;
	public System.Int32 GetID()
	{
		return nCoreEffectId;
	}
}
