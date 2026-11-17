
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;


public interface IABBAssetLoader
{
    public UniTask Initialization();
    public void Destroy();
    public UniTask<T> LoadAssetAsync<T>(int assetID, CancellationToken token)
        where T : Object;

    public T LoadAsset<T>(int assetID)
        where T : Object;
    public void UnloadAsset(in int assetID);
    public void Log();
}