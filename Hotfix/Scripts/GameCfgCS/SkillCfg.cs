// SkillCfg
public class SkillCfg : ICfg
{
	private SkillCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nSkillID;
	// 名字
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strName;
	// 类型
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nTypeId;
	// 参数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrParams;
	// 是否启用根运动
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bApplyRootMotion;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAnimLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bIsIK;
	// 指令登等级
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLevel;
	// 过渡时间
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fMixerTime;
	public System.Int32 GetID()
	{
		return nSkillID;
	}
}
