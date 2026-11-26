using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public interface IPersistentData
{
    public bool IsDirty();
    public void SetDirty(bool isDirty);
}
public class PersistentMgr : Singleton<PersistentMgr>
{
    public override EnSingletonOrder SingletonOrder => EnSingletonOrder.PersistentMgr;
    private Dictionary<EnPersistentId, string> _PersistentString = new();
    private readonly Dictionary<EnPersistentId, IPersistentData> _Persistents = new(20);
    private float _LastUploadTime = float.MinValue;
    private readonly float _UploadDelay = 5f;
    public override void Destroy()
    {
        UploadPeresistentData(false);
        base.Destroy();
    }
    public override async UniTask AwakeAsync()
    {
        await base.AwakeAsync();


    }
    public async UniTask InitPersistentData()
    {
        var path = GetPersistentFilePath();
        var perData = await WebUtil.DownloadServerFileAsync(path);
        var perStr = Encoding.UTF8.GetString(perData);
        _PersistentString = JsonConvert.DeserializeObject<Dictionary<EnPersistentId, string>>(perStr) ?? new();

        var accountData = PersistentMgr.Instance.GetData<PersistentAccountData>(EnPersistentId.Account);
        var gameCount = accountData.GetGameCount();
        accountData.SetGameCount(++gameCount);
        if (gameCount == 1)
        {
            EventMgr.Instance.FireExecute(EnABBEvent.ENTITY_APP_FIRST);
        }
    }
    public void UploadPeresistentData(bool isDelay = true)
    {
        var curTime = ABBUtil.GetGameTimeSeconds();
        if (isDelay && curTime - _LastUploadTime < _UploadDelay)
        {
            AotDebug.LogWarning($"upload persistent wait {curTime - _LastUploadTime}s");
            return;
        }
        var isUpload = false;
        foreach (var (perId, data) in _Persistents)
        {
            if (!data.IsDirty())
                continue;
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            _PersistentString[perId] = json;
            data.SetDirty(false);
            isUpload = true;
        }
        if (!isUpload)
            return;
        _LastUploadTime = curTime;

        var jsonStr = JsonConvert.SerializeObject(_PersistentString);
        var path = GetPersistentFilePath();
        WebUtil.UploadFileAsync(path, jsonStr).Forget();
    }
    private string GetPersistentFilePath()
    {
        var accId = AccountMgr.Instance.GetAccoundId();
        var root = GlobalConfigSO.Instance.GetPersistentRoot();
        var path = Path.Combine(root, $"{accId}.data");
        return path;
    }
    public TValue GetPerisitentData<TValue>(EnPersistentId perId)
        where TValue : PersistentDataBase, new()
    {
        if (!_Persistents.TryGetValue(perId, out var perData))
        {
            if (_PersistentString.TryGetValue(perId, out var perstr))
                perData = JsonConvert.DeserializeObject<TValue>(perstr);
            else
                perData = new TValue();
            _Persistents.Add(perId, perData);
        }
        return perData as TValue;
    }
    public TValue GetData<TValue>(EnPersistentId perId)
        where TValue : PersistentDataBase, new()
    {
        var perData = GetPerisitentData<TValue>(perId);
        return perData;
    }
    public void SetDirty(EnPersistentId perId)
    {
        if (!_Persistents.TryGetValue(perId, out var perData))
            return;
        perData.SetDirty(true);
    }
    public void ClearAllPersistentData()
    {
        _PersistentString.Clear();
        _Persistents.Clear();
    }
    public void ClearPersistentData(EnPersistentId perId)
    {
        _PersistentString.Remove(perId);
        _Persistents.Remove(perId);
    }

}
