using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnDescriptionLevel
{
    None,
    Level1,
    Level2,
    Level3,
    Level4,
    Count,
}
public class DescriptionInfo : IClassPoolDestroy
{
    public EnDescriptionLevel level;
    public string content;
    public string title;
    public void OnPoolDestroy()
    {
        level = EnDescriptionLevel.None;
        content = null;
        title = null;
    }
}
public interface IDescription
{
    public void GetDescriptionList(ref List<DescriptionInfo> descInfos, EnDescriptionLevel level);

}
