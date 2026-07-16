public class VirtualTimelineUserData : IClassPoolDestroy, ISerializeToIntArrayUserData
{
    public int startIndex { get; set; }
    public int count { get ; set; }
    public int[] arrParams { get ; set; }

    public virtual void OnPoolDestroy()
    {
        startIndex
            = count
            = -1;
        arrParams = null;
    }
}
