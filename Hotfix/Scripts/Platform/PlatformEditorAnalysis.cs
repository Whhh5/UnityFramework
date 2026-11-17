using Cysharp.Threading.Tasks;
using TMPro;

public class PlatformEditorAnalysis : IPlatformAnalysys
{

    public string GetPlayerPrefsString(string key)
    {
        var value = UnityEngine.PlayerPrefs.GetString(key, null);
        return value;
    }
    public void SetPlayerPrefsString(string key, string value)
    {
        UnityEngine.PlayerPrefs.SetString(key, value);
    }
    public float GetPlayerPrefsFloat(string key)
    {
        var value = UnityEngine.PlayerPrefs.GetFloat(key, -1);
        return value;
    }

    public void SetPlayerPrefsFloat(string key, float value)
    {
        UnityEngine.PlayerPrefs.SetFloat(key, value);
    }

    public int GetPlayerPrefsInt(string key)
    {
        var value = UnityEngine.PlayerPrefs.GetInt(key, -1);
        return value;
    }
    public void SetPlayerPrefsInt(string key, int value)
    {
        UnityEngine.PlayerPrefs.SetInt(key, value);
    }

    public void RegisterTmpInputField(TMP_InputField inputField)
    {
        
    }

    public void Init()
    {
        UIMgr.Instance.ShowWindow<UIStartGameWindowData>();
    }

    public void ShowAd()
    {
        
    }
}
