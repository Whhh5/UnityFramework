using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;

public partial class AccountMgr : Singleton<AccountMgr>
{
    private static string _AccountId;

    private struct AccountLevelInfo
    {
        public int localExp;
        public int globalExp;
        public AccountLevelInfo(int localExp, int globalExp)
        {
            this.localExp = localExp;
            this.globalExp = globalExp;
        }
    }
    private readonly List<AccountLevelInfo> _Level2ExpInfo = new();
    private int _MaxLevel;
    private int _CurLevel;
    private bool _IsLogin = false;

    public override void Destroy()
    {
        EventMgr.Instance.Unregister(EnABBEvent.EVENT_ACCOUNT_LOGIN, OnEvent_AccountLogin);
        //EventMgr.Instance.Unregister<GameGoodsChangeEventData>(EnABBEvent.EVENT_GOODS_CHANGE, (int)EnGoodsId.Exp, 0, OnEvent_ExpChange);
        base.Destroy();
        _Level2ExpInfo.Clear();
        _MaxLevel
            = _CurLevel
            = -1;
    }
    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();
        var levelCount = GameSchedule.Instance.GetAccountLevelCfgCount();
        var exp = 0;
        for (int i = 0; i < levelCount; i++)
        {
            var levelCfg = GameSchedule.Instance.GetAccountLevelCfg(i);
            exp += levelCfg.nExp;
            _Level2ExpInfo.Add(new(levelCfg.nExp, exp));
        }
        _MaxLevel = _Level2ExpInfo.Count;

        //EventMgr.Instance.Register<GameGoodsChangeEventData>(EnABBEvent.EVENT_GOODS_CHANGE, (int)EnGoodsId.Exp, 0, OnEvent_ExpChange);
        EventMgr.Instance.Register(EnABBEvent.EVENT_ACCOUNT_LOGIN, OnEvent_AccountLogin);
    }
    private void OnEvent_AccountLogin(in int _, in int __)
    {
        _CurLevel = CalculateLevel();
    }
    public void SetAccountId(string accountId)
    {
        _AccountId = accountId;
    }
    public string GetAccoundId()
    {
        return _AccountId;
    }

    public int GetLevel()
    {
        return _CurLevel;
    }
    private int CalculateLevel()
    {
        var exp = GetExp();
        var testLevel = _CurLevel;

        if (_CurLevel == _MaxLevel || exp < _Level2ExpInfo[testLevel].globalExp)
        {
            for (int i = testLevel - 1; i > 0; i--)
            {
                var testLevelInfo = _Level2ExpInfo[i];
                if (exp >= testLevelInfo.globalExp)
                    return i + 1;
            }
            return 0;
        }
        else
        {
            for (int i = testLevel + 1; i < _MaxLevel; i++)
            {
                var testLevelInfo = _Level2ExpInfo[i];
                if (exp < testLevelInfo.globalExp)
                    return i;
            }
            return _MaxLevel;
        }
    }

    private void UpdateLevel()
    {
        var level = CalculateLevel();
        if (level == _CurLevel)
            return;
        var lastLevel = _CurLevel;
        _CurLevel = level;

        //var evUd = ClassPoolMgr.Instance.Pull<GameAccountLevelChangeEventData>();
        //evUd.Setting(lastLevel, level);
        //EventMgr.Instance.FireExecute(EnABBEvent.EVENT_ACCOUNT_LEVEL_CHANGE, evUd);
        //ClassPoolMgr.Instance.Push(evUd);
    }
    //private void OnEvent_ExpChange(in int goodsCfgId, in int _, in GameGoodsChangeEventData data)
    //{
    //    UpdateLevel();
    //}

    public void ChangeExp(int exp)
    {
        //UIGoodsMgr.Instance.AddGoods((int)EnGoodsId.Exp, exp);

        UpdateLevel();
    }
    public int GetExp()
    {
        //var exp = UIGoodsMgr.Instance.GetGoodsCount((int)EnGoodsId.Exp);
        //return exp;
        return 9;
    }
    public float GetExpSliderValue()
    {
        var exp = GetExp();
        if (_CurLevel > 0)
        {
            var lastLevelInfo = _Level2ExpInfo[_CurLevel - 1];
            exp -= lastLevelInfo.globalExp;
        }
        if (_CurLevel == _MaxLevel)
            return 1;
        var levelInfo = _Level2ExpInfo[_CurLevel];
        var localExp = levelInfo.localExp;

        var value = Mathf.Clamp01((float)exp / localExp);
        return value;
    }
    public async void LoginGame(string account)
    {
        if (_IsLogin)
            return;
        _IsLogin = true;
        AccountMgr.Instance.SetAccountId(account);
        await PersistentMgr.Instance.InitPersistentData();
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_ACCOUNT_LOGIN);
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_SHOW_LOBBY);
    }
}
