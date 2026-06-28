// MapCfg
public class MapCfg : ICfg
{
	private MapCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapId;
	// 玩家位置
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fPlayerLocalPos;
	// 地图左下角X
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapLbPos;
	// 地图大小
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapSize;
	// 地图边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBound;
	// 地图格子资源id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nChunkAsssetId;
	// 地图资源id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapAssetID;
	public System.Int32 GetID()
	{
		return nMapId;
	}
}
