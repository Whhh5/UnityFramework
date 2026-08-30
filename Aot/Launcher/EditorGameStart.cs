

using Cysharp.Threading.Tasks;

public class EditorGameStart : IGameStart
{
    public async UniTask<bool> AwakeAsync()
    {
        await UniTask.DelayFrame(0);
        return true;
    }

    public void Destroy()
    {

    }

    public async UniTask<bool> StartAsync()
    {
        await UniTask.DelayFrame(0);
        return true;
    }
}