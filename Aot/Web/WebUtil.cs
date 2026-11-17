using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

public static class WebUtil
{
    public static async UniTask<byte[]> DownloadFileAsync(string downLoadUrl, string localPath)
    {
        var url = Path.Combine(downLoadUrl, localPath).Replace("\\", "/");
        var uri = new Uri(url);

        AotDebug.Log($"开始下载 -> {uri}");

        var webRep = UnityWebRequest.Get(uri);
        webRep.timeout = 60;
        webRep.SetRequestHeader("ngrok-skip-browser-warning", "1");
        try
        {

            // ✅ 关键改动：调用我们封装的安全方法，并传入 this (MonoBehaviour)
            var resultRequest = await webRep.SendWebRequestSafeAsync(GameStart.Instance);

            // 此时请求已经完成，且不会在 WebGL 卡死
            AotDebug.Log($"下载完成 {resultRequest.downloadHandler.data.Length} -> {uri}");
            return resultRequest.downloadHandler.data ?? new byte[0];
        }
        catch (OperationCanceledException)
        {
            AotDebug.LogWarning($"下载取消 -> {uri}");
            return new byte[0];
        }
        catch (Exception ex)
        {
            AotDebug.LogError($"下载失败 {uri}, {ex}");
            return new byte[0];
        }
        finally
        {
            webRep.Dispose();
        }
    }
    public static async UniTask<byte[]> DownloadStaticAssetFileAsync(string localPath)
    {
        // ✅ 使用 Uri 拼接，避免 Path.Combine 的斜杠问题
        var downLoadUrl = GlobalConfigSO.Instance.GetProjectStaticWebAssetUrl();
        var datas = await DownloadFileAsync(downLoadUrl, localPath);
        return datas;
    }
    public static async UniTask<byte[]> DownloadServerFileAsync(string localPath)
    {
        // ✅ 使用 Uri 拼接，避免 Path.Combine 的斜杠问题
        var downLoadUrl = GlobalConfigSO.Instance.GetProjectServerUrlPath();
        var datas = await DownloadFileAsync(downLoadUrl, localPath);
        return datas;
    }
    //public static async UniTask UploadFileAsync(string downLoadUrl, string localPath, string datas)
    //{
    //    var url = Path.Combine(downLoadUrl, localPath).Replace("\\", "/");
    //    var uri = new Uri(url);

    //    AotDebug.Log($"开始上传 -> {uri}");

    //    try
    //    {
    //        using var webRep = UnityWebRequest.Post(uri, datas, "application/json");
    //        webRep.timeout = 30;

    //        // ✅ 关键改动：调用我们封装的安全方法，并传入 this (MonoBehaviour)
    //        var resultRequest = await webRep.SendWebRequestSafeAsync(GameStart.Instance);

    //        var lenth = resultRequest.uploadHandler.data.Length;
    //        // 此时请求已经完成，且不会在 WebGL 卡死
    //        AotDebug.LogWarning($"上传完成 {lenth} -> {uri}");
    //    }
    //    catch (OperationCanceledException)
    //    {
    //        AotDebug.LogWarning($"上传取消 -> {uri}");
    //    }
    //    catch (Exception ex)
    //    {
    //        AotDebug.LogError($"上传失败 {uri}, {ex}");
    //    }
    //}
    public static async UniTask UploadFileAsync(string downLoadUrl, string localPath, byte[] datas)
    {
        var url = Path.Combine(downLoadUrl, localPath).Replace("\\", "/");
        var uri = new Uri(url);

        AotDebug.Log($"开始上传 -> {uri}");

        try
        {
            var request = new UnityWebRequest(uri, "POST")
            {
                uploadHandler = new UploadHandlerRaw(datas)
            };

            request.SetRequestHeader("Content-Type", "application/octet-stream");
            request.timeout = 30;

            // ✅ 关键改动：调用我们封装的安全方法，并传入 this (MonoBehaviour)
            var resultRequest = await request.SendWebRequestSafeAsync(GameStart.Instance);

            var lenth = resultRequest.uploadHandler.data.Length;
            // 此时请求已经完成，且不会在 WebGL 卡死
            AotDebug.LogWarning($"上传完成 {lenth} -> {uri}");
        }
        catch (OperationCanceledException)
        {
            AotDebug.LogWarning($"上传取消 -> {uri}");
        }
        catch (Exception ex)
        {
            AotDebug.LogError($"上传失败 {uri}, {ex}");
        }
    }
    public static async UniTask UploadFileAsync(string localPath, byte[] datas)
    {
        // ✅ 使用 Uri 拼接，避免 Path.Combine 的斜杠问题
        var downLoadUrl = GlobalConfigSO.Instance.GetProjectServerUrlPath();
        await UploadFileAsync(downLoadUrl, localPath, datas);
    }
    public static async UniTask UploadFileAsync(string localPath, string datas)
    {
        // ✅ 使用 Uri 拼接，避免 Path.Combine 的斜杠问题
        var downLoadUrl = GlobalConfigSO.Instance.GetProjectServerUrlPath();
        var byteDatas = Encoding.UTF8.GetBytes(datas);
        await UploadFileAsync(downLoadUrl, localPath, byteDatas);
    }
    //public static async UniTask<byte[]> DownloadFileAsync(string localPath)
    //{
    //    //var url = Path.Combine(GlobalConfigSO.Instance.DownloadRootUrl, localPath).Replace("\\","/");
    //    //var uri = new Uri(url);
    //    //using var webRep = UnityWebRequest.Get(uri);

    //    //var reqOperation = webRep.SendWebRequest();

    //    //try
    //    //{
    //    //    AotDebug.Log($"开始下载 -> {url}");
    //    //    var result = await reqOperation;
    //    //}
    //    //catch (Exception ex)
    //    //{
    //    //    AotDebug.LogError($"url: {url} \n{ex}");
    //    //}

    //    //if (webRep.result != UnityWebRequest.Result.Success)
    //    //{
    //    //    AotDebug.LogError($"下载失败 {url}, {webRep.error}");
    //    //    return new byte[0];
    //    //}
    //    //AotDebug.Log($"下载完成 -> {url}");
    //    //return webRep.downloadHandler.data;
    //}
}
