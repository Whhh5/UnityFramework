using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public enum EnAudioLayer
{
    None,
    Bgm,
    Layer2,
    Tips,
}
public class AudioMgr : Singleton<AudioMgr>
{
    public override EnManagerFuncType FuncType => base.FuncType | EnManagerFuncType.Update;
    public override EnSingletonOrder SingletonOrder => base.SingletonOrder;

    private AudioSource _AudioSource = null;

    private readonly Dictionary<EnAudioLayer, AudioLayer> _AudioLayerMap = new();
    private readonly Dictionary<int, AudioData> _AudioPlayableInfoMap = new();

    public PlayableGraph _Graph;
    public AudioMixerPlayable _AudioMixer;

    private readonly List<int> _UpdateClipInfos = new();
    private int _CurMaxWeight;


    public override void Destroy()
    {
        if (_Graph.IsValid())
            _Graph.Destroy();
        if (_AudioSource != null)
            GameObject.Destroy(_AudioSource.gameObject);
        _AudioSource = null;
        _LastClipId = -1;
        _LastPlayTime = -1;
        _UpdateClipInfos.Clear();
    }
    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();
        if (_AudioSource != null)
            return;
        var go = new GameObject("Audio Source");
        //go.hideFlags |= HideFlags.HideAndDontSave;
        GameObject.DontDestroyOnLoad(go);
        _AudioSource = go.AddComponent<AudioSource>();
        _AudioSource.playOnAwake = false;
        _AudioSource.loop = false;


        _Graph = PlayableGraph.Create("Audio Manager");
        var output = AudioPlayableOutput.Create(_Graph, "audio manager", _AudioSource);
        var inputCount = GameSchedule.Instance.GetAudioLayerCfgCount();
        _AudioMixer = AudioMixerPlayable.Create(_Graph, inputCount);
        output.SetSourcePlayable(_AudioMixer);
        _Graph.SetTimeUpdateMode(DirectorUpdateMode.UnscaledGameTime);
        _Graph.Play();
    }

    public AudioData GetAudioPlayableInfo(int key)
    {
        return _AudioPlayableInfoMap[key];
    }
    public void RemoveAudioPlayableInfo(int key)
    {
        var info = _AudioPlayableInfoMap[key];
        ABBLoadMgr.Instance.Unload(info.clipLoadHandle);
        ClassPoolMgr.Instance.Push(info);
        _AudioPlayableInfoMap.Remove(key);
    }

    public override void Update()
    {
        base.Update();

        foreach (var (layer, data) in _AudioLayerMap)
        {
            //var data = AudioMgr.Instance.GetAudioLayer(layerInfo.Value);
            data.UpdateWeight();

            var layerCfg = GameSchedule.Instance.GetAudioLayerCfg0(data.Layer);
            var curWeight = _AudioMixer.GetInputWeight((int)layer - 1);
            var toWeight = (float)layerCfg.nWeight / _CurMaxWeight;
            var weight = Mathf.Lerp(curWeight, toWeight, GameUtility.GetGameTime() * 10);
            _AudioMixer.SetInputWeight((int)layer - 1, weight);
        }
        for (int i = 0; i < _UpdateClipInfos.Count; i++)
        {
            var clipKey = _UpdateClipInfos[i];
            var clipData = AudioMgr.Instance.GetAudioPlayableInfo(clipKey);
            var clipCfg = GameSchedule.Instance.GetAudioCfg0(clipData.clipID);

            if (clipData.startTime + clipCfg.fLength < Time.time)
            {
                var layerData = _AudioLayerMap[clipData.layer];
                layerData.DisconnectPort(clipData.inputPort);

                if (layerData.Count == 0)
                {
                    _AudioLayerMap.Remove(clipData.layer);
                    ClassPoolMgr.Instance.Push(layerData);

                    _CurMaxWeight = 1;
                    foreach (var (layer, _) in _AudioLayerMap)
                    {
                        var layerCfg = GameSchedule.Instance.GetAudioLayerCfg0((int)layer);
                        _CurMaxWeight = Mathf.Max(_CurMaxWeight, layerCfg.nWeight);
                    }
                }
                RemoveAudioPlayableInfo(clipKey);
                _UpdateClipInfos.RemoveAt(i--);

            }
        }
    }
    public void Stop()
    {

    }
    public void Play(EnAudioId audioID)
    {
        var time = GameUtility.GetGameTime();
        Play((int)audioID, time).Forget();
    }
    public void Play(int audioID)
    {
        var time = GameUtility.GetGameTime();
        Play(audioID, time).Forget();
    }
    public void AddAudio(EnAudioId audioID, float startTime)
    {
        Play((int)audioID, startTime).Forget();
    }
    private int _LastClipId;
    private float _LastPlayTime;
    public async UniTask Play(int audioID, float startTime)
    {
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
#if UNITY_WEBGL
            return;
#endif
        }
        var time = GameUtility.GetGameTime();
        if (audioID == _LastClipId && time - _LastPlayTime < 0.1f)
            return;
        _LastClipId = audioID;
        _LastPlayTime = time;

        var audioCfg = GameSchedule.Instance.GetAudioCfg0(audioID);

        var (isCancel, loadHandle) = await ABBLoadMgr.Instance.LoadAsync<AudioClip>((EnLoadTarget)audioCfg.nAssetID, GetToken())
            .SuppressCancellationThrow();
        if (isCancel)
            return;

        //return;
        var layer = (EnAudioLayer)audioCfg.nLayer;
        if (!_AudioLayerMap.TryGetValue(layer, out var data))
        {
            var layerCfg = GameSchedule.Instance.GetAudioLayerCfg0(audioCfg.nLayer);
            _CurMaxWeight = Mathf.Max(_CurMaxWeight, layerCfg.nWeight);

            data = ClassPoolMgr.Instance.Pull<AudioLayer>();
            _AudioLayerMap.Add(layer, data);
            var weight = (float)layerCfg.nWeight / _CurMaxWeight;
            data.Init(_Graph, _AudioMixer, audioCfg.nLayer, weight);


        }
        {
            var audioInfoKey = ABBUtil.GetTempKey();
            var info = ClassPoolMgr.Instance.Pull<AudioData>();
            _AudioPlayableInfoMap.Add(audioInfoKey, info);

            if (layer == EnAudioLayer.Bgm)
            {
                if (data.Count > 0)
                {
                    data.DisconnectAllPort();
                }
            }

            var audioClip = loadHandle.GetObject<AudioClip>();
            var inputPort = data.Connect(audioClip, startTime, audioCfg.bIsLoop > 0);
            info.clipID = audioCfg.nAudioID;
            info.inputPort = inputPort;
            info.layer = layer;
            info.startTime = startTime;
            info.clipLoadHandle = loadHandle;


            if (audioCfg.bIsLoop <= 0)
            {
                _UpdateClipInfos.Add(audioInfoKey);
            }
        }
    }
}