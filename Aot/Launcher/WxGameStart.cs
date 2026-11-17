#if UNITY_WEBGL && UNITY_MINIGAME_WX
#define UNITY_WX
#endif

#if UNITY_WX
using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using WeChatWASM;

public class WxGameStart : IGameStart
{


    public void Destroy()
    {
    }
    public async UniTask<bool> AwakeAsync()
    {
        var sdkId = -1;
        WX.InitSDK(sdkID =>
        {
            sdkId = sdkID;
            AotDebug.LogWarning($"WXSDKID: {sdkID}");
        });
        await UniTask.WaitUntil(() => sdkId != -1);

        return true;
    }
    public async UniTask<bool> StartAsync()
    {
        WX.ReportGameStart();
        await UniTask.DelayFrame(0);
        return true;
    }


}
#endif