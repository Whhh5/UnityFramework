
using Cysharp.Threading.Tasks;
using WeChatWASM;

public class WxGameStart : IGameStart
{
    private enum EnWxLoginResult
    {
        None,
        Success,
        Faild,
        Timeout,
    }
    private EnWxLoginResult _LoginResult = EnWxLoginResult.None;
    public async UniTask<bool> AwakeAsync()
    {
        var sdkId = -1;
        WeChatWASM.WX.InitSDK(sdkID =>
        {
            sdkId = sdkID;
            AotDebug.LogWarning($"WXSDKID: {sdkID}");
        });
        await UniTask.WaitUntil(() => sdkId != -1);
        return true;
    }
    public async UniTask<bool> StartAsync()
    {
        _LoginResult = EnWxLoginResult.None;
        WeChatWASM.WX.Login(new() { success = LoginSuccess, fail = LoginFaild });
        await UniTask.WaitUntil(() => _LoginResult != EnWxLoginResult.None);
        if (_LoginResult != EnWxLoginResult.Success)
            throw new System.Exception($"wx login error: {_LoginResult}");

        WeChatWASM.WX.ReportGameStart();
        return true;
    }

    public void LoginSuccess(LoginSuccessCallbackResult result)
    {
        _LoginResult = EnWxLoginResult.Success;
        AotDebug.LogWarning($"login info code:{result.code}, errMsg:{result.errMsg}");
        //var phoneNumber = -1;
        WX.GetPhoneNumber(new GetPhoneNumberOption()
        {
            isRealtime = true,
            success = (getPhoneResult) =>
            {
                if (getPhoneResult.errno == 0)
                {
                    AotDebug.LogWarning($"getphone code:{getPhoneResult.code}");
                    //phoneNumber = 1;
                }
            },
        });

    }
    public void LoginFaild(LoginFailCallbackErr result)
    {
        _LoginResult = EnWxLoginResult.Faild;
        AotDebug.LogError($"login errno:{result.errno}, error:{result.errMsg}");
    }

    public void Destroy()
    {

    }

}