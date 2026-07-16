using System;


public enum EnMonsterStatus
{
    None = 0,
    Idle,

    Move,
    Hit,
    Atk,

    Skill1,
    Skill2,
    Skill3,

    Fly,
    ForceMove,
    Freeze,

    Die,
}
[Flags]
public enum EnSceneEntityStatus
{
    None = 0,
    Idle = 1 << EnMonsterStatus.Idle, // 正常
    Move = 1 << EnMonsterStatus.Move,
    Hit = 1 << EnMonsterStatus.Hit,
    Atk = 1 << EnMonsterStatus.Atk,
    Skill1 = 1 << EnMonsterStatus.Skill1,
    Skill2 = 1 << EnMonsterStatus.Skill2,
    Skill3 = 1 << EnMonsterStatus.Skill3,

    Fly = 1 << EnMonsterStatus.Fly, // 在天上
    ForceMove = 1 << EnMonsterStatus.ForceMove, // 停止
    Freeze = 1 << EnMonsterStatus.Freeze, // 冰冻

    Die = 1 << EnMonsterStatus.Die, // 死亡

}

