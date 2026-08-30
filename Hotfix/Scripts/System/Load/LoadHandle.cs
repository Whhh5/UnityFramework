
using UnityEngine;

public struct LoadHandle
{
    public int loadId;
    public LoadHandle(int loadId)
    {
        this.loadId = loadId;
    }

    public readonly bool IsValid()
    {
        return ABBLoadMgr.Instance.Contains(loadId);
    }

    public readonly T GetObject<T>()
        where T : Object
    {
        var obj = ABBLoadMgr.Instance.GetObject<T>(loadId);
        return obj;
    }

    public static implicit operator int(LoadHandle handler)
    {
        return handler.loadId;
    }
    public static implicit operator LoadHandle(int handlerId)
    {
        return new(handlerId);
    }
}