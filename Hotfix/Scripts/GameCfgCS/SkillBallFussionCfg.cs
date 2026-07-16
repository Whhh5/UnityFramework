// SkillBallFussionCfg
public class SkillBallFussionCfg : ICfg
{
	private SkillBallFussionCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBallId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrSourceBallIds;
	public System.Int32 GetID()
	{
		return nBallId;
	}
}
