using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestHelper
{
    /// <summary>
    /// 发送 WebRequest 的安全异步版本（WebGL 兼容）
    /// </summary>
    public static UniTask<UnityWebRequest> SendWebRequestSafeAsync(
        this UnityWebRequest request,
        MonoBehaviour coroutineRunner,
        CancellationToken cancellationToken = default)
    {
        // 非 WebGL 平台，直接使用 Task.Run 或 async 包装（保持原逻辑，避免破坏其他平台）
        // 注意：WebGL 不支持 Task.Run
        if (!AotPlatformUtility.IsWebGL)
        {
            return SendNativeAsync(request, cancellationToken);
        }

        // WebGL 平台：使用 Coroutine + TaskCompletionSource 桥接
        var tcs = new TaskCompletionSource<UnityWebRequest>();

        // 监听取消信号
        cancellationToken.Register(() =>
        {
            request.Abort(); // 取消请求
            tcs.TrySetCanceled(cancellationToken);
        });

        // 在 MonoBehaviour 上启动协程
        coroutineRunner.StartCoroutine(WebRequestCoroutine(request, tcs));

        return tcs.Task.AsUniTask();
    }

    // 原生异步实现（非 WebGL 使用）
    private static async UniTask<UnityWebRequest> SendNativeAsync(
        UnityWebRequest request,
        CancellationToken cancellationToken)
    {
        var operation = request.SendWebRequest();
        while (!operation.isDone)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                request.Abort();
                cancellationToken.ThrowIfCancellationRequested();
            }
            await UniTask.Yield(); // 避免死循环
        }

        // 处理错误
        if (request.result != UnityWebRequest.Result.Success)
        {
            throw new Exception($"Request failed: {request.error}");
        }

        return request;
    }

    // WebGL 专用协程
    private static System.Collections.IEnumerator WebRequestCoroutine(
        UnityWebRequest request,
        TaskCompletionSource<UnityWebRequest> tcs)
    {
        yield return request.SendWebRequest();

        // 如果已经被取消，直接返回
        if (tcs.Task.IsCanceled)
            yield break;

        // 处理完成结果
        if (request.result != UnityWebRequest.Result.Success)
        {
            tcs.TrySetException(new Exception($"WebRequest failed: {request.error}"));
        }
        else
        {
            tcs.TrySetResult(request);
        }
    }
}