using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EnUIWindowType
{
    Window,
    Dialog,
    Tips,
    Hint,
}

public enum EnWindowHideType
{
    None,
    Close,
    SelectItem,
}
public class UIMgr : SingletonMono<UIMgr>
{
    [SerializeField]
    private Canvas m_MainCanvas = null;
    [SerializeField]
    private RectTransform _CanvasRect = null;
    [SerializeField]
    private RectTransform m_UIRootRect = null;
    [SerializeField]
    private RectTransform m_UITipsRootRect = null;

    private readonly Dictionary<int, int> _WindowId2EntityDataID = new();
    private readonly List<int> _WindowList = new();
    private readonly Dictionary<EnUIWindowType, RectTransform> m_WindowType2Root = new();

    private Shader _UITmpDissolveShader = null;
    private Texture _NoiseTexture;
    private Shader _UIImageShader;
    protected override async void Awake()
    {
        base.Awake();

        if (ABBUtil.TryGetClassesWithBaseClass<IStartLaunch>(out var launch))
        {
            var ins = Activator.CreateInstance(launch) as IStartLaunch;
            ins.StartLaunch();
        }
        else
        {
            //ShowWindow<CGameStartWindowData>();
            ABBUtil.LogError($"没有找到启动界面，请继承 {typeof(IStartLaunch)}");
        }

        Debug.Log($"uimgrloadshader {_NoiseTexture} {_UITmpDissolveShader} {_UIImageShader}");
        var noiseLoadhandle = await ABBLoadMgr.Instance.LoadAsync<Texture>(EnLoadTarget.Sp_Noise2, default);
        _NoiseTexture = noiseLoadhandle.GetObject<Texture>();
        var tmpDissolveShader = await ABBLoadMgr.Instance.LoadAsync<Shader>(EnLoadTarget.Shader_DistanceFieldDissolve_ScreenSpaceNoise, default);
        _UITmpDissolveShader = tmpDissolveShader.GetObject<Shader>();
        var imgDissolveShader = await ABBLoadMgr.Instance.LoadAsync<Shader>(EnLoadTarget.Shader_UI_Dissolve_ScreenSpaceNoise, default);
        _UIImageShader = imgDissolveShader.GetObject<Shader>();

        Debug.Log($"uimgrloadshader {_NoiseTexture} {_UITmpDissolveShader} {_UIImageShader}");
    }

    public Shader GetTmpDissolveShader()
    {
        return _UITmpDissolveShader;
    }
    public Shader GetImageDissolveShader()
    {
        return _UIImageShader;
    }
    public Texture GetNoiseTexture()
    {
        return _NoiseTexture;
    }


    public RectTransform GetUITipsRootRect()
    {
        return m_UITipsRootRect;
    }
    public RectTransform GetCanvasRect()
    {
        return _CanvasRect;
    }
    public Vector2 GetUISize()
    {
        return m_MainCanvas.GetComponent<RectTransform>().sizeDelta;
    }
    public Camera GetCanvasCamera()
    {
        return m_MainCanvas.worldCamera;
    }
    public T GetWindow<T>(int windowId)
        where T : UIWindowData
    {
        var windowData = GetWindow(windowId);
        return windowData as T;
    }
    private UIWindowData GetWindow(int windowId)
    {
        if (!_WindowId2EntityDataID.TryGetValue(windowId, out var entityID))
            return null;
        var windowData = EntityMgr.Instance.GetEntityData(entityID);
        return windowData as UIWindowData;
    }
    public bool ContainsWindow(int windowId)
    {
        return _WindowId2EntityDataID.ContainsKey(windowId);
    }
    public int ShowWindow<T>()
        where T : UIWindowData, new()
    {
        var userData = ClassPoolMgr.Instance.Pull<UIWindowUserData>();
        var windowId = ShowWindow<T>(userData);
        ClassPoolMgr.Instance.Push(userData);
        return windowId;
    }

