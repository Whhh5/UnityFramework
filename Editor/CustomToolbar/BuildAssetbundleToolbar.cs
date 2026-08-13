using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomUnityToolbarAttribute("BuildAssetbundle", EnCustomUnityToolbarType.Left, EnCustomUnityToolbarOrder.BuildAssetbundle)]
public class BuildAssetbundleToolbar : ICustomUnityToolbar
{
    public void OnClick()
    {
        var buildTarget = GlobalConfigEditorSO.Instance.BuildTarget;

        // 拷贝 热更 dll
        BuildBundleToolbar.CopyHotfixDlls(buildTarget);

        // 拷贝 matadata dll
        BuildBundleToolbar.CopyStrippedDlls(buildTarget);

        AssetbundleToolEditor.BuildAssetbundle();
    }
}
