using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;

public class AccountMgr : Singleton<AccountMgr>
{
    private static int _AccountId;

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

    public override void Destroy()
    {
        base.Destroy();
        _Level2ExpInfo.Clear();
        _MaxLevel = -1;
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

        _CurLevel = CalculateLevel();
    }
    public void SetAccountId(int accountId)
    {
        _AccountId = accountId;
    }
    public int GetAccoundId()
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

        var evUd = ClassPoolMgr.Instance.Pull<GameAccountLevelChangeEventData>();
        evUd.Setting(lastLevel, level);
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_ACCOUNT_LEVEL_CHANGE, evUd);
        ClassPoolMgr.Instance.Push(evUd);
    }

    public void ChangeExp(int exp)
    {
        var perData = PersistentMgr.Instance.GetData<PersistentAccountData>(EnPersistentId.Account);
        var curExp = perData.GetExp();
        var toExp = curExp + exp;
        perData.SetExp(toExp);

        var evUd = ClassPoolMgr.Instance.Pull<GameAccountExpChangeEventData>();
        evUd.Setting(curExp, toExp);
        EventMgr.Instance.FireExecute(EnABBEvent.EVENT_ACCOUNT_EXP_CHANGE, evUd);
        ClassPoolMgr.Instance.Push(evUd);

        UpdateLevel();
    }
    public int GetExp()
    {
        var perData = PersistentMgr.Instance.GetData<PersistentAccountData>(EnPersistentId.Account);
        var exp = perData.GetExp();
        return exp;
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
    //public void SetLevel(int level)
    //{
    //    var perData = PersistentMgr.Instance.GetData<PersistentAccountData>(EnPersistentId.Account);
    //    perData.SetLevel(level);
    //}
    //public int GetLevel()
    //{
    //    var perData = PersistentMgr.Instance.GetData<PersistentAccountData>(EnPersistentId.Account);
    //    var level = perData.GetLevel();
    //    return level;
    //}
}
