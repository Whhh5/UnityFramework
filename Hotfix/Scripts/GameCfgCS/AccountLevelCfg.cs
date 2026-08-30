// AccountLevelCfg
public class AccountLevelCfg : ICfg
{
	private AccountLevelCfg() {}
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLevelId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nExp;
	public System.Int32 GetID()
	{
		return nLevelId;
	}
}
