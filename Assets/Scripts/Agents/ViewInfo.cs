using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewInfo
{
    public Agent SeeingAgent { get; }
    public Agent SeenAgent { get; }
    public Location Location { get; }
    public double FromTimestamp { get; }
    public double ToTimestamp { get; private set; }

    public ViewInfo(Agent self) : this(self, self) { }

    public ViewInfo(Agent seeingAgent, Agent seenAgent)
    {
        SeeingAgent = seeingAgent;
        SeenAgent = seenAgent;
        Location = seenAgent.CurrentLocation;
        FromTimestamp = Time.timeAsDouble;
        ToTimestamp = FromTimestamp;
    }

    public bool UpdateToTimestamp(ViewInfo other)
    {
        bool update = Equals(other) && Utils.AreTimestampsClose(ToTimestamp, other.ToTimestamp);
        if (update)
            ToTimestamp = other.ToTimestamp;

        return update;
    }

    public override bool Equals(object obj)
    {
        return obj is ViewInfo other &&
               EqualityComparer<Agent>.Default.Equals(SeeingAgent, other.SeeingAgent) &&
               EqualityComparer<Agent>.Default.Equals(SeenAgent, other.SeenAgent) &&
               EqualityComparer<Location>.Default.Equals(Location, other.Location);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SeeingAgent, SeenAgent, Location);
    }

    public override string ToString()
    {
        return (SeeingAgent == SeenAgent) ? "" : (SeeingAgent.name + " saw ") +
            SeenAgent.name + (SeenAgent.IsDead ? "'s dead body" : "") +
            " in " + Location.name +
            ((FromTimestamp == ToTimestamp) ? " at " : (" from " + FromTimestamp.ToString("N1") + " s to ")) +
            ToTimestamp.ToString("N1") + " s";
    }
}
