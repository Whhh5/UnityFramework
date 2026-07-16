public class AiExecutionUserData : IClassPoolDestroy
{
    public int aiId;
    public void OnPoolDestroy()
    {
        aiId = -1;
    }
}