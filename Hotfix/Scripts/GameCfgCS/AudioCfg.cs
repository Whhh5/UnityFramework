// AudioCfg
public class AudioCfg : ICfg
{
	private AudioCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAudioID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.String strDescEditor;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLayer;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fLength;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 bIsLoop;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAssetID;
	public System.Int32 GetID()
	{
		return nAudioID;
	}
}
