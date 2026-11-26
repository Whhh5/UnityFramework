
using System;
using System.Collections.Generic;

public enum EnTypeId
{
    None = 0,

    ____________________________________________________________VirtualTimelineStart = 1000,
    VirtualSkillTimeline,
    _______________________________VirtualTimelineEventStart = 1100,
    VirtualTimelineDefaultEvent,

    ____________________________________________________________SkillTypeStart = 2000,
    SkillVirtualTimelineNodePlayableInfo,
    SkillVirtualTimelineEventPlayableInfo,
    _______________________________SkillVirtualTimelineEventStart = 2100,
    SkillTimelinePhysicsSphereEvent,
    SkillTimelinePhysicsCylinderEvent,
    SkillTimelineAttackCylinderEvent,
    SkillTypeSingletonPlayableAdapter,
    LayerMixerCmdPlayableAdapter,
    SkillLinkPlayableAdapter,
    SkillLoopPlayableAdapter,
    SkillRandomPlayableAdapter,
    SkillTypeSelectPlayableAdapter,
    _______________________________SkillPlayableNodeStart = 2200,
    SkillVirtualTimelineAnimationPlayableNode,
    _______________________________SkillPlayableItemDataStart = 2300,
    SkillItemInfo,
    SkillTypeLoopData,
    SkillTypeSelectData,
    SkillTypeSelectItemInfo,
    PhysicsResolveBox,
    PhysicsResolveSphere,
    SkillHeightBehaviourData,
    CompareLessInfo,
    CompareGreaterInfo,
    CompareEqualInfo,
    EntityBuffTimeDefaultInfo,
    EntityMoveDownBuffUserData,
    PhysicsResolveCylinder,
    EntityNoMovementBuffData,
    EntityNoRotationBuffData,
    _______________________________SkillScheduleStart = 2400,
    SkillBehaviourScheduleAction,
    SkillBuffScheduleAction,
    SkillEffectScheduleAction,
    SkillPhysicsScheduleAction,
    SkillFlyItemScheduleAction,
    SkillEventScheduleAction,
    SkillAudioScheduleAction,

    ____________________________________________________________EntityTypeIdStart = 3000,
    PlayerEntityData,
    DefaultSceneProjectileEntityData,
    _______________________________Attachment = 3100,
    MapChunkAttachment_TowerEntityData,
    _______________________________Monster = 3200,
    MonsterEntityData,
    MonsterAnnaEntityData,
    MonsterAshuEntityData,




    ____________________________________________________________Ai = 4000,
    _______________________________AiDecision = 4100,
    AiDefaultDecision,
    AiTransformDecisionSubGenerate,
    AiDirectionDecisionSubGenerator,
    _______________________________AiDecisionGenerator = 4200,
    AiComAtkDecisionGenerator,
    AiIdleDecisionGenerator,
    AiCmdDecisionGenerator,
    _______________________________AiCondition = 4300,
    AiDistanceCondition,
    AiRotationCondition,
    _______________________________AiMisc = 4400,
    AiPerceptionMatchInfo,
    AiDecisionGeneratorInfo,
    _______________________________AiPerception = 4500,
    AiRandomEntityPerception,
    AiPerceptionInfos,
    AiDecisionInfos,
    _______________________________AiPerceptionTest = 4600,
    AiPerceptionLayerTest,
    AiPerceptionDistanceTest,


    // game
    _______________________________GameMonster = 4700,
    GameBattleMonster1000101EntityData,
    GameBattleMonster1000102EntityData,
    GameBattleMonster1000103EntityData,
    GameBattleMonster1000104EntityData,
    GameBattleMonster1000201EntityData,
    GameBattleMonster1000301EntityData,
    GameBattleMapBoss1001101EntityData,
    _______________________________GameMonster2 = 4900,
    GameBattleLeechEntity,
    _______________________________GameCharacter = 5000,
    GameBattleCharacter1EntityData,
    GameUICharacter1EntityData,
    GameBattleCharacter2EntityData,
    GameUICharacter2EntityData,
    _______________________________EntityBuff = 5100,
    EntityVulnerableBuffData,
    EntityBurnBuffData,
    EntityCrystalBuffData,
    EntityShatteredArmorBuffData,
    EntityFreezeBuffData,
    EntityElectricBuffData,
    EntityBleedBuffData,
    _______________________________SkillBall = 5300,
    GameBattleSkillBall1Data = 5301,
    GameBattleSkillBall101Data = 5321,
    GameBattleSkillBall1001Data = 5351,
    GameBattleSkillBall1002Data,
    GameBattleSkillBall1003Data,
    GameBattleSkillBall1004Data,
    GameBattleSkillBall1005Data,
    GameBattleSkillBall1006Data,
    GameBattleSkillBall1007Data,
    GameBattleSkillBall1008Data,
    GameBattleSkillBall1009Data,
    GameBattleSkillBall1010Data,
    GameBattleSkillBall1011Data,
    GameBattleSkillBall1012Data,
    GameBattleSkillPassivity2001Data = 5401,
    GameBattleSkillPassivity2002Data,
    GameBattleSkillPassivity2003Data,
    GameBattleSkillPassivity2004Data,
    GameBattleSkillPassivity2005Data,
    GameBattleSkillPassivity2006Data,
    GameBattleSkillPassivity2007Data,
    GameBattleSkillPassivity2008Data,

