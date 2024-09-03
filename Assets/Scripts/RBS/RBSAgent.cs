using System.Collections.Generic;

public class RBSAgent
{
    private RBSDB database;
    private RBSManager manager;

    private RBSDatum deadBodyInfo;
    private RBSDatum selfInfo;

    public Agent Agent { get; }
    public int SuspectLevel { get; private set; }

    public RBSAgent(Agent agent)
    {
        Agent = agent;
        database = new RBSDB();
        manager = new RBSManager(this, database);

        SuspectLevel = 0;
    }

    public void AddInfo(List<SeeingInfo> seeingInfoList, DeadBodyInfo deadBodyInfo, LocationInfo selfInfo)
    {
        foreach (SeeingInfo info in seeingInfoList)
        {
            RBSDatum datum = new RBSDatum(info);
            database.AddDatum(datum);
        }

        this.deadBodyInfo = new RBSDatum(deadBodyInfo);
        this.selfInfo = new RBSDatum(selfInfo);

        database.AddDatum(this.deadBodyInfo);
        database.AddDatum(this.selfInfo);

        Debugger.Instance.RBSDBDebug(Agent, database);
    }

    public void AddData(List<RBSDatum> data)
    {
        foreach (RBSDatum datum in data)
        {
            AddDatum(datum);
        }
    }

    public void AddDatum(RBSDatum datum)
    {
        database.AddDatum(datum);

        Debugger.Instance.RBSDatumDebug(Agent, datum);
    }

    public void AddSuspectLevel(int points)
    {
        SuspectLevel += points;

        Debugger.Instance.SuspectDebug(Agent, SuspectLevel);
    }

    public void ShareDeadBodyInfo()
    {
        ShareInfo(deadBodyInfo);
    }

    public void ShareSelfInfo()
    {
        ShareInfo(selfInfo);
    }

    public void TakeTurn()
    {
        manager.RunRBS();
    }

    public void Vote()
    {
        Agent suspect = null;
        int maxSuspectLevel = int.MinValue;

        List<Agent> aliveAgents = GameManager.Instance.meeting.AliveAgents;
        foreach (Agent aliveAgent in aliveAgents)
        {
            if (aliveAgent != Agent && aliveAgent.MeetingAgent.SuspectLevel > maxSuspectLevel)
            {
                suspect = aliveAgent;
                maxSuspectLevel = aliveAgent.MeetingAgent.SuspectLevel;
            }
        }

        GameManager.Instance.meeting.Vote(suspect);
    }

    private void ShareInfo(RBSDatum datum)
    {
        GameManager.Instance.meeting.DisplayMessage(datum);
        GameManager.Instance.meeting.ShareDatum(datum);
    }
}
