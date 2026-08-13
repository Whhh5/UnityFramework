using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Diagnostics;
using System;

/// <summary>
/// 编辑器模式下音频播放工具
/// </summary>
public static class AudioEditorPlayer
{
    // 反射获取 Unity 编辑器内部音频工具类
    private static System.Type _audioUtilType = typeof(EditorWindow).Assembly.GetType("UnityEditor.AudioUtil");

    //[MenuItem("工具/调试/查找AudioUtil类型")]
    public static void FindAllAudioUtil()
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.Name.Contains("AudioUtil") || type.Name.Contains("AudioPlay"))
                {
                    UnityEngine.Debug.Log($"找到：{type.FullName} | 程序集：{assembly.GetName().Name}");
                }
            }
        }
    }
    public static void StopAllClips()
    {
        MethodInfo stopAllMethod = _audioUtilType.GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public);
        stopAllMethod?.Invoke(null, null);
    }

    #region 公共调用方法（可在其他编辑器脚本中直接调用）
    /// <summary>
    /// 播放指定音频片段
    /// </summary>
    /// <param name="clip">音频片段</param>
    /// <param name="startSample">起始采样位置</param>
    /// <param name="loop">是否循环播放</param>
    public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
    {
        MethodInfo isPlayingMethod = _audioUtilType.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public);
        isPlayingMethod?.Invoke(null, new object[] { clip, startSample, loop });
    }

    /// <summary>
    /// 停止指定音频片段
    /// </summary>
    public static void StopClip()
    {
        MethodInfo stopMethod = _audioUtilType.GetMethod("PausePreviewClip", BindingFlags.Static | BindingFlags.Public);
        stopMethod?.Invoke(null, null);
    }

    /// <summary>
    /// 判断音频是否正在播放
    /// </summary>
    public static bool IsClipPlaying()
    {
        MethodInfo isPlayingMethod = _audioUtilType.GetMethod("IsPreviewClipPlaying", BindingFlags.Static | BindingFlags.Public);
        var result = isPlayingMethod?.Invoke(null, null);
        return (bool)(result ?? false);
    }

    /// <summary>
    /// 通过资源路径播放音频
    /// </summary>
    /// <param name="assetPath">音频在项目中的路径，例如 "Assets/Audio/BGM.mp3"</param>
    /// <param name="loop">是否循环</param>
    public static void PlayClipByPath(string assetPath, bool loop = false)
    {
        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);
        if (clip == null)
        {
            UnityEngine.Debug.LogError($"找不到音频文件：{assetPath}");
            return;
        }
        PlayClip(clip, 0, loop);
    }
    #endregion
}