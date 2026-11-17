using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PersistentAccountData : PersistentDataBase
{
    [JsonProperty]
    private int _GameCount;
    //[JsonProperty]
    //private int _Level;



    public void SetGameCount(int count)
    {
        _GameCount = count;
        SetDirty(true);
    }
    public int GetGameCount()
    {
        return _GameCount;
    }
    public void SetIsFirstGame(bool isFirstGame)
    {
        _GameCount = isFirstGame ? 0 : 1;
        SetDirty(true);
    }
    public bool IsFirstGame()
    {
        return _GameCount == 0;
    }
    //public void SetLevel(int level)
    //{
    //    _Level = level;
    //}
    //public int GetLevel()
    //{
    //    return _Level;
    //}
}
