using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;



public class AssetbundleInfo
{
    public int abID;
    public string abName;
    public int[] abAssetIDs;
    public int[] dependAbList;
}
public class AssetInfo
{
    public int assetID;
    public int abID;
    public string addressableName;
    public string path;
}

public class RuntimeAbInfo : IClassPoolDestroy
{
    public int infoID;
    public int refCount;
    public AssetBundle ab;
    public CancellationTokenSource tokenSource;
    public AssetBundleCreateRequest abRequest;
    public EnLoadStatus status;

    public void AddRefCount()
    {
        refCount++;
    }
    public void RemoveRefCount()
    {
        refCount--;
    }
    public bool IsUnLoad()
    {
        return refCount <= 0;
    }
    public void OnPoolDestroy()
    {
        infoID = -1;
        refCount = 0;
        ab = null;
        tokenSource = null;
        abRequest = null;
        status = EnLoadStatus.None;
    }
}
public class RuntimeAssetInfo : IClassPoolDestroy
{
    public int infoID;
    public int assetID;
    public int refCount;
    public Object obj;
    public CancellationTokenSource tokenSource;

    public void AddRefCount()
    {
        refCount++;
    }
    public void RemoveRefCount()
    {
        refCount--;
    }
    public void OnPoolDestroy()
    {
        refCount = 0;
        infoID = -1;
        assetID = 0;
        obj = null;
        tokenSource = null;
    }
    public bool IsUnload()
    {
        return refCount <= 0;
    }
}

public class AbbAssetbundleLoader : IABBAssetLoader
{
    private Dictionary<int, AssetbundleInfo> _AbInfos = new();
    private Dictionary<int, AssetInfo> _AssetInfos = new();
    private Dictionary<int, string> _AbPathMap = new();
    private Dictionary<int, int> _AssetCfgID2AssetId = new();

    // runtime
    private Dictionary<int, RuntimeAssetInfo> _RuntimeAssetInfos = new();
    private Dictionary<int, RuntimeAbInfo> _RuntimeAbInfos = new();


    public void Log()
    {
        var stringBuilder = new StringBuilder(1000);
        stringBuilder.AppendLine($"abInfo = {{");
        foreach (var abInfo in _RuntimeAbInfos)
        {
            stringBuilder.AppendLine($"\t ID:{abInfo.Key}");
            stringBuilder.AppendLine($"\t {{");
            stringBuilder.AppendLine($"\t\t refCount:{abInfo.Value.refCount}");
            stringBuilder.AppendLine($"\t\t ab:{abInfo.Value.ab}");
            stringBuilder.AppendLine($"\t\t state:{abInfo.Value.abRequest.progress}");
            stringBuilder.AppendLine($"\t\t token:{abInfo.Value.tokenSource}");
            stringBuilder.AppendLine($"\t }}");
        }
        stringBuilder.AppendLine($"}}");

        stringBuilder.AppendLine($"");
        stringBuilder.AppendLine($"");
        stringBuilder.AppendLine($"assetInfo = {{");
        foreach (var rtInfo in _RuntimeAssetInfos)
        {
            if (!_AssetInfos.TryGetValue(rtInfo.Key, out var assetInfo))
                return;
            stringBuilder.AppendLine($"\t ID:{rtInfo.Key}");
            stringBuilder.AppendLine($"\t {{");
            stringBuilder.AppendLine($"\t\t ab:{assetInfo.abID}");
            stringBuilder.AppendLine($"\t\t refCount:{rtInfo.Value.refCount}");
            stringBuilder.AppendLine($"\t\t assetID:{(EnLoadTarget)rtInfo.Value.assetID}");
            stringBuilder.AppendLine($"\t\t obj:{rtInfo.Value.obj.name}");
            stringBuilder.AppendLine($"\t\t token:{rtInfo.Value.tokenSource}");
            stringBuilder.AppendLine($"\t }}");
        }
        stringBuilder.AppendLine($"}}");
        AotDebug.LogWarning(stringBuilder);
    }
    public void Destroy()
    {
        _AbInfos.Clear();
        _AssetInfos.Clear();
        _RuntimeAbInfos.Clear();
        _RuntimeAssetInfos.Clear();
    }

