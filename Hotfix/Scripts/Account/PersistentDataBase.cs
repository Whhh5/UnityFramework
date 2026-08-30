using Newtonsoft.Json;

public abstract class PersistentDataBase : IPersistentData
{
    [JsonIgnore]
    private bool _IsDirty;
    public bool IsDirty()
    {
        return _IsDirty;
    }
    public void SetDirty(bool isDirty)
    {
        _IsDirty = isDirty;
    }
}