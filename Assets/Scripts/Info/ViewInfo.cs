using System;
using System.Collections.Generic;

public abstract class ViewInfo : AgentInfo
{
    public Agent SeeingAgent { get; }

    public ViewInfo(Agent seeingAgent, Agent seenAgent) : base(seenAgent)
    {
        SeeingAgent = seeingAgent;
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
