
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
    _______________________________GameCharacter = 5000,
    GameBattleCharacter1EntityData,
    _______________________________EntityBuff = 5100,
    EntityVulnerableBuffData,
    EntityBurnBuffData,
    EntityCrystalBuffData,
    EntityShatteredArmorBuffData,
    EntityFreezeBuffData,
    EntityElectricBuffData,
    EntityElectricBuffAttributeReduceData,
    _______________________________SkillBall = 5300,
    GameBattleSkillBall_1Data = 5301,
    GameBattleSkillBall_101Data = 5321,
    GameBattleSkillBall_1001Data = 5351,
    GameBattleSkillBall_1002Data,
    GameBattleSkillBall_1003Data,
    GameBattleSkillBall_1004Data,
    GameBattleSkillBall_1005Data,
    GameBattleSkillBall_1006Data,
    GameBattleSkillBall_1007Data,
    GameBattleSkillBall_1008Data,
    GameBattleSkillBall_1009Data,
    GameBattleSkillBall_1010Data,
    GameBattleSkillBall_1011Data,
    GameBattleSkillBall_1012Data,
    GameBattleSkillBall_2001Data = 5401,

    GameBattleSkillBallPart_1001Data = 5901,
    GameBattleSkillBallPart_1002Data,
    GameBattleSkillBallPart_1003Data,
    GameBattleSkillBallPart_1004Data,
    GameBattleSkillBallPart_1005Data,
    GameBattleSkillBallPart_1006Data,
    GameBattleSkillBallPart_1007Data,
    GameBattleSkillBallPart_1008Data,
    GameBattleSkillBallPart_1009Data,
    GameBattleSkillBallPart_1010Data,
    GameBattleSkillBallPart_1011Data,
    GameBattleSkillBallPart_1012Data,
    GameBattleSkillBallPartInfo = 6001,


    _______________________________Wave = 6100,
    GameBattleWaveMonsterInfo,
    _______________________________SkillEvent = 6200,
    GameSkillMonsterAttack1EventData,
    GameSkillMonsterDieEventData,
    GameSkillBossSummonEventData,
    GameSkillBossDebutEventData
}
public static class TypeDefine
{
    public static IClassPool CreateInstance(in int typeId, in IClassPoolDestroy userData)
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
            EnTypeId.EntityNoMovementBuffData => ClassPoolMgr.Instance.Pull<EntityNoMovementBuffData>(userData),

            EnTypeId.SkillBehaviourScheduleAction => ClassPoolMgr.Instance.Pull<SkillBehaviourScheduleAction>(userData),
            EnTypeId.SkillBuffScheduleAction => ClassPoolMgr.Instance.Pull<SkillBuffScheduleAction>(userData),
            EnTypeId.SkillEffectScheduleAction => ClassPoolMgr.Instance.Pull<SkillEffectScheduleAction>(userData),
            EnTypeId.SkillPhysicsScheduleAction => ClassPoolMgr.Instance.Pull<SkillPhysicsScheduleAction>(userData),
            EnTypeId.SkillFlyItemScheduleAction => ClassPoolMgr.Instance.Pull<SkillFlyItemScheduleAction>(userData),
            EnTypeId.SkillEventScheduleAction => ClassPoolMgr.Instance.Pull<SkillEventScheduleAction>(userData),


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

            // game
            EnTypeId.GameBattleMonster1000101EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000101EntityData>(userData),
            EnTypeId.GameBattleMonster1000102EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000102EntityData>(userData),
            EnTypeId.GameBattleMonster1000103EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000103EntityData>(userData),
            EnTypeId.GameBattleMonster1000104EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000104EntityData>(userData),
            EnTypeId.GameBattleMonster1000201EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000201EntityData>(userData),
            EnTypeId.GameBattleMonster1000301EntityData => ClassPoolMgr.Instance.Pull<GameBattleMonster1000301EntityData>(userData),
            EnTypeId.GameBattleMapBoss1001101EntityData => ClassPoolMgr.Instance.Pull<GameBattleMapBoss1001101EntityData>(userData),
            EnTypeId.GameSkillBossDebutEventData => ClassPoolMgr.Instance.Pull<GameSkillBossDebutEventData>(userData),
            EnTypeId.GameSkillBossSummonEventData => ClassPoolMgr.Instance.Pull<GameSkillBossSummonEventData>(userData),

