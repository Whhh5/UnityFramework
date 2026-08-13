using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class LoadData : IClassPoolDestroy
{
    public Object obj;
    public int loadTarget;
    public void OnPoolDestroy()
    {
        obj = null;
        loadTarget = -1;
    }
}

public struct LoadHandle
{
    public int loadId;
    public LoadHandle(int loadId)
    {
        this.loadId = loadId;
    }

    public bool IsValid()
    {
        return ABBLoadMgr.Instance.Contains(loadId);
    }

    public readonly T GetObject<T>()
        where T : Object
    {
        var obj = ABBLoadMgr.Instance.GetObject<T>(loadId);
        return obj;
    }

    public static implicit operator int(LoadHandle handler)
    {
        return handler.loadId;
    }
    public static implicit operator LoadHandle(int handlerId)
    {
        return new(handlerId);
    }
}

public class ABBLoadMgr : Singleton<ABBLoadMgr>
{
    public override EnSingletonOrder SingletonOrder => EnSingletonOrder.Load;
    private readonly Dictionary<EnLoaderType, IABBAssetLoader2> _LoaderList = new();
    private readonly Dictionary<int, LoadData> _LoadDatas = new();

    public override async UniTask OnEnableAsync()
    {
        await base.OnEnableAsync();
    }
    public override void Destroy()
    {
        foreach (var item in _LoaderList)
        {
            item.Value.Destroy();
        }

#if UNITY_EDITOR
        UnregisterLoader<ABBEditorLoader>(EnLoaderType.Editor);
#endif
        UnregisterLoader<AbbAssetbundleLoader>(EnLoaderType.Assetbundle);
        base.Destroy();
    }
    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();
#if UNITY_EDITOR
        RegisterLoader(EnLoaderType.Editor, new ABBEditorLoader());
#endif
        RegisterLoader(EnLoaderType.Assetbundle, new AbbAssetbundleLoader());

        var curLoader = GetLoader();
        await curLoader.Initialization();
    }
    private void RegisterLoader(EnLoaderType loadType, IABBAssetLoader2 loader)
    {
        _LoaderList.Add(loadType, loader);
    }
    private void UnregisterLoader<T>(EnLoaderType loadType)
    {
        _LoaderList.Remove(loadType);
    }

    private IABBAssetLoader2 GetLoader(EnLoaderType loadType)
    {
        return _LoaderList[loadType];
    }
    private IABBAssetLoader2 GetLoader()
    {
        return GetLoader(GlobalConfigSO.Instance.LoaderType);
    }
    public async UniTask<LoadHandle> LoadAsync<T>(EnLoadTarget loadTarget, CancellationToken token)
        where T : Object
    {
        var (isCancel, handle) = await LoadAsync<T>((int)loadTarget, token).SuppressCancellationThrow();
        if (isCancel)
            throw new OperationCanceledException();
        return handle;
    }
    private async UniTask<LoadHandle> LoadAsync<T>(int assetCfgID, CancellationToken token)
        where T : Object
    {
        var loader = GetLoader();
        var (isCancel, result) = await loader.LoadAssetAsync<T>(assetCfgID, token)
            .SuppressCancellationThrow();
        if (isCancel)
            throw new OperationCanceledException();
        var loadKey = ABBUtil.GetTempKey();
        var loadData = ClassPoolMgr.Instance.Pull<LoadData>();
        loadData.loadTarget = assetCfgID;
        loadData.obj = result;
        _LoadDatas.Add(loadKey, loadData);
        return new LoadHandle(loadKey);
    }

    public T Load<T>(int assetID)
           where T : Object
    {
        var loader = GetLoader();
        var result = loader.LoadAsset<T>(assetID);
        return result;
    }
    public T Load<T>(EnLoadTarget loadTarget)
           where T : Object
    {
        var result = Load<T>((int)loadTarget);
        return result;
    }
    //public void Unload(EnLoadTarget loadTarget)
    //{
    //    Unload((int)loadTarget);
    //}
    public void Unload(LoadHandle loadId)
    {
        if (!_LoadDatas.TryGetValue(loadId, out var loadData))
            return;
        var loadTarget = loadData.loadTarget;
        _LoadDatas.Remove(loadId);
        ClassPoolMgr.Instance.Push(loadData);

        var loader = GetLoader();
        loader.UnloadAsset(in loadTarget);
    }
    public T GetObject<T>(in int id)
        where T: Object
    {
        if (!_LoadDatas.TryGetValue(id, out var loadData))
            return null;
        return loadData.obj as T;
    }
    public bool Contains(in int id)
    {
        if (!_LoadDatas.ContainsKey(id))
            return false;
        return true;
    }
}
