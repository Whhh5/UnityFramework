
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PoolNaNUserData : IClassPoolDestroy
{
    public void OnPoolDestroy()
    {
    }
}
public class ABBEventDataUserData : IClassPoolDestroy
{
    public EnABBEvent abbevent;

    public void OnPoolDestroy()
    {
        abbevent = EnABBEvent.NONE;
    }
}
public class Entity3DComDataUserData : IClassPoolDestroy
{
    public int entityID;
    public void OnPoolDestroy()
    {
        entityID = -1;
    }
}
public class EntityBuffDataUserData : IClassPoolDestroy
{
    public int targetEntityID;
    public int sourceEntityID;
    public EnBuff buff;
    public void OnPoolDestroy()
    {
        targetEntityID = -1;
        buff = EnBuff.None;
    }
}
public class LayerMixerInfoUserData : IClassPoolDestroy
{
    public EnAnimLayer layer;
    public ScriptPlayable<BridgePlayableAdapter> layerAdapter;
    public void OnPoolDestroy()
    {
        layer = EnAnimLayer.None;
        layerAdapter = ScriptPlayable<BridgePlayableAdapter>.Null;
    }
}
public class PlayableAdapterUserData : IClassPoolDestroy
{
    public PlayableGraphAdapter graph;
    public Playable rootPlayable;

    public virtual void OnPoolDestroy()
    {
        rootPlayable = default;
        graph = null;
    }
}
public class PlayableGraphAdapterUserData : IClassPoolDestroy
{
    public int entityID;
    public Animator anim;

    public void OnPoolDestroy()
    {
        entityID = -1;
        anim = null;
    }
}


public abstract class AttackLinkUserData : IClassPoolDestroy
{
    public abstract void OnPoolDestroy();
}

public abstract class PlayableBehaviourAdapterUserData : IClassPoolDestroy
{
    public virtual void OnPoolDestroy()
    {

    }
}
public abstract class CmdPlayableAdapterUserData : PlayableAdapterUserData
{

}

public class SkillBehaviourUserData : IClassPoolDestroy
{
    public int[] arrValue;
    public void OnPoolDestroy()
    {
        arrValue = null;
    }
}
public sealed class GeneralIntUserData : IClassPoolDestroy
{
    public int intValue;
    public void OnPoolDestroy()
    {
        intValue = -1;
    }
}
public sealed class GeneralInt2UserData : IClassPoolDestroy
{
    public int intValue;
    public int intValue2;
    public void OnPoolDestroy()
    {
        intValue
            = intValue2
            = -1;
    }
}

public class AIModuleUserData : IClassPoolDestroy
{
    public int entityID;
    public int aiModuleCfgID;
    public int moduleDataID;
    public virtual void OnPoolDestroy()
    {
        aiModuleCfgID
            = entityID
            = moduleDataID
            = -1;
    }
}

