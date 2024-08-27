using System;
using System.Collections.Generic;
using UnityEngine;

public class AgentInfo
{
    public struct AgentLocationInfo
    {
        public Agent Agent { get; private set; }
        public Location Location { get; private set; }
        public double FromTimestamp { get; private set; }
        public double ToTimestamp { get; set; }

        public AgentLocationInfo(Agent agent)
        {
            Agent = agent;
            Location = agent.CurrentLocation;
            FromTimestamp = Time.timeAsDouble;
            ToTimestamp = FromTimestamp;
        }

        public override bool Equals(object obj)
        {
            return obj is AgentLocationInfo info &&
                   EqualityComparer<Agent>.Default.Equals(Agent, info.Agent) &&
                   EqualityComparer<Location>.Default.Equals(Location, info.Location);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Agent, Location);
        }

        public override string ToString()
        {
            return Agent.name + (Agent.IsDead ? "'s dead body" : "") +
                " in " + Location.name +
                ((FromTimestamp == ToTimestamp) ? " at " : (" from " + FromTimestamp.ToString("N1") + " s to ")) +
                ToTimestamp.ToString("N1") + " s";
        }
    }

    public struct AgentSeenInfo
    {
        public Agent AgentSeeing { get; private set; }
        public AgentLocationInfo AgentSeenLocationInfo { get; private set; }

        public AgentSeenInfo(Agent agentSeeing, Agent agentSeen)
        {
            AgentSeeing = agentSeeing;
            AgentSeenLocationInfo = new AgentLocationInfo(agentSeen);
        }

        public override string ToString()
        {
            return AgentSeeing.name + " saw " + AgentSeenLocationInfo;
        }
    }

    private List<AgentLocationInfo> agentsLocationsInfo = new List<AgentLocationInfo>();
    private List<AgentSeenInfo> agentsSeenInfo = new List<AgentSeenInfo>();
    private AgentSeenInfo deadBodyInfo;

    public void AddAgentLocationInfo(AgentLocationInfo agentLocationInfo)
    {
        agentsLocationsInfo.Add(agentLocationInfo);
        Debug.Log(agentLocationInfo);
    }

    public void AddAgentLocationInfo(Agent agent)
    {
        AgentLocationInfo agentLocationInfo = new AgentLocationInfo(agent);
        AddAgentLocationInfo(agentLocationInfo);
    }

    public void AddAgentSeenInfo(AgentSeenInfo agentSeenInfo)
    {
        agentsSeenInfo.Add(agentSeenInfo);
        Debug.Log(agentSeenInfo);
    }

    public void AddAgentSeenInfo(Agent agentSeeing, Agent agentSeen)
    {
        AgentSeenInfo agentSeenInfo = new AgentSeenInfo(agentSeeing, agentSeen);
        AddAgentSeenInfo(agentSeenInfo);
    }

    public void SetDeadBodyInfo(AgentSeenInfo deadBodyInfo)
    {
        this.deadBodyInfo = deadBodyInfo;
        Debug.Log(deadBodyInfo);
    }

    public void SetDeadBodyInfo(Agent agentSeeing, Agent deadBody)
    {
        AgentSeenInfo deadBodyInfo = new AgentSeenInfo(agentSeeing, deadBody);
        SetDeadBodyInfo(deadBodyInfo);
    }
}
