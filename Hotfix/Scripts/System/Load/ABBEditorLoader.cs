
#if UNITY_EDITOR

using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System;
using Object = UnityEngine.Object;

public class ABBEditorLoader : IABBAssetLoader2
{
    private readonly Dictionary<int, AssetCfg> _AssetDic = new();
    public async UniTask Initialization()
    {
        var path = GameSchedule.GetCfgPath<AssetCfg>();
        var file = await File.ReadAllTextAsync(path);
        var assetCfgs = JsonConvert.DeserializeObject<AssetCfg[]>(file);
        for (int i = 0; i < assetCfgs.Length; i++)
        {
            var assetCfg = assetCfgs[i];
            _AssetDic.Add(assetCfg.nAssetID, assetCfg);
        }
    }

    public void Destroy()
    {
        _AssetDic.Clear();
    }

    public async UniTask<T> LoadAssetAsync<T>(int assetID, CancellationToken token) where T : Object
    {
        var cfg = _AssetDic[assetID];
        if (typeof(T) != typeof(AnimationClip) && typeof(T) != typeof(AudioClip))
        {
            var delay = ABBUtil.GetRange(-200, 200);
            var isCancel = await UniTask.Delay(Mathf.Max(0, delay), true, cancellationToken: token, cancelImmediately: true)
                .SuppressCancellationThrow();
            if (isCancel)
                throw new OperationCanceledException();
        }
        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(cfg.strPath);
        if (obj == null)
            return null;
        return obj;
    }

    public void UnloadAsset(in int assetID)
    {
        return;
    }

    public T LoadAsset<T>(int assetID) where T : Object
    {
        var cfg = _AssetDic[assetID];
        var obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(cfg.strPath);
        if (obj == null)
            return null;
        return obj;
    }
}
#endif