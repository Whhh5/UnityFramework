using System;
using System.Collections.Generic;

public partial class BuffUtil
{

	private static readonly Dictionary<EnBuff, Func<IClassPoolDestroy, IEntityBuff>> _EntityBuffs = new();

	public static void Register<T>(EnBuff buff)
		where T: IEntityBuff, new()
    {
        var func = Register_BuffData<T>();

        _EntityBuffs.Add(buff, func);

    }
	public static IEntityBuff CreateBuffData(EnBuff buff, IClassPoolDestroy data)
	{
		if (!_EntityBuffs.TryGetValue(buff, out var func))
			throw new ArgumentNullException();
		var result = func.Invoke(data);
		return result;

    }

    private static Func<IClassPoolDestroy, IEntityBuff> Register_BuffData<T>()
        where T : IEntityBuff, new()
    {
        return (data) =>
        {
            var cl = ClassPoolMgr.Instance.Pull<T>(data);
            return cl;
        };
    }
}
