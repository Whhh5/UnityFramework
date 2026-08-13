// LanguageCfg
public class LanguageCfg : ICfg
{
	private LanguageCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strValueCh;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strValueEn;
	public System.Int32 GetID()
	{
		return nLangId;
	}
}
