public class LocationInfo : AgentInfo
{
    public LocationInfo(Agent agent) : base(agent) { }

    public override string ToString()
    {
        return AgentString() + " was in " + Location.name + " " + TimeInterval;
    }
}
