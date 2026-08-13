using System.Collections.Generic;


public abstract class EntityBuffData : IEntityBuffData, IDescription
{

    public abstract EnBuff GetBuff();

    public abstract EnTypeId GetTypeDefineId();
    public abstract List<int> SerializeToIntArray();
    public abstract void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData);

    public virtual void OnPoolDestroy()
    {
        
    }

    public virtual void OnPoolInit(CommonSerializeToIntArrayUserData userData)
    {
       
    }

    public virtual void GetDescriptionList(ref List<DescriptionInfo> descInfos, EnDescriptionLevel level)
    {
        
    }
}
public interface IEntityBuff : IClassPoolInit<EntityBuffDataUserData>
{
    public EnBuff GetBuff();
    public void AddKey(int addKey, IEntityBuffData buffParams);
    public void RemoveKey(int addKey);
    public void ReaddKey(int addKey, IEntityBuffData buffParams);
    public void Clear();
    public bool IsClear();
    public int GetCount();
}
public abstract class EntityBuff<T> : EntityBuff
    where T: class, IEntityBuffData
{
    public sealed override void AddKey(int addKey, IEntityBuffData buffParams)
    {
        base.AddKey(addKey, buffParams);

        AddKey(addKey, buffParams as T);
    }
    public sealed override void ReaddKey(int addKey, IEntityBuffData buffParams)
    {
        base.ReaddKey(addKey, buffParams);

        ReaddKey(addKey, buffParams as T);
    }
    protected abstract void AddKey(int addKey, T buffParams);
    protected abstract void ReaddKey(int addKey, T buffParams);
}
public abstract class EntityBuff : IEntityBuff
{
    public int Count => _AddKey.Count;
    protected int _TargetEntityID = GlobalConfig.IntM1;
    protected int _SourceEntityID = GlobalConfig.IntM1;
    protected EnBuff _Buff = EnBuff.None;

    private readonly HashSet<int> _AddKey = new();
    public virtual void OnPoolDestroy()
    {
        _Buff = EnBuff.None;
        _TargetEntityID
            = GlobalConfig.IntM1;
        _AddKey.Clear();
    }

    public virtual void OnPoolInit(EntityBuffDataUserData userData)
    {
        _Buff = userData.buff;
        _TargetEntityID = userData.targetEntityID;
        _SourceEntityID = userData.sourceEntityID;
    }
    public EnBuff GetBuff()
    {
        return _Buff;
    }

    public virtual void AddKey(int addKey, IEntityBuffData buffParams)
    {
        _AddKey.Add(addKey);
    }
    public virtual void RemoveKey(int addKey)
    {
        _AddKey.Remove(addKey);
    }
    public virtual void Clear()
    {

    }
    public bool IsClear()
    {
        return Count == 0;
    }
    public virtual void ReaddKey(int addKey, IEntityBuffData buffParams)
    {
        _AddKey.Add(addKey);
    }

    public int GetCount()
    {
        return Count;
    }
}