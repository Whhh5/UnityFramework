using System.Collections.Generic;
using UnityEngine;

public delegate void AbbEventCallback<T>(in int sourceId, in int typeId, in T data);
public delegate void AbbEventCallback(in int sourceId, in int typeId);
public class EventMgr : Singleton<EventMgr>
{
    private readonly int _DefClId = 0;
    private readonly Dictionary<EnABBEvent, Dictionary<int, IEventData>> _ActionList = new();

    public void Register<T>(in EnABBEvent ev, in AbbEventCallback<T> action)
        where T : IClassPool
    {
        Register<T>(in ev, 0, 0, in action);
    }
    public void Register<T>(in EnABBEvent ev, in int sourceID, in int typeID, in AbbEventCallback<T> action)
        where T : IClassPool
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
        {
            dic = new();
            _ActionList.Add(ev, dic);
        }
        var clId = typeof(T).GetHashCode();
        if (!dic.TryGetValue(clId, out var evData))
        {
            evData = ClassPoolMgr.Instance.Pull<EventData<T>>();
            dic.Add(clId, evData);
        }
        (evData as EventData<T>).AddEvent(in sourceID, in typeID, in action);
    }
    public void Unregister<T>(in EnABBEvent ev, in AbbEventCallback<T> action)
        where T : IClassPool
    {
        Unregister<T>(in ev, 0, 0, in action);
    }
    public void Unregister<T>(in EnABBEvent ev, in int sourceID, in int typeID, in AbbEventCallback<T> action)
        where T : IClassPool
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
            return;
        var clId = typeof(T).GetHashCode();
        if (!dic.TryGetValue(clId, out var evData))
            return;
        if (evData is not EventData<T> eventData)
        {
            Debug.LogError($"register error type T error, valid:{evData}, in:{typeof(T)}");
            return;
        }
        eventData.RemoveEvent(in sourceID, in typeID, in action);
        if (!eventData.IsValid())
        {
            ClassPoolMgr.Instance.Push(eventData);
            if (dic.Remove(clId) && dic.Count == 0)
            {
                _ActionList.Remove(ev);
            }
        }
    }
    public void FireExecute<T>(in EnABBEvent ev, in int sourceID, in int typeID, in T userData)
        where T : IClassPool
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
            return;
        var clId = userData.GetType().GetHashCode();
        if (!dic.TryGetValue(clId, out var evData))
            return;
        evData.FireEvent(in sourceID, in typeID, userData);
    }
    public void FireExecute<T>(in EnABBEvent ev, in int sourceID, in T userData)
        where T : IClassPool
    {
        FireExecute(in ev, in sourceID, 0, in userData);
    }
    public void FireExecute<T>(in EnABBEvent ev, in T userData)
        where T: IClassPool
    {
        FireExecute(in ev, 0, 0, in userData);
    }



    public void Register(in EnABBEvent ev, in int sourceID, in int typeID, in AbbEventCallback action)
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
        {
            dic = new();
            _ActionList.Add(ev, dic);
        }
        if (!dic.TryGetValue(_DefClId, out var evData))
        {
            evData = ClassPoolMgr.Instance.Pull<EventData>();
            dic.Add(_DefClId, evData);
        }
        if (evData is not EventData eventData)
        {
            Debug.LogError($"register error type T error, valid:{evData}");
            return;
        }
        eventData.AddEvent(in sourceID, in typeID, in action);
    }
    public void Unregister(in EnABBEvent ev, in int sourceID, in int typeID, in AbbEventCallback action)
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
            return;
        if (!dic.TryGetValue(_DefClId, out var evData))
            return;
        if (evData is not EventData eventData)
        {
            Debug.LogError($"register error type T error, valid:{evData}");
            return;
        }
        eventData.RemoveEvent(in sourceID, in typeID, in action);
        if (!eventData.IsValid())
        {
            ClassPoolMgr.Instance.Push(eventData);
            if (dic.Remove(_DefClId) && dic.Count == 0)
            {
                _ActionList.Remove(ev);
            }
        }
    }
    public void FireExecute(in EnABBEvent ev, in int sourceID, in int typeID)
    {
        if (!_ActionList.TryGetValue(ev, out var dic))
            return;
        if (!dic.TryGetValue(_DefClId, out var evData))
            return;
        evData.FireEvent(in sourceID, in typeID, null);
    }
    public void FireExecute(in EnABBEvent ev, in int sourceID)
    {
        FireExecute(in ev, in sourceID, 0);
    }
    public void FireExecute(in EnABBEvent ev)
    {
        FireExecute(in ev, 0, 0);
    }
}
