using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Threading;

public class GOData : IClassPoolDestroy
{
    public EnLoadTarget loadTarget;
    public GameObject go;
    public AsyncInstantiateOperation<GameObject> insOperation = null;

    public void OnPoolDestroy()
    {
        loadTarget = EnLoadTarget.None;
        go = null;
        insOperation = null;
    }
}
public class GoPoolItem : IClassPoolDestroy
{
    public GameObject tempGo;
    public List<GameObject> gos = new();
    public float desTime;
    public int count;

    public void OnPoolDestroy()
    {
        count = 0;
        gos.Clear();
        desTime = -1;
        tempGo = null;
    }
}
public class ABBGOMgr : Singleton<ABBGOMgr>
{
    public override EnManagerFuncType FuncType => base.FuncType | EnManagerFuncType.Update;
    private readonly Dictionary<int, GOData> _GOMap = new();
    private readonly Dictionary<EnLoadTarget, GoPoolItem> _GoPools = new();

    private GameObject _GoPoolRoot = null;
    private readonly float _DelayDestroyTime = 10f;

    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();
        _GoPoolRoot = new GameObject("[Gameobject Pool]");
        GameObject.DontDestroyOnLoad(_GoPoolRoot);
        //_GoPoolRoot.hideFlags |= HideFlags.HideInHierarchy;
        _GoPoolRoot.SetActive(false);
    }
    public GameObject GetGo(int goID)
    {
        if (!_GOMap.TryGetValue(goID, out var goData))
            return null;
        return goData.go;
    }
    public T GetGoCom<T>(in int goID)
    {
        var go = GetGo(goID);
        var com = go.GetComponent<T>();
        return com;
    }
    public async UniTask<int> CreateGOAsync(EnLoadTarget target, Transform parent = null)
    {
        if (!_GoPools.TryGetValue(target, out var poolItem))
        {
            poolItem = ClassPoolMgr.Instance.Pull<GoPoolItem>();
            _GoPools.Add(target, poolItem);
        }

        var goData = ClassPoolMgr.Instance.Pull<GOData>();
        if (poolItem.gos.Count > 0)
        {
            goData.go = poolItem.gos[^1];
            goData.go.transform.SetParent(parent);
            poolItem.gos.RemoveAt(poolItem.gos.Count - 1);
            poolItem.desTime = GetTime() + _DelayDestroyTime;
        }
        else
        {
            if (poolItem.tempGo == null)
            {
                var obj = await ABBLoadMgr.Instance.LoadAsync<GameObject>(target, GetToken());
                if (obj == null)
                    return -1;
                poolItem.tempGo = obj;
            }

            goData.insOperation = GameObject.InstantiateAsync(poolItem.tempGo, parent);
            var ins = await goData.insOperation.ToUniTask(cancellationToken: GetToken());
            goData.insOperation = null;
            goData.go = ins[0];
        }
        poolItem.count++;
        var goID = ABBUtil.GetTempKey();
        _GOMap.Add(goID, goData);

        goData.loadTarget = target;
#if UNITY_EDITOR
        goData.go.name = $"{poolItem.tempGo.name} (Clone)_{goID}";
#endif
        return goID;
    }
    public void DestroyGO(int goID)
    {
        if (!_GOMap.TryGetValue(goID, out var goData))
            return;
        var poolItem = _GoPools[goData.loadTarget];
        poolItem.count--;
        if (goData.insOperation != null)
        {
            goData.insOperation.Cancel();
            goData.insOperation = null;
        }
        else
        {
            goData.go.transform.SetParent(_GoPoolRoot.transform);
            poolItem.gos.Add(goData.go);
            poolItem.desTime = GetTime() + _DelayDestroyTime;
        }
        //var loadTarget = goData.loadTarget;
        _GOMap.Remove(goID);

        ClassPoolMgr.Instance.Push(goData);
    }
    public override void Update()
    {
        base.Update();

        if (TryGetDestroyTargetId(out var target))
        {
            var poolItem = _GoPools[target];
            var go = poolItem.gos[^1];
            GameObject.Destroy(go);
            poolItem.gos.RemoveAt(poolItem.gos.Count - 1);

            if (poolItem.count == 0 && poolItem.gos.Count == 0)
            {
                _GoPools.Remove(target);
                ClassPoolMgr.Instance.Push(poolItem);
                ABBLoadMgr.Instance.Unload(target);
            }
        }
    }
    public void ClearGoCache()
    {

    }

    private bool TryGetDestroyTargetId(out EnLoadTarget loadTarget)
    {
        foreach (var (goId, poolItem) in _GoPools)
        {
            if (poolItem.desTime > GetTime())
                continue;
            if (poolItem.gos.Count == 0)
                continue;
            loadTarget = goId;
            return true;
        }
        loadTarget = EnLoadTarget.None;
        return false;
    }

    public float GetTime()
    {
        return Time.time;
    }
}

