using System.Collections.Generic;
using Newtonsoft.Json;

public class PersistentChapterData : PersistentDataBase
{
    public class ChapterData
    {
        public bool isUnlock;
    }
    [JsonProperty]
    private int _CurSelectChapterId;
    [JsonProperty]
    private int _MaxSuccessChapterId;
    [JsonProperty]
    private readonly Dictionary<int, ChapterData> _ChapterDatas = new();

    private ChapterData GetChapterData(int chaoterId)
    {
        if (!_ChapterDatas.TryGetValue(chaoterId, out var chapterData))
        {
            chapterData = new();
            _ChapterDatas.Add(chaoterId, chapterData);
        }
        return chapterData;
    }
    public int GetCurSelectChapterId()
    {
        return _CurSelectChapterId;
    }
    public void SetCurSelectChapter(int chapterId)
    {
        _CurSelectChapterId = chapterId;
        SetDirty(true);
    }
    public int GetMaxSuccessChapterId()
    {
        return _MaxSuccessChapterId;
    }
    public void SetMaxSuccessChapterId(int chapterId)
    {
        _MaxSuccessChapterId = chapterId;
        SetDirty(true);
    }
    public bool GetIsUnlock(int chapterId)
    {
        if (!_ChapterDatas.TryGetValue(chapterId, out var chapterData))
            return false;
        return chapterData.isUnlock;
    }
    public void SetIsUnlock(int chapterId, bool isUnlock)
    {
        var data = GetChapterData(chapterId);
        data.isUnlock = isUnlock;
        SetDirty(true);
    }

}