    GameBattleSkillBall10071011Data = 5501,
    GameBattleSkillBall10091010Data,
    GameBattleSkillBall10021006Data,
    GameBattleSkillBall10071012Data,
    GameBattleSkillBall10011008Data,
    GameBattleSkillBall10031004Data,

    GameBattleSkillBallPart1001Data = 5901,
    GameBattleSkillBallPart1002Data,
    GameBattleSkillBallPart1003Data,
    GameBattleSkillBallPart1004Data,
    GameBattleSkillBallPart1005Data,
    GameBattleSkillBallPart1006Data,
    GameBattleSkillBallPart1007Data,
    GameBattleSkillBallPart1008Data,
    GameBattleSkillBallPart1009Data,
    GameBattleSkillBallPart1010Data,
    GameBattleSkillBallPart1011Data,
    GameBattleSkillBallPart1012Data,
    GameBattleSkillBallPartInfo = 6001,


    _______________________________Wave = 6100,
    GameBattleWaveMonsterInfo,
    _______________________________SkillEvent = 6200,
    GameSkillMonsterAttack1EventData,
    GameSkillMonsterDieEventData,
    GameSkillBossSummonEventData,
    GameSkillBossDebutEventData,

    _______________________________SkillBallFuission = 6300,
    GameBattleSkillBallPart10071011Data,
    GameBattleSkillBallPart10091010Data,
    GameBattleSkillBallPart10021006Data,
    GameBattleSkillBallPart10071012Data,
    GameBattleSkillBallPart10011008Data,
    GameBattleSkillBallPart10031004Data,
}
public static class TypeDefine
{

    private static readonly Dictionary<int, Func<IClassPool, IClassPool>> _ClassData = new();

    public static void Register<T>(EnTypeId typeId)
        where T: IClassPool, new()
    {
        Register<T>((int)typeId);
    }
    public static void Register<T>(int typeId)
    where T : IClassPool, new()
    {
        var func = Resiter_ClassData<T>(typeId);
        _ClassData.Add(typeId, func);
    }

    private static Func<IClassPool, IClassPool> Resiter_ClassData<T>(int typeId)
        where T : IClassPool, new()
    {
        return (data) =>
        {
            return ClassPoolMgr.Instance.Pull<SkillVirtualTimelineNodePlayableInfo>(data);
        };
    }

