public interface IVirtualTimelineNode<T> : IClassPoolInit<T>, IVirtualTimelineNode
    where T : VirtualTimelineUserData
{
}
public interface IVirtualTimelineNode : IClassPoolInit, ISerializeToIntArray
{
    public float GetVirtualTimelineNodeDurationTime();
    public void OnVirtualTimelineNodeUpdate(in float nodeLocalTime, in float deltaTime, in float nodeProgress01);
    public void OnVirtualTimelineNodeStart();
    public bool IsVirtualTimelineNodeEnd(in float nodeLocalTime, in float deltaTime, in float nodeProgress01);
    public void OnVirtualTimelineNodeEnd();
}
public interface IVirtualTimelineEvent : IClassPoolInit, ISerializeToIntArray
{
    public void OnVirtualTimelineEvent();
}
public interface IVirtualTimelineEvent<T> : IVirtualTimelineEvent, IClassPoolInit<T>
    where T : class, ISerializeToIntArrayUserData
{
}



public interface IVirtualTimeline<T> : IVirtualTimeline, IClassPoolInit<T>
    where T : class, IClassPoolDestroy
{
}
public interface IVirtualTimeline : IClassPoolInit, ISerializeToIntArray
{
    public ref readonly float GetVirtualTimelineTotalTime();
    public void OnVirtualTimelineStart(in float localTime);
    public void OnVirtualTimelineUpdtae(in float localTime, in float deltaTime, in float progress01);
    public void OnVirtualTimelineEnd(in float localTime, in float deltaTime);
}
