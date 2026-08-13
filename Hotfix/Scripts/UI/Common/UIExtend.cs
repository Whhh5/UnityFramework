using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public static class UIExtend
{
    public static void AddListener(this Button btn, UnityAction callback)
    {
        btn.onClick.AddListener(callback);
    }
    public static void RemoveAllListeners(this Button btn)
    {
        btn.onClick.RemoveAllListeners();
    }
    public static void RemoveListener(this Button btn, UnityAction callback)
    {
        btn.onClick.RemoveListener(callback);
    }


    private class ImageLoadData : IClassPoolInit
    {
        public LoadHandle handle;
        public EnLoadTarget loadTarget;
        public EnLoadStatus status;
        public CancellationTokenSource token;
        public void OnPoolDestroy()
        {
            handle = default;
            loadTarget = EnLoadTarget.None;
            status = EnLoadStatus.None;
            token = null;
        }

        public void OnPoolInit<T>(T userData) where T : IClassPool
        {
            token = new();
        }
    }
    private static readonly Dictionary<Image, ImageLoadData> _ImgLoadDatas = new();
    public static async void LoadSprirte(this Image image, EnLoadTarget loadTarget)
    {
        if (_ImgLoadDatas.TryGetValue(image, out var loadData))
        {
            if (loadTarget == loadData.loadTarget)
                return;
            UnloadSprite(image);
        }

        loadData = ClassPoolMgr.Instance.Pull<ImageLoadData>();
        _ImgLoadDatas.Add(image, loadData);

        loadData.loadTarget = loadTarget;
        loadData.status = EnLoadStatus.Loading;
        var (isCancel, loadHandle) = await ABBLoadMgr.Instance.LoadAsync<Sprite>(loadTarget, loadData.token.Token).SuppressCancellationThrow();
        if (!isCancel)
        {
            if (!_ImgLoadDatas.ContainsKey(image))
                return;
            loadData.status = EnLoadStatus.Finish;
            loadData.token = null;
            loadData.handle = loadHandle;
            var sprite = loadHandle.GetObject<Sprite>();
            image.sprite = sprite;
        }
    }
    public static void UnloadSprite(this Image image)
    {
        if (!_ImgLoadDatas.TryGetValue(image, out var loadData))
            return;
        switch (loadData.status)
        {
            case EnLoadStatus.Loading:
                loadData.token.Cancel();
                break;
            case EnLoadStatus.Finish:
                ABBLoadMgr.Instance.Unload(loadData.handle);
                break;
            default:
                break;
        }
        ClassPoolMgr.Instance.Push(loadData);
        _ImgLoadDatas.Remove(image);
    }
    public static void SetLanguageText(this TextMeshProUGUI txt, int langId)
    {
        var str = LanguageUtility.GetLanguage(langId);
        txt.SetText(str);
    }
    public static void SetLanguageText(this TextMeshProUGUI txt, int langId, params object[] format)
    {
        var str = LanguageUtility.GetLanguage(langId, format);
        txt.SetText(str);
    }
}
