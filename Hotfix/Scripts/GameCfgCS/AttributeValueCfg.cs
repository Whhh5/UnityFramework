// AttributeValueCfg
public class AttributeValueCfg : ICfg
{
	private AttributeValueCfg() {}
	// id
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nValueId;
	// 当前生命值
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nHp;
	// 最大生命值
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nLimitHp;
	// 攻击
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nAttack;
	// 物理护甲
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nPhysicsArmor;
	// 魔法护甲
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMagicArmor;
	// 暴击概率
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCritical;
	// 暴击伤害
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nCriticalDamage;
	// 移动速度
	[Newtonsoft.Json.JsonProperty()] public readonly System.Int32 nMoveSpeed;
	public System.Int32 GetID()
	{
		return nValueId;
	}
}
