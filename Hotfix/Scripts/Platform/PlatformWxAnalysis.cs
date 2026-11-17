#if UNITY_WEBGL && UNITY_MINIGAME_WX
#define UNITY_WX
#endif

#if UNITY_WX
using System;
using System.Collections;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using WeChatWASM;

[Serializable]
public class WxPhoneRequest
{
    public string code;
}

[Serializable]
public class WxPhoneResponse
{
    public bool success;
    public string phoneNumber;
    public string purePhoneNumber;
    public string countryCode;
    // 用于接收后端的错误信息
    public string detail;
}
public class PlatformWxAnalysis : IPlatformAnalysys
{
    public string GetPlayerPrefsString(string key)
    {
        var value = WX.StorageGetStringSync(key, null);
        return value;
    }

    public void SetPlayerPrefsString(string key, string value)
    {
        WX.StorageSetStringSync(key, value);
    }
    public float GetPlayerPrefsFloat(string key)
    {
        var value = WX.StorageGetFloatSync(key, -1);
        return value;
    }

    public void SetPlayerPrefsFloat(string key, float value)
    {
        WX.StorageSetFloatSync(key, value);
    }

    public int GetPlayerPrefsInt(string key)
    {
        var value = WX.StorageGetIntSync(key, -1);
        return value;
    }
    public void SetPlayerPrefsInt(string key, int value)
    {
        WX.StorageSetIntSync(key, value);
    }

    public void RegisterTmpInputField(TMP_InputField inputField)
    {
        if (!inputField.TryGetComponent<WXInputFieldTmpAdapter>(out var _))
            inputField.gameObject.AddComponent<WXInputFieldTmpAdapter>();
    }
    public void Init()
    {
        WX.OnHide(HideCallback);

        UIMgr.Instance.ShowWindow<UIWxStartGameWindowData>();

    }

    private void HideCallback(GeneralCallbackResult result)
    {
        PersistentMgr.Instance.UploadPeresistentData(false);
    }

    public void ShowAd()
    {

    }
}
#endif