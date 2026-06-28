// MapCfg
public class MapCfg : ICfg
{
	private MapCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapId;
	// 指令类型
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAssetID;
	// 玩家位置
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single fPlayerLocalPos;
	// 地图左下角X
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapLbPos;
	// 地图大小
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapSize;
	// 地图左边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBoundLeft;
	// 地图左边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBoundRight;
	// 地图左边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBoundUp;
	// 地图左边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBoundBottom;
	public System.Int32 GetID()
	{
		return nMapId;
	}
}
