public partial class BuffUtil
{
	public static IEntityBuff CreateBuffData(EnBuff buff, IClassPoolDestroy data)
	{
		return buff switch
		{
			EnBuff.NoMovement => ClassPoolMgr.Instance.Pull<EntityNoMovementBuff>(data),
			EnBuff.Vulnerable => ClassPoolMgr.Instance.Pull<EntityVulnerableBuff>(data),
			EnBuff.Burn => ClassPoolMgr.Instance.Pull<EntityBurnBuff>(data),
			EnBuff.Crystal => ClassPoolMgr.Instance.Pull<EntityCrystalBuff>(data),
			EnBuff.ShatteredArmor => ClassPoolMgr.Instance.Pull<EntityShatteredArmorBuff>(data),
			EnBuff.Freeze => ClassPoolMgr.Instance.Pull<EntityFreezeBuff>(data),
			EnBuff.Electric => ClassPoolMgr.Instance.Pull<EntityElectricBuff>(data),
            _ => default,
		};
	}
}
