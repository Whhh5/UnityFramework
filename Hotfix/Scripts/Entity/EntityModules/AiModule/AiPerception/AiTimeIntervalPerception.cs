using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTimeIntervalPerceptionUserData : IClassPoolDestroy, ISerializeToIntArrayUserData
{
    public bool isStartInvoke;
    public float interval;

    public int startIndex { get; set ; }
    public int count { get; set ; }
    public int[] arrParams { get; set ; }

    public void OnPoolDestroy()
    {
        isStartInvoke = false;
        interval = -1;
    }
}

public class AiTimeIntervalPerception : IAiPerception<AiTimeIntervalPerceptionUserData>
{
    public void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData)
    {
        throw new System.NotImplementedException();
    }

    public EnAiPerceptionId GetPerceptionId()
    {
        throw new System.NotImplementedException();
    }

    public ref readonly List<int> GetResultEntityList()
    {
        throw new System.NotImplementedException();
    }

    public EnTypeId GetTypeDefineId()
    {
        throw new System.NotImplementedException();
    }

    public void OnPoolDestroy()
    {
        throw new System.NotImplementedException();
    }

    public void OnPoolInit(AiTimeIntervalPerceptionUserData userData)
    {
        throw new System.NotImplementedException();
    }

    public List<int> SerializeToIntArray()
    {
        throw new System.NotImplementedException();
    }

    public bool UpdateAiPerception()
    {
        throw new System.NotImplementedException();
    }
}
