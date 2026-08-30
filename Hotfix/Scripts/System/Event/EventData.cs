
using System.Collections.Generic;
using System.Linq;

public class EventData<T> : IEventData
    where T : IClassPool
{
    private readonly Dictionary<int, Dictionary<int, HashSet<AbbEventCallback<T>>>> _EventList = new();
    public bool IsValid()
    {
        return _EventList.Count > 0;
    }
    public void AddEvent(in int sourceID, in int typeID, in AbbEventCallback<T> eventExecute)
    {
        if (!_EventList.TryGetValue(sourceID, out var sourceList))
        {
            sourceList = new();
            _EventList.Add(sourceID, sourceList);
        }
        if (!sourceList.TryGetValue(typeID, out var eventList))
        {
            eventList = new();
            sourceList.Add(typeID, eventList);
        }
        eventList.Add(eventExecute);
    }
    public void RemoveEvent(in int sourceID, in int typeID, in AbbEventCallback<T> eventExecute)
    {
        if (!_EventList.TryGetValue(sourceID, out var sourceList))
            return;
        if (!sourceList.TryGetValue(typeID, out var eventList))
            return;
        eventList.Remove(eventExecute);
    }
    private readonly List<AbbEventCallback<T>> _TempFireEventList = new(10);
    private void InvokHasList(HashSet<AbbEventCallback<T>> hashSet, in int sourceID, in int typeID, in T userData)
    {
        _TempFireEventList.Clear();
        _TempFireEventList.AddRange(hashSet);
        for (int i = 0; i < _TempFireEventList.Count; i++)
        {
            var callback = _TempFireEventList[i];
            if (!hashSet.Contains(callback))
                continue;
            callback.Invoke(in sourceID, in typeID, in userData);
        }
    }
    public void FireEvent(in int sourceID, in int typeID, in T userData)
    {
        if (sourceID > 0)
        {
            if (_EventList.TryGetValue(0, out var sourceList))
            {
                if (typeID > 0)
                {
                    if (sourceList.TryGetValue(0, out var eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID, in userData);
                    }
                    if (sourceList.TryGetValue(typeID, out eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID, in userData);
                    }
                }
                else
                {
                    foreach (var typeList in sourceList)
                    {
                        InvokHasList(typeList.Value, in sourceID, in typeID, in userData);
                    }
                }
            }
            if (!_EventList.TryGetValue(sourceID, out sourceList))
                return;


            if (typeID > 0)
            {
                if (sourceList.TryGetValue(0, out var eventList))
                {
                    InvokHasList(eventList, in sourceID, in typeID, in userData);
                }
                if (!sourceList.TryGetValue(typeID, out eventList))
                    return;
                InvokHasList(eventList, in sourceID, in typeID, in userData);
            }
            else
            {
                foreach (var typeList in sourceList)
                {
                    InvokHasList(typeList.Value, in sourceID, in typeID, in userData);
                }
            }
        }
        else
        {
            foreach (var sourceList in _EventList)
            {
                if (typeID > 0)
                {
                    if (sourceList.Value.TryGetValue(0, out var eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID, in userData);
                    }
                    if (!sourceList.Value.TryGetValue(typeID, out eventList))
                        return;
                    InvokHasList(eventList, in sourceID, in typeID, in userData);
                }
                else
                {
                    foreach (var typeList in sourceList.Value)
                    {
                        InvokHasList(typeList.Value, in sourceID, in typeID, in userData);
                    }
                }
            }
        }
    }

    public void FireEvent(in int sourceID, in int typeID, in IClassPool userData)
    {
        FireEvent(in sourceID, in typeID, (T)userData);
    }
}
public class EventData : IEventData
{
    private readonly Dictionary<int, Dictionary<int, HashSet<AbbEventCallback>>> _EventList = new();
    public bool IsValid()
    {
        return _EventList.Count > 0;
    }
    public void AddEvent(in int sourceID, in int typeID, in AbbEventCallback eventExecute)
    {
        if (!_EventList.TryGetValue(sourceID, out var sourceList))
        {
            sourceList = new();
            _EventList.Add(sourceID, sourceList);
        }
        if (!sourceList.TryGetValue(typeID, out var eventList))
        {
            eventList = new();
            sourceList.Add(typeID, eventList);
        }
        eventList.Add(eventExecute);
    }
    public void RemoveEvent(in int sourceID, in int typeID, in AbbEventCallback eventExecute)
    {
        if (!_EventList.TryGetValue(sourceID, out var sourceList))
            return;
        if (!sourceList.TryGetValue(typeID, out var eventList))
            return;
        if (eventList.Remove(eventExecute) && eventList.Count == 0)
        {
            if (sourceList.Remove(typeID) && sourceList.Count == 0)
            {
                _EventList.Remove(sourceID);
            }
        }
    }
    private readonly List<AbbEventCallback> _TempFireEventList = new(50);
    private void InvokHasList(HashSet<AbbEventCallback> list, in int sourceID, in int typeID)
    {
        _TempFireEventList.Clear();
        _TempFireEventList.AddRange(list);
        for (int i = 0; i < _TempFireEventList.Count; i++)
        {
            var callback = _TempFireEventList[i];
            if (!list.Contains(callback))
                continue;
            callback.Invoke(sourceID, typeID);
        }
    }
    public void FireEvent(in int sourceID, in int typeID)
    {
        if (sourceID > 0)
        {
            if (_EventList.TryGetValue(0, out var sourceList))
            {
                if (typeID > 0)
                {
                    if (sourceList.TryGetValue(0, out var eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID);
                    }
                    if (sourceList.TryGetValue(typeID, out eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID);
                    }
                }
                else
                {
                    foreach (var typeList in sourceList)
                    {
                        InvokHasList(typeList.Value, in sourceID, in typeID);
                    }
                }
            }
            if (!_EventList.TryGetValue(sourceID, out sourceList))
                return;


            if (typeID > 0)
            {
                if (sourceList.TryGetValue(0, out var eventList))
                {
                    InvokHasList(eventList, in sourceID, in typeID);
                }
                if (!sourceList.TryGetValue(typeID, out eventList))
                    return;
                InvokHasList(eventList, in sourceID, in typeID);
            }
            else
            {
                foreach (var typeList in sourceList)
                {
                    InvokHasList(typeList.Value, in sourceID, in typeID);
                }
            }
        }
        else
        {
            foreach (var sourceList in _EventList)
            {
                if (typeID > 0)
                {
                    if (sourceList.Value.TryGetValue(0, out var eventList))
                    {
                        InvokHasList(eventList, in sourceID, in typeID);
                    }
                    if (!sourceList.Value.TryGetValue(typeID, out eventList))
                        return;
                    InvokHasList(eventList, in sourceID, in typeID);
                }
                else
                {
                    foreach (var typeList in sourceList.Value)
                    {
                        InvokHasList(typeList.Value, in sourceID, in typeID);
                    }
                }
            }
        }
    }

    public void FireEvent(in int sourceID, in int typeID, in IClassPool userData)
    {
        FireEvent(in sourceID, typeID);
    }
}