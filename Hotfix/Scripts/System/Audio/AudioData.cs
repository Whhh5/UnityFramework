
using UnityEngine.Audio;

public class AudioData : IClassPoolDestroy
{
    public int clipID;
    public EnAudioLayer layer;
    public int inputPort;
    public float startTime;
    public LoadHandle clipLoadHandle;

    public void OnPoolDestroy()
    {
        startTime
            = clipID
            = inputPort
            = -1;
        clipLoadHandle = default;
        layer = EnAudioLayer.None;
    }
}