    public static Dictionary<int, AssetInfo> CreateAssetInfos()
    {
        var assetInfos = new Dictionary<int, AssetInfo>();
        var assetInfoPath = AssetbundleUtil.GetCurPlatformAssetInfoFilePath();
        var assetInfoStr = File.ReadAllText(assetInfoPath, Encoding.UTF8);
        var assetInfoList = JsonConvert.DeserializeObject<List<AssetInfo>>(assetInfoStr);

        for (int i = 0; i < assetInfoList.Count; i++)
        {
            var assetInfo = assetInfoList[i];
            assetInfos.Add(assetInfo.assetID, assetInfo);
        }


        return assetInfos;
    }

    public async UniTask Initialization()
    {
        var abInfoPath = AssetbundleUtil.GetCurPlatformAbInfoFilePath();
        var abInfoStr = File.ReadAllText(abInfoPath, Encoding.UTF8);
        var abInfoList = JsonConvert.DeserializeObject<List<AssetbundleInfo>>(abInfoStr);

        var abRoot = AssetbundleUtil.GetCurPlatformAssetbundleRoot();
        var testDic = new Dictionary<string, int>();
        for (int i = 0; i < abInfoList.Count; i++)
        {
            var abInfo = abInfoList[i];
            _AbInfos.Add(abInfo.abID, abInfo);
            var value = Path.Combine(abRoot, abInfo.abName);
            _AbPathMap.Add(abInfo.abID, value);
            if (testDic.TryGetValue(value, out var key))
            {
                AotDebug.LogError($"ab value 重复 key1:{key} key2:{abInfo.abID}-{value}");
            }
            else
            {
                testDic.Add(value, abInfo.abID);
            }
        }


        var assetInfoPath = AssetbundleUtil.GetCurPlatformAssetInfoFilePath();
        var assetInfoStr = File.ReadAllText(assetInfoPath, Encoding.UTF8);
        var assetInfoList = JsonConvert.DeserializeObject<List<AssetInfo>>(assetInfoStr);

        for (int i = 0; i < assetInfoList.Count; i++)
        {
            var assetInfo = assetInfoList[i];
            _AssetInfos.Add(assetInfo.assetID, assetInfo);
        }
        await UniTask.Delay(0);
    }

    //public async UniTask<T> LoadAssetAsync<T>(int assetID, CancellationToken token) where T : Object
    //{
    //    if (!await LoadAbAndAssetAsync<T>(assetID, token))
    //        return null;
    //    if (!_RuntimeAssetInfos.TryGetValue(assetID, out var runtimeAssetInfo))
    //        return null;

    //    //runtimeAssetInfo.AddRefCount();
    //    if (runtimeAssetInfo.obj is not T)
    //    {
    //        ABBUtil.LogError($"asset type error {runtimeAssetInfo.obj} not {typeof(T)}");
    //        UnloadAsset(assetID);
    //        return null;
    //    }
    //    return runtimeAssetInfo.obj as T;
    //}

    public T LoadAsset<T>(int assetID) where T : Object
    {
        throw new Exception($"加载超时 -> {assetID}");
    }

