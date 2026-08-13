using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class WebUtil
{
    public static async Task<byte[]> DownloadFileAsync(string localPath)
    {
        // ✅ 使用 Uri 拼接，避免 Path.Combine 的斜杠问题
        var downLoadUrl = GlobalConfigSO.Instance.GetDownLoadUrlPath();
        var url = Path.Combine(downLoadUrl, localPath).Replace("\\","/");
        var uri = new Uri(url);

        Debug.Log($"开始下载 -> {uri}");

        try
        {
            using var webRep = UnityWebRequest.Get(uri);
            webRep.timeout = 30;

            // ✅ 关键改动：调用我们封装的安全方法，并传入 this (MonoBehaviour)
            var resultRequest = await webRep.SendWebRequestSafeAsync(GameStart.Instance);

            // 此时请求已经完成，且不会在 WebGL 卡死
            Debug.Log($"下载完成 {resultRequest.downloadHandler.data.Length} -> {uri}");
            return resultRequest.downloadHandler.data;
        }
        catch (OperationCanceledException)
        {
            Debug.LogWarning($"下载取消 -> {uri}");
            return new byte[0];
        }
        catch (Exception ex)
        {
            Debug.LogError($"下载失败 {uri}, {ex}");
            return new byte[0];
        }
    }
    //public static async UniTask<byte[]> DownloadFileAsync(string localPath)
    //{
    //    //var url = Path.Combine(GlobalConfigSO.Instance.DownloadRootUrl, localPath).Replace("\\","/");
    //    //var uri = new Uri(url);
    //    //using var webRep = UnityWebRequest.Get(uri);

    //    //var reqOperation = webRep.SendWebRequest();

    //    //try
    //    //{
    //    //    Debug.Log($"开始下载 -> {url}");
    //    //    var result = await reqOperation;
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    Debug.LogError($"url: {url} \n{ex}");
    //    //}

    //    //if (webRep.result != UnityWebRequest.Result.Success)
    //    //{
    //    //    Debug.LogError($"下载失败 {url}, {webRep.error}");
    //    //    return new byte[0];
    //    //}
    //    //Debug.Log($"下载完成 -> {url}");
    //    //return webRep.downloadHandler.data;
    //}
    public static async UniTask<bool> UploadContent(string uploadLocalPath, byte[] data)
    {
        var path = Path.Combine(GlobalConfigSO.Instance.ServerDataPath, uploadLocalPath);
        Debug.Log($"上传成功: {path}");
        var dir = Path.GetDirectoryName(path);
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        await File.WriteAllBytesAsync(path, data);
        return true;
    }
}
