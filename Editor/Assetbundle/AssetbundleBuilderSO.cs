using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/AbBuilderConfig", fileName = "AbBuilderConfig")]
public class AssetbundleBuilderSO : ScriptableObject
{
#if UNITY_EDITOR
    private static AssetbundleBuilderSO _Instance = null;
    public static AssetbundleBuilderSO Instance
    {
        get
        {
            if (_Instance != null)
                return _Instance;
            var assetPD = AssetbundleUtil.GetAbSoPath();
            _Instance = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetbundleBuilderSO>(assetPD);
            return _Instance;
        }
    }
#endif

    public string[] abPaths;
    public string[] dirInAllFilesPaths;
}
