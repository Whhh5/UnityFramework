#if UNITY_WEBGL && UNITY_MINIGAME_WX && !UNITY_EDITOR
#define UNITY_WX
#endif

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EnLoaderType
{
#if UNITY_EDITOR
    Editor,
#endif
    Localhost,
    internet,
    EnumCount,
}
public interface IUpdateGame
{
    public UniTask InitializationAsync();
}

public interface IGameStart
{
    public UniTask<bool> AwakeAsync();
    public UniTask<bool> StartAsync();
    public void Destroy();
}
public class GameStart : MonoBehaviour
{
    public static GameStart Instance = null;
    private IUpdateGame _UpdateGameController;
    private Assembly _MainAssembly;
    private readonly IGameStart _GameStart =
#if UNITY_WX
        new WxGameStart()
#else
        new EditorGameStart()
#endif
        ;

    private async void Awake()
    {
        Instance = this;
        _UpdateGameController = GlobalConfigSO.Instance.LoaderType switch
        {
#if UNITY_EDITOR
            EnLoaderType.Editor => new HotfixTest(),
#endif
            _ => new HotfixMgr()
        };

        if (!await _GameStart.AwakeAsync())
            return;

        await _UpdateGameController.InitializationAsync();
        _MainAssembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == GlobalConfigSO.Instance.MainAssemblyName);
        await Resources.UnloadUnusedAssets();

        if (!await _GameStart.StartAsync())
            return;

        if (!Application.isPlaying)
            return;

        var type = AotUtility.GetClassesWithGameStartAttribute(typeof(GameStartAttribute));
        AotDebug.Log($"通过【{type.Name}】运行游戏");
        var go = new GameObject(type.FullName, type);
        GameObject.DontDestroyOnLoad(go);
    }

    public Assembly GetMainAssembly()
    {
        return _MainAssembly;
    }
}