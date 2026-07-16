using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AnimatorCheckerEditor
{
    static AnimatorCheckerEditor()
    {
        PrefabStage.prefabStageOpened += PrefabStageOpened;
        PrefabStage.prefabStageClosing += PrefabStageClosing;
    }

    private static void PrefabStageOpened(PrefabStage prefabState)
    {
        if (!prefabState.prefabContentsRoot.TryGetComponent<IEntityAnimCom>(out var _))
            return;
        if (!prefabState.prefabContentsRoot.TryGetComponent<Animator>(out var animator))
            animator = prefabState.prefabContentsRoot.AddComponent<Animator>();
        var assGuidList = AssetDatabase.FindAssets($"{prefabState.prefabContentsRoot.name} t:animationclip");
        var controller = new AnimatorController();
        controller.AddLayer("Base Layer");
        var stateMachine = controller.layers[0].stateMachine;

        for (int i = 0; i < assGuidList.Length; i++)
        {
            var guid = assGuidList[i];
            var assPath = AssetDatabase.GUIDToAssetPath(guid);
            var ass = AssetDatabase.LoadAssetAtPath<AnimationClip>(assPath);

            var state = stateMachine.AddState($"{ass.name}");
            state.motion = ass;

        }

        animator.runtimeAnimatorController = controller;
    }
    private static void PrefabStageClosing(PrefabStage state)
    {
        if (!state.prefabContentsRoot.TryGetComponent<IEntityAnimCom>(out var entity))
            return;

        var path = state.assetPath;
        EditorApplication.delayCall += () =>
        {
            var go = PrefabUtility.LoadPrefabContents(path);
            if (!go.TryGetComponent<Animator>(out var animator))
                return;
            Object.DestroyImmediate(animator);
            PrefabUtility.SaveAsPrefabAsset(go, path);
            PrefabUtility.UnloadPrefabContents(go);
        };
    }
}
