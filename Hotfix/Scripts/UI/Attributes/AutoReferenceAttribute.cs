using System;

public enum SearchScope
{
    None,
    Self,
    Children,
}

[AttributeUsage(AttributeTargets.Field)]
public class AutoReferenceAttribute : Attribute
{
    public string objName;
    public SearchScope scope = SearchScope.Children;

    public AutoReferenceAttribute() { }
    public AutoReferenceAttribute(string name, SearchScope scope = SearchScope.Children)
    {
        objName = name;
        this.scope = scope;
    }
    public AutoReferenceAttribute(SearchScope scope)
    {
        this.scope = scope;
    }
}