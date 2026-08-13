using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAudioScheduleAction : ISkillScheduleAction
{
    private float _Schedule;
    private EnAudioId _AudioId;
    public List<int> SerializeToIntArray()
    {
        var result = new List<int>();
        ISerializeToIntArray.SerializeToInt(ref result, _Schedule);
        ISerializeToIntArray.SerializeToInt(ref result, _AudioId);
        return result;
    }
    public void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        var localIndex = 0;
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref localIndex, out _Schedule);
        ISerializeToIntArray.ParseToValue(in datas, in start, in count, ref localIndex, out _AudioId);
    }

    public void GetEventList(ref List<SkillItemEventInfo> eventList)
    {
        var eventInfo = ClassPoolMgr.Instance.Pull<SkillItemEventInfo>();
        eventInfo.onEvent = OnEvent;
        eventInfo.userData = null;
        eventInfo.schedule = _Schedule;
        eventList.Add(eventInfo);
    }

    public EnAtkLinkScheculeType GetScheduleType() => EnAtkLinkScheculeType.Audio;

    public EnTypeId GetTypeDefineId() => EnTypeId.SkillAudioScheduleAction;

    public void OnPoolDestroy()
    {
        
    }

    public void OnPoolInit(ISerializeToIntArrayUserData userData)
    {
        DeserializeFromIntArray(userData.arrParams, userData.startIndex, userData.count, userData);
    }

    public void Reset()
    {
        
    }

    private void OnEvent(int entityID, IClassPoolDestroy userData)
    {
        AudioMgr.Instance.Play(_AudioId);
    }
}
