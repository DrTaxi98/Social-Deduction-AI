public class SeeingInfo : ViewInfo
{
    public SeeingInfo(Agent seeingAgent, Agent seenAgent) : base(seeingAgent, seenAgent) { }

    public bool UpdateSeeingInfo(SeeingInfo other)
    {
        if (Equals(other))
            return TimeInterval.UpdateInterval(other.TimeInterval);

        return false;
    }
}
