
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class AudioLayer : IClassPoolDestroy
{
    private AudioMixerPlayable _MixerRoot;
    private readonly List<int> _IdlePorts = new();
    private readonly HashSet<int> _PlayableInfos = new();
    private int _Layer;
    public int Layer => _Layer;
    public int Count => _PlayableInfos.Count;

    public void OnPoolDestroy()
    {
        var root = _MixerRoot.GetOutput(0);
        root.DisconnectInput(_Layer - 1);
        _MixerRoot.Destroy();

        _Layer = -1;
        _IdlePorts.Clear();
        _MixerRoot = default;
        _PlayableInfos.Clear();
    }
    public void Init(PlayableGraph graph, Playable root, int layer, float weight)
    {
        this._Layer = layer;
        _MixerRoot = AudioMixerPlayable.Create(graph, 0);
        root.ConnectInput(layer - 1, _MixerRoot, 0, weight);
    }

    private int GetInputPort()
    {
        if (_IdlePorts.Count == 0)
        {
            var inputCount = _MixerRoot.GetInputCount();
            _MixerRoot.SetInputCount(inputCount + 1);
            return inputCount;
        }
        var port = _IdlePorts[^1];
        _IdlePorts.RemoveAt(_IdlePorts.Count - 1);
        return port;
    }
    public void UpdateWeight()
    {
        var weight = 1f; // 1f / _PlayableInfos.Count;
        foreach (var portId in _PlayableInfos)
        {
            var playable = _MixerRoot.GetInput(portId);
            var curTime = playable.GetTime();
            var maxtime = playable.GetDuration();

            var slider = Mathf.Clamp01((float)(curTime / Mathf.Min(1f, (float)(maxtime * 0.1f))));

            var curWeight = _MixerRoot.GetInputWeight(portId);
            var toWeight = Mathf.Lerp(curWeight, weight, slider);
            _MixerRoot.SetInputWeight(portId, toWeight);
        }
    }
    public int Connect(in AudioClip audioClip, float startTime, in bool isLoop)
    {
        var inputPort = GetInputPort();
        var audioClipPlayable = AudioClipPlayable.Create(_MixerRoot.GetGraph(), audioClip, isLoop);
        _MixerRoot.ConnectInput(inputPort, audioClipPlayable, 0, 1);

        var curTime = GameUtility.GetGameTime();
        audioClipPlayable.SetTime(curTime - startTime);

        _PlayableInfos.Add(inputPort);
        return inputPort;
    }
    public void DisconnectPort(int inputPort)
    {
        var playable = _MixerRoot.GetInput(inputPort);
        playable.Destroy();
        _MixerRoot.DisconnectInput(inputPort);

        _IdlePorts.Add(inputPort);
        _PlayableInfos.Remove(inputPort);

    }
    public void DisconnectAllPort()
    {
        var connectPortList = _PlayableInfos.ToList();
        for (int i = 0; i < connectPortList.Count; i++)
        {
            DisconnectPort(connectPortList[i]);
        }
    }
}