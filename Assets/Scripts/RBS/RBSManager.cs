using System.Collections.Generic;

public class RBSManager
{
    private const string DEFAULT_MESSAGE = "Nothing to say.";

    private const int CLOSE_DEAD_BODY_SUSPECT_POINTS = 3;
    private const int CLOSE_DEAD_ALIVE_SUSPECT_POINTS = 2;
    private const int DISTANT_SUSPECT_POINTS = 1;
    private const int TRUTHFUL_SUSPECT_POINTS = -1;
    private const int CLOSE_AGENTS_SUSPECT_POINTS = -2;

    private RBSAgent agent;
    private RBSDB database;
    private RBS rbs;

    private RBSDatum binding;
    private RBSDatum match;

    public RBSManager(RBSAgent agent, RBSDB database)
    {
        this.agent = agent;
        this.database = database;
        rbs = new RBS();

        DefineRules();
    }

    public void RunRBS()
    {
        if (!rbs.Run())
        {
            GameManager.Instance.meeting.DisplayMessage(DEFAULT_MESSAGE);
        }
    }

    // Rules

    private void DefineRules()
    {
        RBSRule closeDeadBodyRule = new RBSRule(CloseDeadBodyCondition, CloseDeadBodyAction);
        rbs.AddRule(closeDeadBodyRule);

        RBSRule closeDeadAliveRule = new RBSRule(CloseDeadAliveCondition, CloseDeadAliveAction);
        rbs.AddRule(closeDeadAliveRule);

        RBSRule distantRule = new RBSRule(DistantCondition, DistantAction);
        rbs.AddRule(distantRule);

        RBSRule truthfulRule = new RBSRule(TruthfulCondition, TruthfulAction);
        rbs.AddRule(truthfulRule);

        RBSRule closeAgentsRule = new RBSRule(CloseAgentsCondition, CloseAgentsAction);
        rbs.AddRule(closeAgentsRule);
    }

    // Conditions

    private bool CloseDeadBodyCondition()
    {
        return Match(RBSQuery.DeadBody(), RBSQuery.LocationTime(), RBSQuery.CloseDeadBody());
    }

    private bool CloseDeadAliveCondition()
    {
        return Match(RBSQuery.DeadAlive(), RBSQuery.LocationTime(), RBSQuery.CloseDeadAlive());
    }

    private bool DistantCondition()
    {
        return Match(RBSQuery.LocationInfo(), RBSQuery.DistantLocationTime(), RBSQuery.Distant());
    }

    private bool TruthfulCondition()
    {
        return Match(RBSQuery.LocationInfo(), RBSQuery.SeenLocationTime(), RBSQuery.Truthful());
    }

    private bool CloseAgentsCondition()
    {
        return Match(RBSQuery.AgentInfo(), RBSQuery.LocationTime(), RBSQuery.CloseAgents());
    }

    private bool Match(RBSQuery.RBSBindingQuery bindingQuery,
        RBSQuery.RBSConditionQuery conditionQuery,
        RBSQuery.RBSRepetitionQuery repetitionQuery)
    {
        List<RBSDatum> bindings = database.Match(bindingQuery);
        foreach (RBSDatum binding in bindings)
        {
            conditionQuery.Binding = binding;
            List<RBSDatum> matches = database.Match(conditionQuery);
            foreach (RBSDatum match in matches)
            {
                if (match.value is not Info info || info.Agent != agent.Agent)
                {
                    repetitionQuery.Binding = match;
                    List<RBSDatum> repetitions = database.Match(repetitionQuery);
                    if (repetitions.Count == 0)
                    {
                        this.binding = binding;
                        this.match = match;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Actions

    private void CloseDeadBodyAction()
    {
        Info bindingInfo = binding.value as Info;
        Info matchInfo = match.value as Info;
        Info.CloseDeadBodyInfo info =
            new Info.CloseDeadBodyInfo(matchInfo.Agent, matchInfo.Location, matchInfo.TimeInterval, bindingInfo.Agent);

        List<Agent> suspects = new List<Agent>() { matchInfo.Agent };
        Action(info, suspects, CLOSE_DEAD_BODY_SUSPECT_POINTS);
    }

    private void CloseDeadAliveAction()
    {
        Info bindingInfo = binding.value as Info;
        Info matchInfo = match.value as Info;
        Info.CloseDeadAliveInfo info =
            new Info.CloseDeadAliveInfo(matchInfo.Agent, matchInfo.Location, matchInfo.TimeInterval, bindingInfo.Agent);

        List<Agent> suspects = new List<Agent>() { matchInfo.Agent };
        Action(info, suspects, CLOSE_DEAD_ALIVE_SUSPECT_POINTS);
    }

    private void DistantAction()
    {
        Info bindingInfo = binding.value as Info;
        Info matchInfo = match.value as Info;
        Info.DistantInfo info =
            new Info.DistantInfo(matchInfo.Agent, bindingInfo.Location, matchInfo.TimeInterval);

        List<Agent> suspects = new List<Agent>() { matchInfo.Agent };
        Action(info, suspects, DISTANT_SUSPECT_POINTS);
    }

    private void TruthfulAction()
    {
        Info bindingInfo = binding.value as Info;
        Info matchInfo = match.value as Info;
        Info.TruthfulInfo info =
            new Info.TruthfulInfo(matchInfo.Agent, matchInfo.Location, matchInfo.TimeInterval);

        List<Agent> suspects = new List<Agent>() { matchInfo.Agent };
        Action(info, suspects, TRUTHFUL_SUSPECT_POINTS);
    }

    private void CloseAgentsAction()
    {
        Info bindingInfo = binding.value as Info;
        Info matchInfo = match.value as Info;
        Info.CloseAgentsInfo info =
            new Info.CloseAgentsInfo(matchInfo.Agent, matchInfo.Location, matchInfo.TimeInterval, bindingInfo.Agent);

        List<Agent> suspects = new List<Agent>() { matchInfo.Agent, bindingInfo.Agent };
        Action(info, suspects, CLOSE_AGENTS_SUSPECT_POINTS);
    }

    private void Action(Info.MeetingInfo info, List<Agent> suspects, int suspectPoints)
    {
        RBSDatum datum = new RBSDatum(info);
        agent.AddDatum(datum);

        foreach (Agent suspect in suspects)
        {
            suspect.MeetingAgent.AddSuspectLevel(suspectPoints);
        }

        List<RBSDatum> startMessageData = new List<RBSDatum>() { binding, match };
        List<RBSDatum> endMessageData = new List<RBSDatum>() { datum };
        GameManager.Instance.meeting.DisplayMessage(startMessageData, endMessageData);

        List<RBSDatum> dataToShare = new List<RBSDatum>() { match, datum };
        GameManager.Instance.meeting.ShareData(dataToShare);
    }
}
