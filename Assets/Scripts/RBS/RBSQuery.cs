public abstract class RBSQuery : IRBSQuery
{
    public static RBSQueryDeadBody DeadBody() =>
        new RBSQueryDeadBody();
    public static RBSQueryDeadAlive DeadAlive() =>
        new RBSQueryDeadAlive();
    public static RBSQueryLocationInfo LocationInfo() =>
        new RBSQueryLocationInfo();
    public static RBSQueryAgentInfo AgentInfo() =>
        new RBSQueryAgentInfo();
    public static RBSQueryLocationTime LocationTime() =>
        new RBSQueryLocationTime();
    public static RBSQuerySeenLocationTime SeenLocationTime() =>
        new RBSQuerySeenLocationTime();
    public static RBSQueryDistantLocationTime DistantLocationTime() =>
        new RBSQueryDistantLocationTime();
    public static RBSQueryCloseAgents CloseAgents() =>
        new RBSQueryCloseAgents();
    public static RBSQueryCloseDeadBody CloseDeadBody() =>
        new RBSQueryCloseDeadBody();
    public static RBSQueryCloseDeadAlive CloseDeadAlive() =>
        new RBSQueryCloseDeadAlive();
    public static RBSQueryDistant Distant() =>
        new RBSQueryDistant();
    public static RBSQueryTruthful Truthful() =>
        new RBSQueryTruthful();

    public abstract bool Predicate(RBSDatum datum);

    // Binding Queries

    public abstract class RBSBindingQuery : RBSQuery { }

    public class RBSQueryDeadBody : RBSBindingQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is DeadBodyInfo;
        }
    }

    public class RBSQueryDeadAlive : RBSBindingQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is SeeingInfo info &&
                info.Agent.IsDead;
        }
    }

    public class RBSQueryLocationInfo : RBSBindingQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is LocationInfo info &&
                !info.Agent.IsDead;
        }
    }

    public class RBSQueryAgentInfo : RBSBindingQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is AgentInfo info &&
                datum.value is not DeadBodyInfo &&
                !info.Agent.IsDead;
        }
    }

    // Condition Queries

    public abstract class RBSConditionQuery : RBSQuery
    {
        public RBSDatum Binding { get; set; }
    }

    public class RBSQueryLocationTime : RBSConditionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is AgentInfo info &&
                info is not DeadBodyInfo &&
                Binding.value is AgentInfo bindingInfo &&
                !info.Agent.IsDead &&
                !info.Equals(bindingInfo) &&
                info.Agent != bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }

    public class RBSQuerySeenLocationTime : RBSConditionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is SeeingInfo info &&
                Binding.value is AgentInfo bindingInfo &&
                !info.Agent.IsDead &&
                !info.Equals(bindingInfo) &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }

    public class RBSQueryDistantLocationTime : RBSConditionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is AgentInfo info &&
                info is not DeadBodyInfo &&
                Binding.value is AgentInfo bindingInfo &&
                !info.Agent.IsDead &&
                !info.Equals(bindingInfo) &&
                info.Agent == bindingInfo.Agent &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval) &&
                !info.Location.CloseTo(bindingInfo.Location);
        }
    }

    // Repetition Queries

    public abstract class RBSRepetitionQuery : RBSQuery
    {
        public RBSDatum Binding { get; set; }
    }

    public class RBSQueryCloseAgents : RBSRepetitionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is Info.CloseAgentsInfo info &&
                Binding.value is Info bindingInfo &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }

    public class RBSQueryCloseDeadBody : RBSRepetitionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is Info.CloseDeadBodyInfo info &&
                Binding.value is Info bindingInfo &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }

    public class RBSQueryCloseDeadAlive : RBSRepetitionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is Info.CloseDeadAliveInfo info &&
                Binding.value is Info bindingInfo &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }

    public class RBSQueryDistant : RBSRepetitionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is Info.DistantInfo info &&
                Binding.value is Info bindingInfo &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location);
        }
    }

    public class RBSQueryTruthful : RBSRepetitionQuery
    {
        public override bool Predicate(RBSDatum datum)
        {
            return datum.value is Info.TruthfulInfo info &&
                Binding.value is Info bindingInfo &&
                info.Agent == bindingInfo.Agent &&
                info.Location.CloseTo(bindingInfo.Location) &&
                info.TimeInterval.CloseTo(bindingInfo.TimeInterval);
        }
    }
}
