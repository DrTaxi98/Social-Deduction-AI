using System.Collections.Generic;

public class AgentInfo
{
    private struct AgentLocation
    {
        public Agent Agent { get; private set; }
        public Location Location { get; private set; }

        public AgentLocation(Agent agent, Location location)
        {
            Agent = agent;
            Location = location;
        }
    }

    private List<AgentLocation> agentsLocations = new List<AgentLocation>();
    private List<AgentLocation> agentsSeen = new List<AgentLocation>();
    private AgentLocation dead;

    public void AddAgentLocation(Agent agent, Location location)
    {
        AgentLocation agentLocation = new AgentLocation(agent, location);
        agentsLocations.Add(agentLocation);
    }

    public void AddAgentSeen(Agent agent, Location location)
    {
        AgentLocation agentSeen = new AgentLocation(agent, location);
        agentsSeen.Add(agentSeen);
    }

    public void SetDead(Agent agent, Location location)
    {
        dead = new AgentLocation(agent, location);
    }
}
