public abstract class AgentInfo : Info
{
    public AgentInfo(Agent agent) : base(agent, agent.CurrentLocation, new TimeInterval()) { }
}
