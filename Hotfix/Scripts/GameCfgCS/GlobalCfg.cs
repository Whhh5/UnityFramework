// GlobalCfg
public class GlobalCfg : ICfg
{
	private GlobalCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strDescEditor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIntValue;
	public System.Int32 GetID()
	{
		return nId;
	}
}
