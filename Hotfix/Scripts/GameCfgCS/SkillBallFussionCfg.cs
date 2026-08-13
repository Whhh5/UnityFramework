// SkillBallFussionCfg
public class SkillBallFussionCfg : ICfg
{
	private SkillBallFussionCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFussionId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFinalBallId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFinalBallStar;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrSourceBallIds;
	public System.Int32 GetID()
	{
		return nFussionId;
	}
}
