public class DeadBodyInfo : ViewInfo
{
    public DeadBodyInfo(Agent seeingAgent, Agent deadBody) : base(seeingAgent, deadBody) { }

    protected override string AgentString => base.AgentString + "'s dead body";
}
