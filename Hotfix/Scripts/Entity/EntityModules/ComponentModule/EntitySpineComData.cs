using System.Threading;
using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;



public enum EnSpineAnimId
{
    None,
    appear,
    attack,
    die,
    hit_1,
    hit_2,
    hit_3,
    stand,
}
public enum EnGameSpineCompleteStatus
{
    None,
    Normal,
    Force,
}
public interface IEntitySpineCom : IEntity3DCom
{
    public SkeletonAnimation GetSkeletonAnimation();
}
public class EntitySpineComData : Entity3DComDataGO<IEntitySpineCom>
{
    public override EnSceneEntityComType GetComType()
    {
        return EnSceneEntityComType.Spine;
    }


    private EnSpineAnimId _CurAnimId;
    private EnSceneEntityStatus _LinkStatus;
    private CancellationTokenSource _Token;

    public override void OnDisable()
    {
        if (_CurAnimId != EnSpineAnimId.None)
        {
            var isLoop = GetAnimIsLoop(_CurAnimId);
            if (isLoop)
            {
                //GameMonsterUtility.ExitStatus(_EntityId, _LinkStatus, EnGameSpineCompleteStatus.Force);
            }
        }
        _Token?.Cancel();
        base.OnDisable();
        _Token = null;
        _CurAnimId = EnSpineAnimId.None;
        _LinkStatus = EnSceneEntityStatus.None;
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnCreateGO()
    {
        base.OnCreateGO();
        PlayAnim(_CurAnimId, _LinkStatus);
    }
    public override void OnDestroyGO()
    {
        var com = _GoCom.GetSkeletonAnimation();
        com.ClearState();
        base.OnDestroyGO();
    }

    private float GetAnimTime(EnSpineAnimId animId)
    {
        return animId switch
        {
            EnSpineAnimId.appear => 1,
            EnSpineAnimId.attack => 0.133f,
            EnSpineAnimId.die => 0.833f,
            EnSpineAnimId.hit_1 => 0.133f,
            EnSpineAnimId.hit_2 => 0.133f,
            EnSpineAnimId.hit_3 => 0.133f,
            EnSpineAnimId.stand => 1.333f,
            _ => 0f,
        };
    }
    private bool GetAnimIsLoop(EnSpineAnimId animId)
    {
        return animId switch
        {
            EnSpineAnimId.appear => false,
            EnSpineAnimId.attack => true,
            EnSpineAnimId.die => false,
            EnSpineAnimId.hit_1 => false,
            EnSpineAnimId.hit_2 => false,
            EnSpineAnimId.hit_3 => false,
            EnSpineAnimId.stand => true,
            _ => false,
        };
    }
    public async void PlayAnim(EnSpineAnimId animId, EnSceneEntityStatus status)
    {
        var isLoop = GetAnimIsLoop(animId);
        if (_CurAnimId != EnSpineAnimId.None)
        {
            if (isLoop)
            {
                //GameMonsterUtility.ExitStatus(_EntityId, _LinkStatus, EnGameSpineCompleteStatus.Force);
            }
            if (_Token != null)
            {
                _Token.Cancel();
                _Token = null;
            }
        }
        _CurAnimId = animId;
        _LinkStatus = status;


        if (_GoCom != null)
        {
            var com = _GoCom.GetSkeletonAnimation();
            if (animId == EnSpineAnimId.None)
            {
                com.ClearState();
            }
            else
            {
                com.AnimationState.SetAnimation(0, $"{animId}", isLoop);
            }
        }

        if (animId != EnSpineAnimId.None && !isLoop)
        {
            var time = GetAnimTime(animId);
            _Token = new();
            var isCancel = await UniTask.WaitForSeconds(time, cancellationToken: _Token.Token, cancelImmediately: true)
                .SuppressCancellationThrow();
            _Token = null;
            if (isCancel)
            {
                //GameMonsterUtility.ExitStatus(_EntityId, _LinkStatus, EnGameSpineCompleteStatus.Force);
            }
            else
            {
                //GameMonsterUtility.ExitStatus(_EntityId, _LinkStatus, EnGameSpineCompleteStatus.Normal);
            }

        }
    }

}