    public void UnloadAsset(in int assetID)
    {
        if (!_RuntimeAssetInfos.TryGetValue(assetID, out var runtimeAssetInfo))
            return;
        runtimeAssetInfo.RemoveRefCount();
        if (runtimeAssetInfo.IsUnload())
            UnloadAsset(assetID);
    }
    public async UniTask<T> LoadAssetAsync<T>(int assetID, CancellationToken token)
        where T : Object
    {
        if (_RuntimeAssetInfos.TryGetValue(assetID, out var runtimeAssetInfo))
        {
            runtimeAssetInfo.AddRefCount();
            if (runtimeAssetInfo.tokenSource != null)
            {
                var curInfoID = runtimeAssetInfo.infoID;
                var isCancel2 = await UniTask.WaitUntil(() =>
                {
                    return curInfoID != runtimeAssetInfo.infoID || runtimeAssetInfo.tokenSource == null;

                }, cancellationToken: token, cancelImmediately: true)
                    .SuppressCancellationThrow();
                if (isCancel2)
                {
                    throw new OperationCanceledException();
                }
                if (curInfoID != runtimeAssetInfo.infoID)
                {
                    return null;
                }
            }
            return runtimeAssetInfo.obj as T;
        }

        if (!_AssetInfos.TryGetValue(assetID, out var assetInfo))
            return null;

        runtimeAssetInfo = ClassPoolMgr.Instance.Pull<RuntimeAssetInfo>();
        runtimeAssetInfo.tokenSource = new CancellationTokenSource();
        var infoID = ABBUtil.GetTempKey();
        runtimeAssetInfo.infoID = infoID;
        runtimeAssetInfo.assetID = assetID;
        runtimeAssetInfo.AddRefCount();
        _RuntimeAssetInfos.Add(assetID, runtimeAssetInfo);
        var (isCancel, ab) = await LoadAssetbundleAsync(assetInfo.abID, runtimeAssetInfo.tokenSource.Token).SuppressCancellationThrow();
        if (isCancel)
        {
            throw new OperationCanceledException();
        }
        if (infoID != runtimeAssetInfo.infoID)
        {
            UnloadAsset(assetID);
            return null;
        }
        //ABBUtil.Log($"加载 asset - {assetID} - {assetInfo.path} - {assetInfo.addressableName}");
        var abRequest = ab.LoadAssetAsync<T>(assetInfo.addressableName);
        var (isCancel3, asset) = await abRequest.ToUniTask(cancellationToken: runtimeAssetInfo.tokenSource.Token).SuppressCancellationThrow();
        if (isCancel3)
        {
            throw new OperationCanceledException();
        }
        //var isCancel4 = await UniTask.DelayFrame(1, PlayerLoopTiming.Update, token, true).SuppressCancellationThrow();
        //if (isCancel4)
        //{
        //    AotDebug.LogError("cancel 11");
        //    throw new OperationCanceledException();
        //}
        if (infoID != runtimeAssetInfo.infoID)
        {
            return null;
        }
        runtimeAssetInfo.tokenSource.Dispose();
        runtimeAssetInfo.tokenSource = null;
        if (asset == null)
        {
            UnloadAsset(assetID);
            return null;
        }
        runtimeAssetInfo.obj = asset;
        return asset as T;
    }

    private void UnloadAsset(int assetID)
    {
        if (!_RuntimeAssetInfos.TryGetValue(assetID, out var runtimeAssetInfo))
            return;
        if (!_AssetInfos.TryGetValue(assetID, out var assetInfo))
            return;
        if (!_AbInfos.TryGetValue(assetInfo.abID, out var abInfo))
            return;
        if (!_RuntimeAbInfos.TryGetValue(assetInfo.abID, out var runtimeAbInfo))
            return;

        if (runtimeAssetInfo.tokenSource != null && !runtimeAssetInfo.tokenSource.IsCancellationRequested)
        {
            runtimeAssetInfo.tokenSource.Cancel();
            runtimeAssetInfo.tokenSource.Dispose();
            runtimeAssetInfo.tokenSource = null;
        }

        //ABBUtil.Log($"卸载 asset - {assetID} - {runtimeAssetInfo.obj}");
        _RuntimeAssetInfos.Remove(assetID);
        ClassPoolMgr.Instance.Push(runtimeAssetInfo);

        runtimeAbInfo.RemoveRefCount();
        if (runtimeAbInfo.IsUnLoad())
        {
            UnloadAssetbundle(in abInfo.abID);
        }

    }

