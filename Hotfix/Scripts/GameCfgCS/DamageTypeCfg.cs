// DamageTypeCfg
public class DamageTypeCfg : ICfg
{
	private DamageTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDamageType;
	// 指令类型
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strColor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nStyleNameLangId;
	public System.Int32 GetID()
	{
		return nDamageType;
	}
}
