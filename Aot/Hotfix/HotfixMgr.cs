using UnityEngine;
using Cysharp.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Reflection;
using HybridCLR;
using System.Collections.Generic;
using System.Text;
using System.Linq;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class GameStartAttribute : Attribute
{

}
public class AbVersionInfo
{
    public int version;
}
public class HotfixMgr : IUpdateGame
{
    public async UniTask InitializationAsync()
    {
        var loadWindow = await Resources.LoadAsync<GameObject>("LoadWindow");
        var window = GameObject.Instantiate(loadWindow);
        AotDebug.Log("开始检查更新 Assetbundle");
        await CheckCatalogAsync();
        AotDebug.Log("开始加载程序集");
        LoadWindow.Instance.SetText("assembly");
        await LoadAssemblyAsync();
        LoadWindow.Instance.SetText("matadata");
        await LoadAssemblyMatadataAsync();
        AotDebug.Log("更新完成");
        LoadWindow.Instance.SetText("finish");
        await UniTask.DelayFrame(3);
        GameObject.Destroy(window);
    }

    private async UniTask CheckCatalogAsync()
    {
        var path = AssetbundleUtil.GetAbCatalogLocalPath();
        var webCatalogData = await WebUtil.DownloadStaticAssetFileAsync(path);
        var catalogStr = Encoding.UTF8.GetString(webCatalogData);
        var webCatalogList = AssetbundleUtil.JsonToCatalogList(catalogStr);

        var localCatalogPath = AssetbundleUtil.GetCurPlatformAbCatalogFilePath();

        var count = webCatalogList.Count;
        var uploadStr = new StringBuilder(500);
        uploadStr.AppendLine("hotfix info:");
        for (int i = 0; i < count; i++)
        {
            var webCatalog = webCatalogList[i];
            var versionPath = AssetbundleUtil.GetCurPlatformAssetbundleVersionFilePath(webCatalog.abName);

            var versionInfoStr = File.Exists(versionPath) ? File.ReadAllText(versionPath) : "";
            var versionInfo = JsonConvert.DeserializeObject<AbVersionInfo>(versionInfoStr) ?? new();

            LoadWindow.Instance.SetText(webCatalog.abName);
            LoadWindow.Instance.SetSlider((i + 1) / (float)count);
            if (versionInfo.version == webCatalog.version)
                continue;

            var abLoadPath = AssetbundleUtil.GetAssetbundleLocalPath(webCatalog.abName);
            var abFileByte = await WebUtil.DownloadStaticAssetFileAsync(abLoadPath);
            if (abFileByte.Length != webCatalog.length
                || AotUtility.CalculateFileMD5(abFileByte) != webCatalog.md5)
            {
                // 下载失败重复下载当前文件
                AotDebug.LogError($"下载失败重新下载 {abFileByte.Length}-{webCatalog.length}  {webCatalog.md5} -> {abLoadPath}");
                i--;
                continue;
            }
            uploadStr.AppendLine(webCatalog.abName);
            // 下载校验成功保存文件
            var abPath = AssetbundleUtil.GetCurPlatformAssetbundlePath(webCatalog.abName);
            var root = Path.GetDirectoryName(abPath);
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            AotDebug.Log($"保存文件 -> {abPath}, {abFileByte.Length}");
            File.WriteAllBytes(abPath, abFileByte);
            AotDebug.Log($"保存文件2 -> {abPath}, {abFileByte.Length}");
            await AssetBundle.RecompressAssetBundleAsync(abPath, abPath, BuildCompression.LZ4Runtime);
            versionInfo.version = webCatalog.version;
            var versionInfoJson = JsonConvert.SerializeObject(versionInfo);
            AotDebug.Log($"保存文件 -> {versionPath}, {versionInfoJson}");
            File.WriteAllText(versionPath, versionInfoJson);
        }
        File.WriteAllBytes(localCatalogPath, webCatalogData);

        // 下载assetinfo 和 abinfo
        var abInfo = AssetbundleUtil.GetAbInfoLocalPath();
        var abInfoLocalPath = AssetbundleUtil.GetCurPlatformAbInfoFilePath();
        var fileName = Path.GetFileName(abInfoLocalPath);
        LoadWindow.Instance.SetText(fileName);
        uploadStr.AppendLine(fileName);
        await DownloadFile(abInfo, abInfoLocalPath);

        var assetInfo = AssetbundleUtil.GetAssetInfoLocalPath();
        var assetInfoLocalPath = AssetbundleUtil.GetCurPlatformAssetInfoFilePath();
        var fileName2 = Path.GetFileName(assetInfoLocalPath);
        LoadWindow.Instance.SetText(fileName2);
        uploadStr.AppendLine(fileName2);
        await DownloadFile(assetInfo, assetInfoLocalPath);

        AotDebug.LogWarning(uploadStr);

    }
    private async UniTask DownloadFile(string webLocalPath, string savePath)
    {
        var abInfoData = await WebUtil.DownloadStaticAssetFileAsync(webLocalPath);
        File.WriteAllBytes(savePath, abInfoData);
    }
    private async UniTask LoadAssemblyAsync()
    {
        var abName = AssetbundleUtil.GetAbNameByPath(GlobalConfigSO.Instance.DllBytesDirectory);
        var abPath = AssetbundleUtil.GetCurPlatformAssetbundlePath(abName);
        var ab = await AssetBundle.LoadFromFileAsync(abPath);
        if (ab == null)
            return;

        for (int i = 0; i < GlobalConfigSO.Instance.HotfixList.Length; i++)
        {
            var name = GlobalConfigSO.Instance.HotfixList[i];
            var file = ab.LoadAssetAsync<TextAsset>(name);
            AotDebug.Log($"loadassembly: {name}");

            var data = file.asset as TextAsset;

#if UNITY_EDITOR

#else
            Assembly hotUpdateAss = Assembly.Load(data.bytes);
            AotDebug.Log($"load hotfixassembly: {hotUpdateAss.FullName}");
#endif
        }
        ab.Unload(true);
    }

    private async UniTask LoadAssemblyMatadataAsync()
    {
        var abName = AssetbundleUtil.GetAbNameByPath(GlobalConfigSO.Instance.DllMatadataBytesDirectory);
        var metaDataAbPath = AssetbundleUtil.GetCurPlatformAssetbundlePath(abName);
        var metadataAb = await AssetBundle.LoadFromFileAsync(metaDataAbPath);
        if (metadataAb == null)
            return;
        var metadataNames = metadataAb.GetAllAssetNames();

        for (int i = 0; i < metadataNames.Length; i++)
        {
            var name = metadataNames[i];
            var file = metadataAb.LoadAssetAsync<TextAsset>(name);
            AotDebug.Log(file.asset);

            var data = file.asset as TextAsset;

#if UNITY_EDITOR

#else
            RuntimeApi.LoadMetadataForAOTAssembly(data.bytes, HomologousImageMode.SuperSet);
#endif
        }
        metadataAb.Unload(true);
    }
}
