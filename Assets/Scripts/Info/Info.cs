using System;
using System.Collections.Generic;

public abstract class Info
{
    protected const string SUBJECT_REPLACEMENT = "I";
    protected const string OBJECT_REPLACEMENT = "me";

    public Agent Agent { get; }
    public Location Location { get; }
    public TimeInterval TimeInterval { get; }

    protected virtual string AgentString => Agent.name;
    protected virtual string Replacement => OBJECT_REPLACEMENT;

    public Info(Agent agent, Location location, TimeInterval timeInterval)
    {
        Agent = agent;
        Location = location;
        TimeInterval = timeInterval;
    }

    public virtual bool CloseTo(object obj)
    {
        return obj is Info info &&
            CloseTo(info.Agent, info.Location, info.TimeInterval);
    }

    public bool CloseTo(Agent agent, Location location, TimeInterval timeInterval)
    {
        return Agent == agent &&
            Location.CloseTo(location) &&
            TimeInterval.CloseTo(timeInterval);
    }

    public virtual string ToMessage(Agent agent)
    {
        string message = ToString();

        return (Agent == agent) ?
            Utils.ReplaceString(message, Agent.name, Replacement) :
            message;
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
        return AgentString + " in " + Location.name + " " + TimeInterval;
    }

    // Meeting Info

    public abstract class MeetingInfo : Info
    {
        protected override string Replacement => SUBJECT_REPLACEMENT;

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

        protected virtual string CloseAgentString => CloseAgent.name;

        public CloseInfo(Agent agent, Location location, TimeInterval timeInterval, Agent closeAgent) :
            base(agent, location, timeInterval)
        {
            CloseAgent = closeAgent;
        }

        public override bool CloseTo(object obj)
        {
            return obj is CloseInfo info &&
                CloseTo(info.Agent, info.Location, info.TimeInterval, info.CloseAgent);
        }

        public bool CloseTo(Agent agent, Location location, TimeInterval timeInterval, Agent closeAgent)
        {
            return CloseTo(agent, location, timeInterval) &&
                CloseAgent == closeAgent;
        }

        public override string ToMessage(Agent agent)
        {
            string message = base.ToMessage(agent);

            return (CloseAgent == agent) ?
                Utils.ReplaceString(message, CloseAgent.name, OBJECT_REPLACEMENT) :
                message;
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
            return AgentString + " was close to " + CloseAgentString +
                " in " + Location.name +
                " " + TimeInterval;
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
        protected override string CloseAgentString => base.CloseAgentString + "'s dead body";

        public CloseDeadBodyInfo(Agent agent, Location location, TimeInterval timeInterval, Agent deadBody) :
            base(agent, location, timeInterval, deadBody)
        { }
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
            return AgentString + " was distant from " + Location.name + " " + TimeInterval;
        }
    }

    public class TruthfulInfo : MeetingInfo
    {
        public TruthfulInfo(Agent agent, Location location, TimeInterval timeInterval) :
            base(agent, location, timeInterval)
        { }

        public override string ToString()
        {
            return AgentString + " was truthful about being in " + Location.name + " " + TimeInterval;
        }
    }
}
