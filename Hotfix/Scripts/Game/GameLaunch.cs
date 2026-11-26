using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{

    private void Start()
    {
        if (ABBUtil.TryGetClassesWithBaseClass<IStartLaunch>(out var launch))
        {
            var ins = Activator.CreateInstance(launch) as IStartLaunch;
            ins.StartLaunch();
        }
        else
        {
            //ShowWindow<CGameStartWindowData>();
            ABBUtil.LogError($"没有找到启动界面，请继承 {typeof(IStartLaunch)}");
        }
    }
}
