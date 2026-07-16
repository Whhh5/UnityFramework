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
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nSkillType;
	// 球图标
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 半径
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fRadius;
	// 发射特效
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFireEffectId;
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
	public System.Int32 GetID()
	{
		return nBallId;
	}
}
