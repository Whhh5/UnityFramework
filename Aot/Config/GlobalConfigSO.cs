using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(menuName = "SO/GlobalConfigSO", fileName = "GlobalConfigSO")]
public class GlobalConfigSO : ScriptableObject
{
    [SerializeField]
    private EnLoaderType _LoaderType = EnLoaderType.Editor;

    [Header("项目名称")]
    public string ProjectName = $"RunGame";

    [Header("静态网页资源")]
    public string StaticWebUtl = $"https://cloud1-d5gy0bngg418fba77-1304051594.tcloudbaseapp.com";
    [Header("内网服务器地址")]
    public string DownloadRootUrl = $"http://192.168.31.129:8000";
    [Header("外网服务器地址穿透")]
    public string ServerUrl = $"https://librarian-widget-churn.ngrok-free.dev";

    [Header("外网服务器地址穿透")]
    public string PersistentRoot = $"Persistent";

    [Header("dll 加载顺序")]
    public string[] HotfixList = null;

    [Header("主程序集名字")]
    public string MainAssemblyName = "HotfixAssembly";

    [Header("config ab 名字")]
    public string ConfigAssetbundleName = "gamecfgjson.ab";

    //[Header("dll 元数据 ab字 名")]
    //public string HotfixMatadataAssetbundleName = "hotfixmetadatadlls.ab";

    //[Header("服务器文件地址")]
    //public string ServerDataPath = "/Users/qiuxiaohui/Desktop/MyServer/Remote/RunGame";

    [Header("dll bytes 目录")]
    public string DllBytesDirectory = "Assets/GameRes/HotfixRes/Assembly/HotfixDlls";
    [Header("dll matadata bytes 目录")]
    public string DllMatadataBytesDirectory = "Assets/GameRes/HotfixRes/Assembly/HotfixMetadataDlls";

    public EnLoaderType LoaderType
    {
        get
        {
#if !UNITY_EDITOR
            if (_LoaderType == EnLoaderType.Editor)
                throw new Exception($"load type error:{_LoaderType}");
#endif
            return _LoaderType;
        }
        set
        {
            _LoaderType = value;
        }
    }

    private static GlobalConfigSO _Instance = null;
    public static GlobalConfigSO Instance
    {
        get
        {
            if (_Instance == null)
            {
                var path = Path.Combine("SO", "GlobalConfigSO");
                _Instance = Resources.Load<GlobalConfigSO>(path);
            }
            return _Instance;
        }
    }

    public string GetPersistentRoot()
    {
        var path = PersistentRoot;
        return path;
    }
    public string GetServerUrlPath()
    {
        var url = LoaderType switch
        {
            EnLoaderType.Editor => DownloadRootUrl,
            EnLoaderType.Localhost => DownloadRootUrl,
            EnLoaderType.internet => ServerUrl,
            _ => throw new Exception($"get server url error: {LoaderType}"),
        };
        var path = Path.Combine(url, ProjectName);
        return path;
    }
}
