

public static class AiUtility
{
    public static IAiExecution CreateAiExecution(in EnAiExecutionType exeType, in IClassPoolDestroy userData)
    {
        return exeType switch
        {
            EnAiExecutionType.Cmd => ClassPoolMgr.Instance.Pull<AiCmdExecution>(userData),
            EnAiExecutionType.Move => ClassPoolMgr.Instance.Pull<AiMoveExecution>(userData),
            EnAiExecutionType.LookAt => ClassPoolMgr.Instance.Pull<AiLookAtExecution>(userData),
            _ => null,
        };
    }

    public static void DestroyAiExecution(in IAiExecution aiExecution)
    {
        ClassPoolMgr.Instance.Push(aiExecution);
    }

    public static void ExecuteAiDecision(in EnAiExecutionType exeType, in IClassPoolDestroy userData)
    {
        var execution = CreateAiExecution(in exeType, userData);
        execution.ExecuteAiNode();
        DestroyAiExecution(execution);
    }

    public static IAiPerception CreatePerception(in EnAiPerceptionId perceptionId, IClassPoolDestroy userData)
    {
        IAiPerception result = perceptionId switch
        {
            EnAiPerceptionId.RandomEntity => ClassPoolMgr.Instance.Pull<AiRandomEntityPerception>(userData),
            _ => null,
        };
        return result;
    }
    public static void DestroyPerception(IAiPerception perception)
    {
        ClassPoolMgr.Instance.Push(perception);
    }

}