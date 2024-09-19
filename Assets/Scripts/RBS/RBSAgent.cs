using System.Collections.Generic;
using UnityEngine;

public class RBSAgent
{
    private RBSDB database;
    private RBSManager manager;

    private RBSDatum deadBodyInfo = null;
    private RBSDatum selfInfo = null;

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

        if (deadBodyInfo != null)
        {
            this.deadBodyInfo = new RBSDatum(deadBodyInfo);
            database.AddDatum(this.deadBodyInfo);
        }

        this.selfInfo = new RBSDatum(selfInfo);
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
        if (deadBodyInfo != null)
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
        List<Agent> suspects = GetSuspects();
        int randomIndex = Random.Range(0, suspects.Count);
        GameManager.Instance.meeting.Vote(suspects[randomIndex]);
    }

    private void ShareInfo(RBSDatum datum)
    {
        GameManager.Instance.meeting.DisplayMessage(datum);
        GameManager.Instance.meeting.ShareDatum(datum);
    }

    private List<Agent> GetSuspects()
    {
        List<Agent> suspects = new List<Agent>();
        int maxSuspectLevel = int.MinValue;

        List<Agent> aliveAgents = GameManager.Instance.meeting.AliveAgents;
        foreach (Agent aliveAgent in aliveAgents)
        {
            if (aliveAgent != Agent)
            {
                int suspectLevel = aliveAgent.MeetingAgent.SuspectLevel;

                if (suspectLevel == maxSuspectLevel)
                {
                    suspects.Add(aliveAgent);
                }
                else if (suspectLevel > maxSuspectLevel)
                {
                    suspects.Clear();
                    suspects.Add(aliveAgent);
                    maxSuspectLevel = suspectLevel;
                }
            }
        }

        return suspects;
    }
}
