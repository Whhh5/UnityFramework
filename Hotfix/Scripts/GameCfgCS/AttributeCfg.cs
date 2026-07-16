// AttributeCfg
public class AttributeCfg : ICfg
{
	private AttributeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAttributeId;
	public System.Int32 GetID()
	{
		return nAttributeId;
	}
}
