// ChapterCfg
public class ChapterCfg : ICfg
{
	private ChapterCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nChapterId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fCofficient;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nSuccessDiamondCount;
	// 默认初始球id，等级
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrDefaultBalls;
	// 波数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMinWaveId;
	// 波数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxWaveId;
	// boos波数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBossWaveId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapBossMonsterId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBossBgmAudioId;
	// 能量阶段
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrEnergyStageValue;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxEnergyStage;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrSkillBallIds;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrSkillPassivityIds;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrAttributeIds;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrMonsterIds;
	public System.Int32 GetID()
	{
		return nChapterId;
	}
}
