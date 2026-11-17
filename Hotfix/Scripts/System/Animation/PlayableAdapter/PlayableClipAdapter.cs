using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class PlayableClipAdapterUserData : PlayableAdapterUserData
{
    public int clipID = -1;
    public override void OnPoolDestroy()
    {
        base.OnPoolDestroy();
        clipID = -1;
    }
}
public class PlayableClipAdapter : PlayableAdapter<PlayableClipAdapterUserData>
{
    private AnimationClipPlayable _ClipPlayable;
    private int _ClipID = -1;
    private float _ClipLength = -1;
    private EnAnimLayer _AnimLayer = EnAnimLayer.None;

    private float _StartTime;
    private CancellationTokenSource _LoadToken;
    private LoadHandle _ClipLoadHandle;

    public override void OnPoolDestroy()
    {
        _LoadToken?.Cancel();
        if (_ClipLoadHandle.IsValid())
            ABBLoadMgr.Instance.Unload(_ClipLoadHandle);
        if (_ClipPlayable.IsValid())
            _ClipPlayable.Destroy();
        AnimMgr.Instance.RecycleClip(_ClipID);
        base.OnPoolDestroy();
        _ClipID = -1;
        _ClipLength = -1;
        _StartTime = -1;
        _LoadToken = null;
        _ClipLoadHandle = default;
    }
    public override void OnPoolInit(PlayableClipAdapterUserData userData)
    {
        base.OnPoolInit(userData);

        _StartTime = GameUtility.GetGameTime();
        _ClipID = userData.clipID;
        var clipCfg = GameSchedule.Instance.GetClipCfg0(_ClipID);
        _AnimLayer = (EnAnimLayer)clipCfg.nLayer;
        _ClipLength = clipCfg.fLength;

        LoadClipAsync(userData.clipID);
    }
    private async void LoadClipAsync(int clipId)
    {
        _LoadToken = new();
        var clipCfg = GameSchedule.Instance.GetClipCfg0(clipId);
        var (isCancel, handle) = await ABBLoadMgr.Instance.LoadAsync<AnimationClip>((EnLoadTarget)clipCfg.nAssetID, _LoadToken.Token)
            .SuppressCancellationThrow();

        if (!isCancel)
        {

            if (_LoadToken == null)
            {
                if (_ClipLoadHandle.IsValid())
                    ABBLoadMgr.Instance.Unload(_ClipLoadHandle);
                return;
            }


            _ClipLoadHandle = handle;
            await UniTask.SwitchToMainThread(cancellationToken: _LoadToken.Token);


            var clip = handle.GetObject<AnimationClip>();
            _ClipPlayable = AnimationClipPlayable.Create(_Graph.GetGraph(), clip);
            _ClipPlayable.SetApplyFootIK(false);
            _ClipPlayable.SetApplyPlayableIK(false);
            AddConnectRootAdapter(_ClipPlayable, GlobalConfig.Int0, GlobalConfig.Float1);
            var time = GetPlayTime();
            _ClipPlayable.SetTime(time);
            _LoadToken = null;

        }
    }
    public override float GetPlayTime()
    {
        var curTime = GameUtility.GetGameTime();
        return curTime - _StartTime;
    }
    public override float GetUnitTime()
    {
        return _ClipLength;
    }

    public override EnAnimLayer GetOutputLayer()
    {
        return _AnimLayer;
    }
}
