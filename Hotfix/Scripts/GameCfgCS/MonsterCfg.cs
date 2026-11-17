// MonsterCfg
public class MonsterCfg : ICfg
{
	private MonsterCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMonsterID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strName;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEntityTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMonsterType;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrEnemyLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrFriendLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrGridLocalPos;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrGridValidEdge;
	// 体积
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fVolume;
	// 动作组id-skillcfg
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrSkillGroup;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAssetCfgID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nKeyCodeControllerID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIdleCmdID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nWalkCmdId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nRunCmdId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDieCmdID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nFlyDown;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAttackCmdId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdHitLeft;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdHitRight;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdHitUp;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdHitDown;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCmdFreeze;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAIID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nIconAssetID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nDieAudioId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nEnergyCount;
	public System.Int32 GetID()
	{
		return nMonsterID;
	}
}
