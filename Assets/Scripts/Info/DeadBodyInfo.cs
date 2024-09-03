public class DeadBodyInfo : ViewInfo
{
    public DeadBodyInfo(Agent seeingAgent, Agent deadBody) : base(seeingAgent, deadBody) { }

    protected override string AgentString()
    {
        return base.AgentString() + "'s dead body";
    }
}
