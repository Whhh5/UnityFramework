// SkillBallPartCfg
public class SkillBallPartCfg : ICfg
{
	private SkillBallPartCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nPartId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nClassTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxStar;
	// 参数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrStarParams;
	public System.Int32 GetID()
	{
		return nPartId;
	}
}