            EnTypeId.GameBattleCharacter1EntityData => ClassPoolMgr.Instance.Pull<GameBattleCharacter1EntityData>(userData),

            EnTypeId.EntityVulnerableBuffData => ClassPoolMgr.Instance.Pull<EntityVulnerableBuffData>(userData),
            EnTypeId.EntityBurnBuffData => ClassPoolMgr.Instance.Pull<EntityBurnBuffData>(userData),
            EnTypeId.EntityCrystalBuffData => ClassPoolMgr.Instance.Pull<EntityCrystalBuffData>(userData),
            EnTypeId.EntityShatteredArmorBuffData => ClassPoolMgr.Instance.Pull<EntityShatteredArmorBuffData>(userData),
            EnTypeId.EntityFreezeBuffData => ClassPoolMgr.Instance.Pull<EntityFreezeBuffData>(userData),
            EnTypeId.EntityElectricBuffData => ClassPoolMgr.Instance.Pull<EntityElectricBuffData>(userData),
            EnTypeId.EntityElectricBuffAttributeReduceData => ClassPoolMgr.Instance.Pull<EntityElectricBuffAttributeReduceData>(userData),

            EnTypeId.GameBattleSkillBall_1Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1Data>(userData),
            EnTypeId.GameBattleSkillBall_101Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_101Data>(userData),
            EnTypeId.GameBattleSkillBall_1001Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1001Data>(userData),
            EnTypeId.GameBattleSkillBall_1002Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1002Data>(userData),
            EnTypeId.GameBattleSkillBall_1003Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1003Data>(userData),
            EnTypeId.GameBattleSkillBall_1004Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1004Data>(userData),
            EnTypeId.GameBattleSkillBall_1005Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1005Data>(userData),
            EnTypeId.GameBattleSkillBall_1006Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1006Data>(userData),
            EnTypeId.GameBattleSkillBall_1007Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1007Data>(userData),
            EnTypeId.GameBattleSkillBall_1008Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1008Data>(userData),
            EnTypeId.GameBattleSkillBall_1009Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1009Data>(userData),
            EnTypeId.GameBattleSkillBall_1010Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1010Data>(userData),
            EnTypeId.GameBattleSkillBall_1011Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1011Data>(userData),
            EnTypeId.GameBattleSkillBall_1012Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBall_1012Data>(userData),

            EnTypeId.GameBattleSkillBallPart_1001Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1001Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1002Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1002Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1003Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1003Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1004Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1004Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1005Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1005Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1006Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1006Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1007Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1007Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1008Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1008Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1009Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1009Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1010Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1010Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1011Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1011Data>(userData),
            EnTypeId.GameBattleSkillBallPart_1012Data => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPart_1012Data>(userData),

            EnTypeId.GameBattleSkillBallPartInfo => ClassPoolMgr.Instance.Pull<GameBattleSkillBallPartInfo>(userData),


            EnTypeId.GameBattleWaveMonsterInfo => ClassPoolMgr.Instance.Pull<GameBattleWaveMonsterInfo>(userData),


            EnTypeId.GameSkillMonsterAttack1EventData => ClassPoolMgr.Instance.Pull<GameSkillMonsterAttack1EventData>(userData),
            EnTypeId.GameSkillMonsterDieEventData => ClassPoolMgr.Instance.Pull<GameSkillMonsterDieEventData>(userData),

            _ => null,
        };
    }
    public static T CreateInstance<T>(in int typeId, in IClassPoolDestroy userData)
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
