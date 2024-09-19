using System;
using System.Collections.Generic;

public abstract class ViewInfo : AgentInfo
{
    public Agent SeeingAgent { get; }

    public ViewInfo(Agent seeingAgent, Agent seenAgent) : base(seenAgent)
    {
        SeeingAgent = seeingAgent;
    }

    public override bool CloseTo(object obj)
    {
        return obj is ViewInfo info &&
            CloseTo(info.Agent, info.Location, info.TimeInterval, info.SeeingAgent);
    }

    public bool CloseTo(Agent agent, Location location, TimeInterval timeInterval, Agent seeingAgent)
    {
        return CloseTo(agent, location, timeInterval) &&
            SeeingAgent == agent;
    }

    public override string ToMessage(Agent agent)
    {
        string message = base.ToMessage(agent);

        return (SeeingAgent == agent) ?
            Utils.ReplaceString(message, SeeingAgent.name, SUBJECT_REPLACEMENT) :
            message;
    }

    public override bool Equals(object obj)
    {
        return obj is ViewInfo info &&
               base.Equals(obj) &&
               EqualityComparer<Agent>.Default.Equals(SeeingAgent, info.SeeingAgent);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), SeeingAgent);
    }

    public override string ToString()
    {
        return SeeingAgent.name + " saw " + base.ToString();
    }
}
