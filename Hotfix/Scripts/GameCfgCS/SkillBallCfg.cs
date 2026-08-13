// SkillBallCfg
public class SkillBallCfg : ICfg
{
	private SkillBallCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBallId;
	// 枚举名称
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strDescEditor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nClassTypeId;
	// 主，被
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBallFuncType;
	// 基，融
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBallUseType;
	// 白球，技能球
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBallType;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDescLangId;
	// 球图标
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 半径
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fRadius;
	// 发射特效
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFireEffectId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bIsCollision;
	// 碰撞特效
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCollisionEffectId;
	// 最大星级
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxStar;
	// 参数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrStarParams;
	// 触底是否回收
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bIsRecycle;
	// 延迟发射时间
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fDelayFireTime;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFireAudioId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nHitAudioId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nHitCritAudioId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bMonsterDownClear;
	public System.Int32 GetID()
	{
		return nBallId;
	}
}
