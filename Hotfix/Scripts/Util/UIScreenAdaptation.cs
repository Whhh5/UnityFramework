using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeChatWASM;


/// <summary>
/// 微信小游戏 + WebGL 专用屏幕适配
/// 1. 支持 SafeArea（刘海、灵动岛）
/// 2. 支持横竖屏切换
/// 3. 编辑器下实时预览
/// 4. 极低 GC 开销，避免 WASM 内存抖动
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class UIScreenAdaptation : MonoBehaviour
{
    private RectTransform _rt;
    private Rect _lastSafeArea;
    private bool _isDirty = true;

    // 适配模式
    public enum AdapterMode
    {
        FullScreen,     // 全屏（不处理 SafeArea）
        SafeArea,       // 仅处理 SafeArea
        ExpandSafeArea  // 扩展背景到全屏，内容限制在 SafeArea
    }

    [Header("适配模式")]
    public AdapterMode mode = AdapterMode.SafeArea;

    [Header("调试")]
    [SerializeField] private bool _showAotDebug = false;

    private Rect _ScreenRect;
    private void Awake()
    {
        _ScreenRect = GetScreenArea();
        _rt = GetComponent<RectTransform>();
        Apply();
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL 首帧 SafeArea 可能为 0，延迟一帧
        Invoke(nameof(Apply), 0.1f);
#else
        Apply();
#endif
    }

    private Rect GetScreenArea()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        var sysInfo = WX.GetSystemInfoSync();
        if (sysInfo != null)
        {
            var area = sysInfo.safeArea;
            return new Rect()
            {
                width = (float)area.width,
                height = (float)area.height,
                x = (float)area.left,
                y = (float)area.top,
            };
        }
#endif
        return Screen.safeArea;
    }

    public float GetScreenWidth()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        var sysInfo = WX.GetSystemInfoSync();
        if (sysInfo != null)
        {
            return (float)sysInfo.screenWidth;
        }
#endif
        return Screen.width;
    }
    public float GetScreenHeight()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        var sysInfo = WX.GetSystemInfoSync();
        if (sysInfo != null)
        {
            return (float)sysInfo.screenHeight;
        }
#endif
        return Screen.height;
    }

    private void OnEnable()
    {
        _isDirty = true;
        Apply();
    }

    private void Update()
    {
        // 只在 SafeArea 变化时刷新（防止 WASM 频繁 Rebuild）
        if (_lastSafeArea != _ScreenRect || _isDirty)
        {
            Apply();
        }
    }

    private void Apply()
    {
        if (_rt == null)
            _rt = GetComponent<RectTransform>();

        var safeArea = _ScreenRect;
        AotDebug.Log($"areatest: {safeArea.position} {safeArea.size} {GetScreenWidth()} {GetScreenHeight()}");

        // 微信小游戏首帧 SafeArea 可能为 0，兜底处理
        if (safeArea.width <= 0 || safeArea.height <= 0)
        {
            safeArea = new Rect(0, 0, GetScreenWidth(), GetScreenHeight());
        }

        switch (mode)
        {
            case AdapterMode.FullScreen:
                ApplyFullScreen();
                break;

            case AdapterMode.SafeArea:
                ApplySafeArea(safeArea);
                break;

            case AdapterMode.ExpandSafeArea:
                ApplyExpandSafeArea(safeArea);
                break;
        }

        _lastSafeArea = safeArea;
        _isDirty = false;

        if (_showAotDebug)
        {
            AotDebug.Log($"[ScreenAdapter] SafeArea: {safeArea}, Screen: {GetScreenWidth()}x{GetScreenHeight()}");
        }
    }

    /// <summary>
    /// 全屏模式（背景铺满，不考虑 SafeArea）
    /// </summary>
    private void ApplyFullScreen()
    {
        _rt.anchorMin = Vector2.zero;
        _rt.anchorMax = Vector2.one;
        _rt.offsetMin = Vector2.zero;
        _rt.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// SafeArea 模式（内容限制在安全区内）
    /// </summary>
    private void ApplySafeArea(Rect safeArea)
    {
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= GetScreenWidth();
        anchorMin.y /= GetScreenHeight();
        anchorMax.x /= GetScreenWidth();
        anchorMax.y /= GetScreenHeight();

        _rt.anchorMin = anchorMin;
        _rt.anchorMax = anchorMax;
        _rt.offsetMin = Vector2.zero;
        _rt.offsetMax = Vector2.zero;
    }

    /// <summary>
    /// 扩展 SafeArea 模式（背景铺满，内容限制）
    /// 注意：此模式需要 Canvas 下有一个 Background 子节点铺满全屏
    /// </summary>
    private void ApplyExpandSafeArea(Rect safeArea)
    {
        // Canvas 铺满
        _rt.anchorMin = Vector2.zero;
        _rt.anchorMax = Vector2.one;
        _rt.offsetMin = Vector2.zero;
        _rt.offsetMax = Vector2.zero;

        // 如果有 Content 子节点，限制到 SafeArea
        var content = transform.Find("Content");
        if (content != null)
        {
            var contentRt = content.GetComponent<RectTransform>();
            if (contentRt != null)
            {
                var anchorMin = safeArea.position;
                var anchorMax = safeArea.position + safeArea.size;

                anchorMin.x /= GetScreenWidth();
                anchorMin.y /= GetScreenHeight();
                anchorMax.x /= GetScreenWidth();
                anchorMax.y /= GetScreenHeight();

                contentRt.anchorMin = anchorMin;
                contentRt.anchorMax = anchorMax;
                contentRt.offsetMin = Vector2.zero;
                contentRt.offsetMax = Vector2.zero;
            }
        }
    }
}