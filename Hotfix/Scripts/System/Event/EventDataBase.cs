public interface IEventData : IClassPoolNone
{
    public bool IsValid();
    public void FireEvent(in int sourceID, in int typeID, in IClassPool userData);
}