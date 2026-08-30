using System.Collections.Generic;
using Newtonsoft.Json;

public class PersistentCharacterData : PersistentDataBase
{
    private class CharacterData : IClassPoolDestroy
    {
        public int level;
        public bool isLock = true;
        public Dictionary<EnEntityAttributeId, int> aptitudeInfo = new((int)EnEntityAttributeId.EnumCount);

        public void OnPoolDestroy()
        {
            level = 0;
            isLock = true;
            aptitudeInfo.Clear();
        }
    }
    [JsonProperty]
    private int _ActiveCharacterId = -1;
    [JsonProperty]
    private readonly Dictionary<int, CharacterData> _CharacterId2Datas = new();

    private CharacterData GetCharacterData(int characterId)
    {
        if (!_CharacterId2Datas.TryGetValue(characterId, out var data))
        {
            data = ClassPoolMgr.Instance.Pull<CharacterData>();
            _CharacterId2Datas.Add(characterId, data);
        }
        return data;
    }

    public void SetLevel(int characterId, int level)
    {
        var data = GetCharacterData(characterId);
        data.level = level;
        SetDirty(true);
    }
    public void SetIsLock(int characterId, bool isLock)
    {
        var data = GetCharacterData(characterId);
        data.isLock = isLock;
        SetDirty(true);
    }
    public void SetActiveCharacterId(int characterId)
    {
        _ActiveCharacterId = characterId;
        SetDirty(true);
    }
    public void SetAptitudeValue(int characterId, EnEntityAttributeId attId, int value)
    {
        var data = GetCharacterData(characterId);
        data.aptitudeInfo[attId] = value;
        SetDirty(true);
    }

    public int GetLevel(int characterId)
    {
        if (!_CharacterId2Datas.TryGetValue(characterId, out var data))
            return 0;
        return data.level;
    }
    public bool IsLock(int characterId)
    {
        if (!_CharacterId2Datas.TryGetValue(characterId, out var data))
            return true;
        return data.isLock;
    }
    public int GetActiveCharacterId()
    {
        return _ActiveCharacterId;
    }
    public int GetAptitudeValue(int characterId, EnEntityAttributeId attId)
    {
        if (!_CharacterId2Datas.TryGetValue(characterId, out var data))
            return 0;
        if (!data.aptitudeInfo.TryGetValue(attId, out var value))
            return 0;
        return value;
    }
}
