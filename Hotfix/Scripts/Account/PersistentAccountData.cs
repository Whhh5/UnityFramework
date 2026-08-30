using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class PersistentAccountData : PersistentDataBase
{
    [JsonProperty]
    private int _Exp;
    //[JsonProperty]
    //private int _Level;



    public void SetExp(int exp)
    {
        _Exp = exp;
    }
    public int GetExp()
    {
        return _Exp;
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
