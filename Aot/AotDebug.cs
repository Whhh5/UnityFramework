using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public static class AotDebug
{
#if !UNITY_EDITOR
    [Conditional("UNITY_AOTDEBUG")]
#endif
    public static void Log(object content)
    {
        Debug.Log($"{content}");
    }
    public static void LogWarning(object content)
    {
        Debug.LogWarning($"{content}");
    }
    public static void LogError(object content)
    {
        Debug.LogError($"{content}");
    }
}
