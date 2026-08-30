// CharacterCfg
public class CharacterCfg : ICfg
{
	private CharacterCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCharacterID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nNameLangId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nClassTypeId;
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
	public System.Int32 GetID()
	{
		return nCharacterID;
	}
}
