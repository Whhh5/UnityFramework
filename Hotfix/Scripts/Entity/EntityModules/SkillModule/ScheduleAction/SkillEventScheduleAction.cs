using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnSkillEventScheduleId
{
    None,
    Attack = 100,
    Skill = 200,
    Die = 300,
    Summon = 400,
    Debut = 500,
}
public abstract class SkillEventScheduleData : ISkillEventScheduleData
{
    //var index = 0;
    //ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out testInt);
    public abstract void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData);
    //var list = new List<int>();
    //ISerializeToIntArray.SerializeToInt(ref list, in testInt);
    //return list;
    public abstract List<int> SerializeToIntArray();

    public abstract EnTypeId GetTypeDefineId();

    public virtual void OnPoolDestroy()
    {
    }

    public void OnPoolInit(ISerializeToIntArrayUserData userData)
    {
        DeserializeFromIntArray(userData.arrParams, userData.startIndex, userData.count, userData);
    }

}
public interface ISkillEventScheduleData : ISerializeToIntArray<ISerializeToIntArrayUserData>
{

}
public class SkillEventScheduleAction : ISkillScheduleAction
{
    private float _Schedule;
    private EnSkillEventScheduleId _EventId;
    private ISkillEventScheduleData _EventData;
    public void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var index = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out _Schedule);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out _EventId);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref index, out _EventData, in userData);
    }

    public List<int> SerializeToIntArray()
    {
        var list = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref list, in _Schedule);
        ISerializeToIntArray.SerializeToInt(ref list, _EventId);
        ISerializeToIntArray.SerializeToInt(ref list, in _EventData);
        return list;
    }

    public void GetEventList(ref List<SkillItemEventInfo> eventList)
    {
        var eventData = ClassPoolMgr.Instance.Pull<SkillItemEventInfo>();
        eventData.schedule = _Schedule;
        eventData.onEvent = ScheduleEvent;
        eventList.Add(eventData);
    }

    public EnAtkLinkScheculeType GetScheduleType() => EnAtkLinkScheculeType.Event;

    public EnTypeId GetTypeDefineId() => EnTypeId.SkillEventScheduleAction;

    public void OnPoolDestroy()
    {
        ClassPoolMgr.Instance.Push(_EventData);
    }

    public void OnPoolInit(ISerializeToIntArrayUserData userData)
    {
        DeserializeFromIntArray(userData.arrParams, userData.startIndex, userData.count, userData);
    }

    public void Reset()
    {

    }

    public void ScheduleEvent(int entityID, IClassPoolDestroy userData)
    {
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_SKILL_EVENT_SCHEDULE, entityID, (int)_EventId, _EventData);
    }
}
