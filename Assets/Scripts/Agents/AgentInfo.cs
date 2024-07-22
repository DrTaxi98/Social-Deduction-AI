using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo
{
    private struct AgentLocation
    {
        public Agent Agent { get; private set; }
        public Location Location { get; private set; }
        public double Timestamp { get; private set; }

        public AgentLocation(Agent agent, Location location, double timestamp)
        {
            Agent = agent;
            Location = location;
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            return Agent.name + " in " + Location.name + " at " + Timestamp;
        }
    }

    private List<AgentLocation> agentsLocations = new List<AgentLocation>();
    private List<AgentLocation> agentsSeen = new List<AgentLocation>();
    private AgentLocation dead;

    public void AddAgentLocation(Agent agent, Location location)
    {
        AgentLocation agentLocation = new AgentLocation(agent, location, Time.timeAsDouble);
        agentsLocations.Add(agentLocation);
    }

    public void AddAgentSeen(Agent agent, Location location)
    {
        AgentLocation agentSeen = new AgentLocation(agent, location, Time.timeAsDouble);
        agentsSeen.Add(agentSeen);
    }

    public void SetDead(Agent agent, Location location)
    {
        dead = new AgentLocation(agent, location, Time.timeAsDouble);
    }
}
