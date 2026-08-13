using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum EnCmdStep
{
    None = -1,
    Step0,
    Step1,
    Step2,
    Step3,
    Step4,
    Step5,
    Step6,
    Step7,
}
public class SkillPlayableAdapterUserData : PlayableAdapterUserData, ISerializeToIntArrayUserData
{
    public int startIndex { get; set; }
    public int count { get; set; }
    public int[] arrParams { get; set; }

    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        startIndex
            = count
            = -1;
        arrParams = null;
    }
}
public interface ISkillPlayableAdapter : IPlayableAdapter, ISerializeToIntArray
{
    public void ExecutePlayable();
    public void RemovePlayable();
    public void ReexecutePlayable();
    public void CancelPlayable();
    public bool NextAnimLevelComdition();
}

public interface ISkilAnimationLayers
{
    public EnAnimLayerBit GetAnimationLayers();
}
public abstract class SkillPlayableAdapter<T> : PlayableAdapter<T>, ISkillPlayableAdapter, ISerializeToIntArray, ISkilAnimationLayers
    where T : SkillPlayableAdapterUserData, new()
{
    public abstract EnTypeId GetTypeDefineId();
    public abstract List<int> SerializeToIntArray();
    public abstract void DeserializeFromIntArray(int[] datas, int start, int count, ISerializeToIntArrayUserData userData);
    public abstract EnAnimLayerBit GetAnimationLayers();

    public override void OnPoolInit(T userData)
    {
        base.OnPoolInit(userData);
        DeserializeFromIntArray(userData.arrParams, userData.startIndex, userData.count, userData);
    }
    public virtual void ExecutePlayable()
    {
    }
    public virtual void RemovePlayable()
    {
    }
    public virtual void ReexecutePlayable()
    {

    }
    public virtual void CancelPlayable()
    {

    }
    public virtual bool NextAnimLevelComdition()
    {
        return true;
    }

}
