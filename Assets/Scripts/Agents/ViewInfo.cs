using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewInfo
{
    public Agent Agent { get; private set; }
    public Location Location { get; private set; }
    public double FromTimestamp { get; private set; }
    public double ToTimestamp { get; set; }

    public ViewInfo(Agent agent)
    {
        Agent = agent;
        Location = agent.CurrentLocation;
        FromTimestamp = Time.timeAsDouble;
        ToTimestamp = FromTimestamp;
    }

    public override bool Equals(object obj)
    {
        return obj is ViewInfo info &&
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