    public int ShowWindow<T>(UIWindowUserData userData)
        where T : UIWindowData, new()
    {
        var windowId = ABBUtil.GetTempKey();
        userData.windowId = windowId;
        var dataID = EntityMgr.Instance.CreateEntityData<T>(userData);
        _WindowId2EntityDataID.Add(windowId, dataID);
        _WindowList.Add(windowId);
        var windowData = GetWindow<T>(windowId);

        if (windowData.WindowType == EnUIWindowType.Window)
            GameStateMgr.Instance.AddGameState(EnGameStatus.UIWindow);

        //windowData.OnShow(userData);

        if (!m_WindowType2Root.TryGetValue(windowData.WindowType, out var root))
            root = m_UIRootRect;
        windowData.SetParentTran(root);
        EntityMgr.Instance.LoadEntity(dataID);
        return windowId;
    }
    //public void HideWindow<T>()
    //    where T: UIWindowData
    //{
    //    var type = typeof(T);
    //    HideWindow(type);
    //}
    public bool IsValid(int windowId)
    {
        return _WindowId2EntityDataID.ContainsKey(windowId);
    }
    public void HideWindow(int windowId)
    {
        HideWindow(windowId, EnWindowHideType.Close);
    }
    public async void HideWindow(int windowId, EnWindowHideType hideCode)
    {
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_WINDOW_HIDE, windowId, (int)hideCode);
        var windowData = GetWindow(windowId);

        if (windowData.WindowType == EnUIWindowType.Window)
            GameStateMgr.Instance.RemoveGameState(EnGameStatus.UIWindow);

        //windowData.OnHide();
        _WindowList.Remove(windowId);
        _WindowId2EntityDataID.Remove(windowId);

        await windowData.HideAsync();
        EntityMgr.Instance.RecycleEntityData(windowData.EntityID);
    }
    public void HideAllWindow()
    {
        var list = new List<int>();
        foreach (var item in _WindowId2EntityDataID.Keys)
            list.Add(item);
        foreach (var item in list)
            HideWindow(item);
    }
    public void HideAllWindow(EnUIWindowType windowType)
    {
        var list = new List<int>();
        foreach (var (windowId, _) in _WindowId2EntityDataID)
        {
            var windowData = GetWindow(windowId);
            if (windowData.WindowType != windowType)
                continue;
            list.Add(windowId);
        }
        foreach (var item in list)
            HideWindow(item);
    }

    public void AddBtnListener(Button btn, UnityAction action)
    {
        btn.onClick.AddListener(action);
    }
    public void RemoveBtnListener(Button btn, UnityAction action)
    {
        btn.onClick.RemoveListener(action);
    }

    public async UniTask<T> LoadWindowItemAsync<T>(EnLoadTarget loadTarget, RectTransform parent, CancellationToken token)
    {
        var goId = await ABBGOMgr.Instance.CreateGOAsync(loadTarget, parent);
        if (token.IsCancellationRequested)
        {
            ABBGOMgr.Instance.DestroyGO(goId);
            throw new OperationCanceledException();
        }
        var com = ABBGOMgr.Instance.GetGoCom<T>(goId);
        if (com is not UIWindowItem windowITem)
        {
            throw new Exception($"{typeof(T)} is not {typeof(UIWindowItem)}");
        }
        windowITem.SetGoId(goId);
        windowITem.LoadCompeletion();
        return com;
    }
    public void UnloadWindowItem<T>(T item)
    {
        if (item is not UIWindowItem windowITem)
        {
            throw new Exception($"{item.GetType()} is not {typeof(UIWindowItem)}");
        }
        var goId = windowITem.GetGoId();
        windowITem.Unload();
        ABBGOMgr.Instance.DestroyGO(goId);
    }
    public bool IsTop(int windowId, EnUIWindowType windowType)
    {
        for (int i = 0; i < _WindowList.Count; i++)
        {
            var testWindowId = _WindowList[^(i + 1)];
            var windowData = EntityMgr.Instance.GetEntityData<UIWindowData>(_WindowId2EntityDataID[testWindowId]);
            if (windowData.WindowType != windowType)
                continue;
            return windowId == testWindowId;
        }
        return false;
    }
    public bool IsTop(int windowId)
    {
        var testWindowId = _WindowList[^1];
        return windowId == testWindowId;

    }
}
