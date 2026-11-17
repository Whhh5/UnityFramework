// CharacterCfg
public class CharacterCfg : ICfg
{
	private CharacterCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCharacterID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMonsterId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nUIClassTypeId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fBallSpeed;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fBallFireInterval;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMaxLevel;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nBaseAttributeValueId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fLevelCoefficient;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrUnlockGoods;
	public System.Int32 GetID()
	{
		return nCharacterID;
	}
}
