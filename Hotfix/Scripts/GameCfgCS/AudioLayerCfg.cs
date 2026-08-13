// AudioLayerCfg
public class AudioLayerCfg : ICfg
{
	private AudioLayerCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nWeight;
	public System.Int32 GetID()
	{
		return nLayer;
	}
}
