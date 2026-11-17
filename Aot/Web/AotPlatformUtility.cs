public static class AotPlatformUtility
{
    // 判断当前是否运行在 WebGL 环境（排除编辑器）
    public static bool IsWebGL
    {
        get
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}