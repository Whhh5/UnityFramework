

using Cysharp.Threading.Tasks;
using TMPro;

public interface IPlatformAnalysys
{
    public void Init();
    public string GetPlayerPrefsString(string key);
    public void SetPlayerPrefsString(string key, string value);
    public int GetPlayerPrefsInt(string key);
    public void SetPlayerPrefsInt(string key, int value);
    public float GetPlayerPrefsFloat(string key);
    public void SetPlayerPrefsFloat(string key, float value);
    public void RegisterTmpInputField(TMP_InputField inputField);
    public void ShowAd();
}


