using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum EnPlayerPrefsId
{
    None,
    Account,
    Password,
}
public class PlayerPrefsMgr : Singleton<PlayerPrefsMgr>
{

    public int GetInt(EnPlayerPrefsId prefsId)
    {
        var value = PlatformUtility.PlatformAnalysis.GetPlayerPrefsInt($"{prefsId}");
        return value;
    }
    public void SetInt(EnPlayerPrefsId prefsId, int value)
    {
        PlatformUtility.PlatformAnalysis.SetPlayerPrefsInt($"{prefsId}", value);
    }
    public float GetFloat(EnPlayerPrefsId prefsId)
    {
        var value = PlatformUtility.PlatformAnalysis.GetPlayerPrefsFloat($"{prefsId}");
        return value;
    }
    public void SetFloat(EnPlayerPrefsId prefsId, float value)
    {
        PlatformUtility.PlatformAnalysis.SetPlayerPrefsFloat($"{prefsId}", value);
    }
    public string GetString(EnPlayerPrefsId prefsId)
    {
        var value = PlatformUtility.PlatformAnalysis.GetPlayerPrefsString($"{prefsId}");
        return value;
    }
    public void SetString(EnPlayerPrefsId prefsId, string value)
    {
        PlatformUtility.PlatformAnalysis.SetPlayerPrefsString($"{prefsId}", value);
    }

    public T GetStruct<T>(EnPlayerPrefsId prefsId)
    {
        var json = GetString(prefsId);
        var data = JsonConvert.DeserializeObject<T>(json);
        return data;
    }
    public void SetStruct<T>(EnPlayerPrefsId prefsId, T data)
    {
        var json = JsonConvert.SerializeObject(data);
        SetString(prefsId, json);
    }
}
