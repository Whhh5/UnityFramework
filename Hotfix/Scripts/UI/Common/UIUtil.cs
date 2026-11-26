using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static partial class UIUtil
{
    private static readonly Dictionary<EnLoadTarget, Func<RectTransform, int>> _WindowDatas = new();
    public static void Register<T>(EnLoadTarget id)
        where T : UIWindowData, new()
    {
        static int func(RectTransform parent)
        {
            return UIMgr.Instance.ShowWindow<T>(parent);
        }
        _WindowDatas.Add(id, func);
    }
    public static int ShowWindow(EnLoadTarget windowId, RectTransform parent = null)
    {
        if (!_WindowDatas.TryGetValue(windowId, out var func))
            throw new ArgumentNullException();
        var id = func.Invoke(parent);
        return id;
    }
    //public static Vector2 WorldToUIPos(Vector3 worldPos)
    //{
    //    var canvasRect = UIMgr.Instance.GetCanvasRect();
    //    var uiCamera = CameraMgr.Instance.GetUICamera();
    //    var mainCamera = CameraMgr.Instance.GetMainCamera();
    //    var screenPoint = WorldToView(in worldPos);
    //    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out var uiPos))
    //        return Vector2.zero;
    //    return uiPos;
    //}
    //public static Vector2 ViewToUIPos(in Vector2 screenPoint)
    //{
    //    var canvasRect = UIMgr.Instance.GetCanvasRect();
    //    var uiCamera = CameraMgr.Instance.GetUICamera();
    //    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out var uiPos))
    //        return Vector2.zero;
    //    return uiPos;
    //}
    //public static Vector2 WorldToView(in Vector3 worldPos)
    //{
    //    var uiCamera = CameraMgr.Instance.GetUICamera();
    //    var mainCamera = CameraMgr.Instance.GetMainCamera();
    //    var screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPos);
    //    return screenPoint;
    //}
    public static Vector2 ViewToWorld(in Vector3 worldPos)
    {
        var mainCamera = CameraMgr.Instance.GetMainCamera();
        var screenPoint = mainCamera.ScreenToWorldPoint(worldPos);
        return screenPoint;
    }
    public static Vector2 UIViewToWorld(in Vector3 worldPos)
    {
        var mainCamera = CameraMgr.Instance.GetUICamera();
        var screenPoint = mainCamera.ScreenToWorldPoint(worldPos);
        return screenPoint;
    }
    public static Vector3 UIPosToWorldPos(in Vector2 uiPos)
    {
        var canvasScale = UIMgr.Instance.GetCanvasScale();
        var worldPos = uiPos / 100 * canvasScale;
        return worldPos;
    }

    public static void ShowHitTips(int langId)
    {
        var content = LanguageUtility.GetLanguage(langId);
        ShowHitTips(content);
    }
    public static void ShowHitTips(string content)
    {
        var ud = ClassPoolMgr.Instance.Pull<UIHitTipsUserData>();
        ud.Setting(content);
        UIMgr.Instance.ShowWindow<UIHitTipsData>(ud);
        ClassPoolMgr.Instance.Push(ud);
    }
    public static void ShowControllerDialog(string content, EnSourceId sourceId, Action confirmation, Action cancel)
    {
        var ud = ClassPoolMgr.Instance.Pull<UIControllerDialogUserData>();
        ud.Setting(content, confirmation, cancel, sourceId);
        UIMgr.Instance.ShowWindow<UIControllerDialogData>(ud);
        ClassPoolMgr.Instance.Push(ud);
    }
}