    public static IClassPool CreateInstance(in int typeId, in IClassPool userData)
    {
        return (EnTypeId)typeId switch
        {
            EnTypeId.None => null,


            EnTypeId.SkillVirtualTimelineNodePlayableInfo => ClassPoolMgr.Instance.Pull<SkillVirtualTimelineNodePlayableInfo>(userData),

            EnTypeId.SkillVirtualTimelineEventPlayableInfo => ClassPoolMgr.Instance.Pull<SkillVirtualTimelineEventInfo>(userData),


            EnTypeId.VirtualSkillTimeline => ClassPoolMgr.Instance.Pull<SkillVirtualTimeline>(userData),
            EnTypeId.VirtualTimelineDefaultEvent => ClassPoolMgr.Instance.Pull<VirtualTimelineDefaultEvent>(userData),

            EnTypeId.SkillTimelinePhysicsSphereEvent => ClassPoolMgr.Instance.Pull<SkillTimelinePhysicsSphereEvent>(userData),
            EnTypeId.SkillTimelinePhysicsCylinderEvent => ClassPoolMgr.Instance.Pull<SkillTimelinePhysicsCylinderEvent>(userData),
            EnTypeId.SkillTimelineAttackCylinderEvent => ClassPoolMgr.Instance.Pull<SkillTimelineAttackCylinderEvent>(userData),
            EnTypeId.SkillTypeSingletonPlayableAdapter => ClassPoolMgr.Instance.Pull<SkillSingletonPlayableAdapter>(userData),
            EnTypeId.LayerMixerCmdPlayableAdapter => ClassPoolMgr.Instance.Pull<LayerMixerCmdPlayableAdapter>(userData),
            EnTypeId.SkillLinkPlayableAdapter => ClassPoolMgr.Instance.Pull<SkillLinkPlayableAdapter>(userData),
            EnTypeId.SkillLoopPlayableAdapter => ClassPoolMgr.Instance.Pull<SkillLoopPlayableAdapter>(userData),
            EnTypeId.SkillRandomPlayableAdapter => ClassPoolMgr.Instance.Pull<SkillRandomPlayableAdapter>(userData),
            EnTypeId.SkillTypeSelectPlayableAdapter => ClassPoolMgr.Instance.Pull<SkillTypeSelectPlayableAdapter>(userData),

            EnTypeId.SkillVirtualTimelineAnimationPlayableNode => ClassPoolMgr.Instance.Pull<SkillVirtualTimelineAnimationPlayableNode>(userData),

            EnTypeId.SkillItemInfo => ClassPoolMgr.Instance.Pull<SkillStageInfo>(userData),
            EnTypeId.SkillTypeLoopData => ClassPoolMgr.Instance.Pull<SkillTypeLoopData>(userData),
            EnTypeId.SkillTypeSelectData => ClassPoolMgr.Instance.Pull<SkillTypeSelectData>(userData),
            EnTypeId.SkillTypeSelectItemInfo => ClassPoolMgr.Instance.Pull<SkillTypeSelectItemInfo>(userData),
            EnTypeId.PhysicsResolveBox => ClassPoolMgr.Instance.Pull<PhysicsResolveBox>(userData),
            EnTypeId.PhysicsResolveSphere => ClassPoolMgr.Instance.Pull<PhysicsResolveSphere>(userData),
            EnTypeId.CompareLessInfo => ClassPoolMgr.Instance.Pull<CompareLessInfo>(userData),
            EnTypeId.CompareGreaterInfo => ClassPoolMgr.Instance.Pull<CompareGreaterInfo>(userData),
            EnTypeId.CompareEqualInfo => ClassPoolMgr.Instance.Pull<CompareEqualInfo>(userData),
            EnTypeId.PhysicsResolveCylinder => ClassPoolMgr.Instance.Pull<PhysicsResolveCylinder>(userData),
            //EnTypeId.EntityNoMovementBuffData => ClassPoolMgr.Instance.Pull<EntityNoMovementBuffData>(userData),

            EnTypeId.SkillBehaviourScheduleAction => ClassPoolMgr.Instance.Pull<SkillBehaviourScheduleAction>(userData),
            EnTypeId.SkillBuffScheduleAction => ClassPoolMgr.Instance.Pull<SkillBuffScheduleAction>(userData),
            EnTypeId.SkillEffectScheduleAction => ClassPoolMgr.Instance.Pull<SkillEffectScheduleAction>(userData),
            EnTypeId.SkillPhysicsScheduleAction => ClassPoolMgr.Instance.Pull<SkillPhysicsScheduleAction>(userData),
            EnTypeId.SkillFlyItemScheduleAction => ClassPoolMgr.Instance.Pull<SkillFlyItemScheduleAction>(userData),
            EnTypeId.SkillEventScheduleAction => ClassPoolMgr.Instance.Pull<SkillEventScheduleAction>(userData),
            EnTypeId.SkillAudioScheduleAction => ClassPoolMgr.Instance.Pull<SkillAudioScheduleAction>(userData),


            EnTypeId.AiDefaultDecision => ClassPoolMgr.Instance.Pull<AiDefaultDecision>(userData),
            EnTypeId.AiTransformDecisionSubGenerate => ClassPoolMgr.Instance.Pull<AiTransformDecisionSubGenerator>(userData),
            EnTypeId.AiDirectionDecisionSubGenerator => ClassPoolMgr.Instance.Pull<AiLookAtDecisionSubGenerator>(userData),


            EnTypeId.AiComAtkDecisionGenerator => ClassPoolMgr.Instance.Pull<AiCmdDecisionGenerator>(userData),
            EnTypeId.AiIdleDecisionGenerator => ClassPoolMgr.Instance.Pull<AiIdleDecisionGenerator>(userData),

            EnTypeId.AiDistanceCondition => ClassPoolMgr.Instance.Pull<AiDistanceCondition>(userData),
            EnTypeId.AiRotationCondition => ClassPoolMgr.Instance.Pull<AiRotationCondition>(userData),

            EnTypeId.AiPerceptionMatchInfo => ClassPoolMgr.Instance.Pull<AiPerceptionMatchInfo>(userData),
            EnTypeId.AiDecisionGeneratorInfo => ClassPoolMgr.Instance.Pull<AiDecisionGeneratorInfo>(userData),

            EnTypeId.AiRandomEntityPerception => ClassPoolMgr.Instance.Pull<AiRandomEntityPerception>(userData),
            EnTypeId.AiPerceptionInfos => ClassPoolMgr.Instance.Pull<AiPerceptionInfos>(userData),
            EnTypeId.AiDecisionInfos => ClassPoolMgr.Instance.Pull<AiDecisionInfo>(userData),

            EnTypeId.AiPerceptionLayerTest => ClassPoolMgr.Instance.Pull<AiPerceptionLayerTest>(userData),
            EnTypeId.AiPerceptionDistanceTest => ClassPoolMgr.Instance.Pull<AiPerceptionDistanceTest>(userData),

            _ => null,
        };
    }
    public static T CreateInstance<T>(in int typeId, in IClassPool userData)
        where T : IClassPool
    {
        var result = CreateInstance(in typeId, in userData);
        return (T)result;
    }
    public static void DestroyInstance(in IClassPool instance)
    {
        ClassPoolMgr.Instance.Push(instance);
    }
}
