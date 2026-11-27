// CmdTypeCfg
public class CmdTypeCfg : ICfg
{
	private CmdTypeCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdID;
	public System.Int32 GetID()
	{
		return nCmdID;
	}
}
