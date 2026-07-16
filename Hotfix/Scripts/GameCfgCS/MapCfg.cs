// MapCfg
public class MapCfg : ICfg
{
	private MapCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapId;
	// 摄像机位置
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] arrCameraPos;
	// 地图深度
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single nDepth;
	// 玩家位置
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fPlayerLocalPos;
	// 地图左下角X
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapLbPos;
	// 地图大小
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapSize;
	// 地图边界
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] fMapBound;
	// 格子h行列
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] nGridRowCol;
	// 格子大小
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] nGridSize;
	// 默认初始球id，等级
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] arrDefaultBalls;
	// 波数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 minWaveId;
	// 波数
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 maxWaveId;
	// 地图格子资源id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nGridAsssetId;
	// 地图资源id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapAssetID;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMapBossMonsterId;
	// 
	[Newtonsoft.Json.JsonProperty()] public readonly System.Single[] nMapBossPos;
	// id/属性
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32[] nBossMonsterInfo;
	public System.Int32 GetID()
	{
		return nMapId;
	}
}