    private void UnloadAssetbundle(in int abID)
    {
        if (!_AbInfos.TryGetValue(abID, out var abInfo))
            return;
        if (!_RuntimeAbInfos.TryGetValue(abID, out var runtimeAbInfo))
            return;
        if (runtimeAbInfo.tokenSource != null && !runtimeAbInfo.tokenSource.IsCancellationRequested)
        {
            runtimeAbInfo.tokenSource.Cancel();
            runtimeAbInfo.tokenSource.Dispose();
            runtimeAbInfo.tokenSource = null;
            AotDebug.LogError("UnloadAssetbundle");

            if (runtimeAbInfo.abRequest != null && runtimeAbInfo.abRequest.assetBundle != null)
            {
                runtimeAbInfo.abRequest.assetBundle.Unload(true);
                runtimeAbInfo.abRequest = null;
            }
        }
        else
        {
            runtimeAbInfo.ab.Unload(true);
        }

        //ABBUtil.Log($"卸载 ab - {abID}");
        _RuntimeAbInfos.Remove(abID);
        ClassPoolMgr.Instance.Push(runtimeAbInfo);

        foreach (var depAbID in abInfo.dependAbList)
        {
            if (!_RuntimeAbInfos.TryGetValue(depAbID, out var depAbInfo))
                continue;
            depAbInfo.RemoveRefCount();
            if (depAbInfo.IsUnLoad())
                UnloadAssetbundle(in depAbID);
        }

    }
    private async UniTask<AssetBundle> LoadAssetbundleAsync(int abID, CancellationToken token)
    {
        if (_RuntimeAbInfos.TryGetValue(abID, out var runtimeAbInfo))
        {
            var curInfoId = runtimeAbInfo.infoID;
            runtimeAbInfo.AddRefCount();
            if (runtimeAbInfo.tokenSource != null)
            {
                var isCancel = await UniTask.WaitUntil(() => runtimeAbInfo.tokenSource == null, cancelImmediately: true, cancellationToken: token)
                    .SuppressCancellationThrow();
                if (isCancel)
                {
                    throw new OperationCanceledException();
                }
            }
            return curInfoId == runtimeAbInfo.infoID ? runtimeAbInfo.ab : null;
        }
        if (!_AbInfos.TryGetValue(abID, out var abInfo))
            return null;
        //ABBUtil.Log($"请求加载 ab - {abID}");

        runtimeAbInfo = ClassPoolMgr.Instance.Pull<RuntimeAbInfo>();
        runtimeAbInfo.tokenSource = new CancellationTokenSource();
        runtimeAbInfo.infoID = ABBUtil.GetTempKey();
        runtimeAbInfo.AddRefCount();
        _RuntimeAbInfos.Add(abID, runtimeAbInfo);

        //await UniTask.Delay(500, true, PlayerLoopTiming.Update, token, true);

        var unitaskList = new List<UniTask>();
        for (int i = 0; i < abInfo.dependAbList.Length; i++)
        {
            var depAbID = abInfo.dependAbList[i];
            var unitask = LoadAssetbundleAsync(depAbID, token);
            //if (_RuntimeAbInfos.TryGetValue(depAbID, out var depAbInfo))
            //    depAbInfo.AddRefCount();
            unitaskList.Add(unitask);
        }

        var abPath = _AbPathMap[abID];
        //AotDebug.LogWarning($"load1 ab {abID}->{abPath}");
        var abFileHandle = AssetBundle.LoadFromFileAsync(abPath); ;
        var abLoadHandle = abFileHandle.ToUniTask(cancellationToken: runtimeAbInfo.tokenSource.Token, cancelImmediately: true);
        unitaskList.Add(abLoadHandle);
        runtimeAbInfo.abRequest = abFileHandle;
        try
        {
            await UniTask.WhenAll(unitaskList);
            runtimeAbInfo.ab = abFileHandle.assetBundle;
            runtimeAbInfo.tokenSource.Dispose();
            runtimeAbInfo.tokenSource = null;
        }
        catch (OperationCanceledException ex)
        {
            AotDebug.Log($"load cancel {ex.Message}"); 
            throw new OperationCanceledException();
        }

        return abFileHandle.assetBundle;
    }
    //public void LoadExcelAsync(EnLoadTarget loadTarget)
    //{
    //    AssetbundleUtil.GetAbCatalogLocalPath
    //}
}