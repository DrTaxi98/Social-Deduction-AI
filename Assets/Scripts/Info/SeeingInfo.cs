public class SeeingInfo : ViewInfo
{
    public SeeingInfo(Agent seeingAgent, Agent seenAgent) : base(seeingAgent, seenAgent) { }

    public bool UpdateSeeingInfo(SeeingInfo otherInfo)
    {
        if (Equals(otherInfo))
            return TimeInterval.UpdateInterval(otherInfo.TimeInterval);

        return false;
    }
}
