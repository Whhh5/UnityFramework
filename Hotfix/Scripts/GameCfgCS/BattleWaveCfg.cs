// BattleWaveCfg
public class BattleWaveCfg : ICfg
{
	private BattleWaveCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nWaveId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrMonsterInfos;
	public System.Int32 GetID()
	{
		return nWaveId;
	}
}
