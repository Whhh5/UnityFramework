#if UNITY_WEBGL && UNITY_MINIGAME_WX && !UNITY_EDITOR
#define UNITY_WX
#endif

public static class PlatformUtility
{
    public static readonly IPlatformAnalysys PlatformAnalysis =
#if UNITY_WX
        new PlatformWxAnalysis()
#else
        new PlatformEditorAnalysis()
#endif
        ;
}
