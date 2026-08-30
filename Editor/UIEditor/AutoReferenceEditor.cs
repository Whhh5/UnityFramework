using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityFS;
using Debug = UnityEngine.Debug;

public static class AutoReferenceFinder
{
    private static List<FieldInfo> GetAllFieldInfos(Type type)
    {
        var fieldInfos = new List<FieldInfo>();
        var parentType = type;
        while (parentType != null && parentType != typeof(MonoBehaviour))
        {
            var types = parentType.GetFields((BindingFlags)int.MaxValue);
            fieldInfos.AddRange(types);
            parentType = parentType.BaseType;
        }
        return fieldInfos;
    }
    /// <summary>
    /// 对单个MonoBehaviour执行自动引用赋值
    /// </summary>
    public static void AutoAssignReferences(MonoBehaviour target)
    {
        if (target == null) return;
        GameObject go = target.gameObject;
        Type targetType = target.GetType();
        var fields = GetAllFieldInfos(targetType);

        bool modified = false;

        var trans = target.GetComponentsInChildren<Transform>().ToList();
        foreach (var field in fields)
        {
            bool hasSerializeField = field.IsDefined(typeof(SerializeField), true);
            var autoAttr = field.GetCustomAttribute<AutoReferenceAttribute>(true);

            if (!hasSerializeField || autoAttr == null)
                continue;

            var testGoName = autoAttr.objName ?? field.Name;
            var find = autoAttr.scope == SearchScope.Self
                ? target.transform
                : trans.Find(tran => tran.gameObject.name == testGoName);
            if(find == null)
            {
                Debug.LogWarning($"{target.gameObject.name} 找不到 {field.Name} 对象 {testGoName}");
                continue;
            }
            if (field.FieldType == typeof(GameObject))
            {
                modified = true;
                field.SetValue(target, find.gameObject);
                continue;
            }
            if (!find.TryGetComponent(field.FieldType, out var comp))
            {
                Debug.LogWarning($"[AutoReference] {targetType.Name}.{find.name} 没有组件 {field.FieldType}");
                continue;
            }

            field.SetValue(target, comp);
            modified = true;
        }

        if (modified)
        {
            EditorUtility.SetDirty(target);
        }
    }

    /// <summary>
    /// 扫描GameObject上所有MonoBehaviour
    /// </summary>
    public static void ScanGameObject(GameObject go)
    {
        if (go == null) return;
        if (go.TryGetComponent<GameEntity>(out var com))
        {
            AutoAssignReferences(com);
        }
        if (go.TryGetComponent<UIWindowItem>(out var com2))
        {
            AutoAssignReferences(com2);
        }
    }

    [MenuItem("GameObject/AutoReference/扫描当前物体(仅自身脚本) &_r", false, 10)]
    private static void MenuScanSelected()
    {
        var go = Selection.activeGameObject;
        if (go == null)
        {
            EditorUtility.DisplayDialog("提示", "请选中GameObject", "OK");
            return;
        }
        ScanGameObject(go);
    }
}

/// <summary>
/// Prefab打开监听：进入Prefab编辑模式时自动扫描根节点
/// </summary>
[InitializeOnLoad]
public static class PrefabOpenAutoScanHook
{
    static PrefabOpenAutoScanHook()
    {
        PrefabEditorEvents.prefabOpened += OnPrefabOpened;
    }

    private static void OnPrefabOpened(GameObject prefabRoot)
    {
        if (prefabRoot == null) return;
        //Debug.Log($"[AutoReference] Prefab打开：{prefabRoot.name}，开始自动扫描根节点");
        AutoReferenceFinder.ScanGameObject(prefabRoot);
    }
}

/// <summary>
/// Unity 2020+ Prefab编辑事件封装
/// </summary>
public static class PrefabEditorEvents
{
    public static event Action<GameObject> prefabOpened;

    static PrefabEditorEvents()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private static GameObject _lastOpenPrefabRoot;

    private static void OnEditorUpdate()
    {
        var stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (stage == null)
        {
            _lastOpenPrefabRoot = null;
            return;
        }

        GameObject root = stage.prefabContentsRoot;
        if (root != null && _lastOpenPrefabRoot != root)
        {
            _lastOpenPrefabRoot = root;
            prefabOpened?.Invoke(root);
        }
    }
}