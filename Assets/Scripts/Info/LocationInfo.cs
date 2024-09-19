public class LocationInfo : AgentInfo
{
    protected override string Replacement => SUBJECT_REPLACEMENT;

    public LocationInfo(Agent agent) : base(agent) { }

    public override string ToString()
    {
        return AgentString + " was in " + Location.name + " " + TimeInterval;
    }
}
