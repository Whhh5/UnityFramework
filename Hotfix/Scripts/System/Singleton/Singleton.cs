using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

public enum EnManagerFuncType
{
    None = 0,
    AwakeAsync = 1 << 0,
    OnEnableAsync = 1 << 1,
    Update = 1 << 2,
    FixedUpdate = 1 << 3,
}
public enum EnSingletonOrder
{
    None,
    Load,
    GameSchedule,
    Server,
    Base,
    Default,
}
public abstract class Singleton
{
    public virtual EnSingletonOrder SingletonOrder => EnSingletonOrder.Default;
    public virtual EnManagerFuncType FuncType => EnManagerFuncType.AwakeAsync | EnManagerFuncType.OnEnableAsync;

    protected CancellationTokenSource _LifeToken;
    public virtual async UniTask AwakeAsync()
    {
        _LifeToken = new();
        await UniTask.DelayFrame(1);
    }
    public virtual async UniTask OnEnableAsync()
    {
        await UniTask.DelayFrame(1);
    }
    public virtual void OnDisable()
    {

    }
    public virtual void Destroy()
    {
        _LifeToken?.Cancel();
        _LifeToken = null;
    }
    public virtual void Update()
    {

    }
    public virtual void FixedUpdate()
    {

    }
    public virtual bool IsUpdate()
    {
        return true;
    }

    public CancellationToken GetToken()
    {
        return _LifeToken.Token;
    }
    ~Singleton()
    {
        _LifeToken?.Cancel();
    }
}
public abstract class Singleton<T> : Singleton
    where T : class, new()
{
    public static T Instance = new();
}

