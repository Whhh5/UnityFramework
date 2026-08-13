// SkillPassivityCfg
public class SkillPassivityCfg : ICfg
{
	private SkillPassivityCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nSkillPassivityId;
	// 枚举名称
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strDescEditor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nClassTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDescLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nTipsDescLangId;
	// 图标
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxStar;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrStarParams;
	public System.Int32 GetID()
	{
		return nSkillPassivityId;
	}
}
