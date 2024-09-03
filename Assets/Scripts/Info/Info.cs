using System;
using System.Collections.Generic;

public abstract class Info
{
    public Agent Agent { get; }
    public Location Location { get; }
    public TimeInterval TimeInterval { get; }

    public Info(Agent agent, Location location, TimeInterval timeInterval)
    {
        Agent = agent;
        Location = location;
        TimeInterval = timeInterval;
    }

    protected virtual string AgentString()
    {
        return Agent.name;
    }

    public override bool Equals(object obj)
    {
        return obj is Info info &&
               EqualityComparer<Agent>.Default.Equals(Agent, info.Agent) &&
               EqualityComparer<Location>.Default.Equals(Location, info.Location);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Agent, Location);
    }

    public override string ToString()
    {
        return AgentString() + " in " + Location.name + " " + TimeInterval;
    }

    // Meeting Info

    public abstract class MeetingInfo : Info
    {
        public MeetingInfo(Agent agent, Location location, TimeInterval timeInterval) :
            base(agent, location, timeInterval)
        { }

        public override bool Equals(object obj)
        {
            return obj is MeetingInfo info &&
                   base.Equals(obj) &&
                   EqualityComparer<TimeInterval>.Default.Equals(TimeInterval, info.TimeInterval);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), TimeInterval);
        }
    }

    public abstract class CloseInfo : MeetingInfo
	{
        public Agent CloseAgent { get; }

        public CloseInfo(Agent agent, Location location, TimeInterval timeInterval, Agent closeAgent) :
            base(agent, location, timeInterval)
        {
            CloseAgent = closeAgent;
        }

        public override bool Equals(object obj)
        {
            return obj is CloseInfo info &&
                   base.Equals(obj) &&
                   EqualityComparer<Agent>.Default.Equals(CloseAgent, info.CloseAgent);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), CloseAgent);
        }

        public override string ToString()
        {
            return AgentString() + " was close to " + CloseAgent.name;
        }
    }

    public class CloseAgentsInfo : CloseInfo
    {
        public CloseAgentsInfo(Agent agent, Location location, TimeInterval timeInterval, Agent closeAgent) :
            base(agent, location, timeInterval, closeAgent)
        { }
    }

    public class CloseDeadBodyInfo : CloseInfo
    {
        public CloseDeadBodyInfo(Agent agent, Location location, TimeInterval timeInterval, Agent deadBody) :
            base(agent, location, timeInterval, deadBody)
        { }

        public override string ToString()
        {
            return base.ToString() + "'s dead body";
        }
    }

    public class CloseDeadAliveInfo : CloseInfo
    {
        public CloseDeadAliveInfo(Agent agent, Location location, TimeInterval timeInterval, Agent deadAlive) :
            base(agent, location, timeInterval, deadAlive)
        { }

        public override string ToString()
        {
            return base.ToString() + " when it was alive";
        }
    }

    public class DistantInfo : MeetingInfo
	{
        public DistantInfo(Agent agent, Location location, TimeInterval timeInterval) :
            base(agent, location, timeInterval)
        { }

        public override string ToString()
        {
            return AgentString() + " was distant from " + Location.name + " " + TimeInterval;
        }
    }

    public class TruthfulInfo : MeetingInfo
    {
        public TruthfulInfo(Agent agent, Location location, TimeInterval timeInterval) :
            base(agent, location, timeInterval)
        { }

        public override string ToString()
        {
            return AgentString() + " is truthful about being in " + Location.name + " " + TimeInterval;
        }
    }
}
