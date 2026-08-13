using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtil
{
    public static Vector2 WorldToUIPos(Vector3 worldPos)
    {
        var canvasRect = UIMgr.Instance.GetCanvasRect();
        var uiCamera = CameraMgr.Instance.GetUICamera();
        var mainCamera = CameraMgr.Instance.GetMainCamera();
        var screenPoint = WorldToView(in worldPos);
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out var uiPos))
            return Vector2.zero;
        return uiPos;
    }
    public static Vector2 ViewToUIPos(in Vector2 screenPoint)
    {
        var canvasRect = UIMgr.Instance.GetCanvasRect();
        var uiCamera = CameraMgr.Instance.GetUICamera();
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, uiCamera, out var uiPos))
            return Vector2.zero;
        return uiPos;
    }
    public static Vector2 WorldToView(in Vector3 worldPos)
    {
        var uiCamera = CameraMgr.Instance.GetUICamera();
        var mainCamera = CameraMgr.Instance.GetMainCamera();
        var screenPoint = RectTransformUtility.WorldToScreenPoint(uiCamera, worldPos);
        return screenPoint;
    }
    public static Vector2 ViewToWorld(in Vector3 worldPos)
    {
        var mainCamera = CameraMgr.Instance.GetMainCamera();
        var screenPoint = mainCamera.ScreenToWorldPoint(worldPos);
        return screenPoint;
    }
}
