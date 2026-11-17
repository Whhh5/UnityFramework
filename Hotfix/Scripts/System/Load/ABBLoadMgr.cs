using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;



public class ABBLoadMgr : Singleton<ABBLoadMgr>
{
    private class LoadData : IClassPoolDestroy
    {
        public Object obj;
        public int loadTarget;
        public void OnPoolDestroy()
        {
            obj = null;
            loadTarget = -1;
        }
    }
    public override EnSingletonOrder SingletonOrder => EnSingletonOrder.Load;
    private IABBAssetLoader _Loader = null;
    private readonly Dictionary<int, LoadData> _LoadDatas = new();

    public override async UniTask OnEnableAsync()
    {
        await base.OnEnableAsync();
    }
    public override void Destroy()
    {
        _Loader.Destroy();

        base.Destroy();
        _Loader = null;
    }
    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();

        _Loader = GlobalConfigSO.Instance.LoaderType switch
        {
#if UNITY_EDITOR
            EnLoaderType.Editor => new ABBEditorLoader(),
#endif
            _ => new AbbAssetbundleLoader(),
        };
        await _Loader.Initialization();
    }
    public IABBAssetLoader GetLoader()
    {
        return _Loader;
    }
    public async UniTask<LoadHandle> LoadAsync<T>(EnLoadTarget loadTarget, CancellationToken token)
        where T : Object
    {
        var (isCancel, handle) = await LoadAsync<T>((int)loadTarget, token).SuppressCancellationThrow();
        if (isCancel)
        {
            throw new OperationCanceledException();
        }
        return handle;
    }
    private async UniTask<LoadHandle> LoadAsync<T>(int assetCfgID, CancellationToken token)
        where T : Object
    {
        var loader = GetLoader();
        var (isCancel, result) = await loader.LoadAssetAsync<T>(assetCfgID, token)
            .SuppressCancellationThrow();
        if (isCancel)
        {
            throw new OperationCanceledException();
        }
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
