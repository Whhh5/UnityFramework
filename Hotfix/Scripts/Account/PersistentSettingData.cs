using UnityEngine;
using System.Collections;
using Newtonsoft.Json;

public class PersistentSettingData : PersistentDataBase
{
    [JsonProperty]
    private bool _AutoFireBall = true;
    [JsonProperty]
    private bool _IsShowDamageTxt = true;


    public void SetAutoFireBall(bool count)
    {
        _AutoFireBall = count;
        SetDirty(true);
    }
    public bool GetAutoFireBall()
    {
        return _AutoFireBall;
    }

    public void SetIsShowDamageTxt(bool count)
    {
        _IsShowDamageTxt = count;
        SetDirty(true);
    }
    public bool GetIsShowDamageTxt()
    {
        return _IsShowDamageTxt;
    }
}