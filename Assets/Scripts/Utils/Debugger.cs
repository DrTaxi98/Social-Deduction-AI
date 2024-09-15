using System;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    #region singleton
    public static Debugger Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [Flags]
    public enum DebugFlags
    {
        Location = 1 << 0,
        POI = 1 << 1,
        Task = 1 << 2,
        FOV = 1 << 3,
        Kill = 1 << 4,
        Report = 1 << 5,
        RBS = 1 << 6,
        Meeting = 1 << 7
    }

    public DebugFlags debugFlags = 0;

    public void LocationDebug(Agent agent, Location location)
    {
        if (debugFlags.HasFlag(DebugFlags.Location))
            Debug.Log(agent.name + " in " + location.name);
    }

    public void POIDebug(Agent agent, PointOfInterest poi)
    {
        if (debugFlags.HasFlag(DebugFlags.POI))
            Debug.Log(agent.name + " to " + poi.name);
    }

    public void TaskDebug(Agent agent, PointOfInterest poi, bool start)
    {
        if (debugFlags.HasFlag(DebugFlags.Task))
            Debug.Log(agent.name + (start ? " started" : " ended") + " task at " + poi.name);
    }

    public void FOVDebug(ViewInfo info)
    {
        if (debugFlags.HasFlag(DebugFlags.FOV))
            Debug.Log(info);
    }

    public void KillCooldownDebug(Killer killer)
    {
        if (debugFlags.HasFlag(DebugFlags.Kill))
            Debug.Log(killer.name + " can kill");
    }

    public void KillDebug(Killer killer, Agent agent)
    {
        if (debugFlags.HasFlag(DebugFlags.Kill))
            Debug.Log(killer.name + " killed " + agent.name);
    }

    public void ReportDebug(DeadBodyInfo deadBodyInfo)
    {
        if (debugFlags.HasFlag(DebugFlags.Report))
            Debug.Log(deadBodyInfo.SeeingAgent.name + " reported " +
                deadBodyInfo.Agent.name + "'s dead body in " +
                deadBodyInfo.Location.name + " " +
                deadBodyInfo.TimeInterval);
    }

    public void RBSDBDebug(Agent agent, RBSDB database)
    {
        if (debugFlags.HasFlag(DebugFlags.RBS))
            Debug.Log(agent.name + " database:\n" + database);
    }

    public void RBSDatumDebug(Agent agent, RBSDatum datum)
    {
        if (debugFlags.HasFlag(DebugFlags.RBS))
            Debug.Log(agent.name + " new datum:\n" + datum);
    }

    public void SuspectDebug(Agent suspect, int suspectLevel)
    {
        if (debugFlags.HasFlag(DebugFlags.Meeting))
            Debug.Log(suspect.name + " suspect level: " + suspectLevel);
    }

    public void VoteDebug(Agent agent, int votes)
    {
        if (debugFlags.HasFlag(DebugFlags.Meeting))
            Debug.Log(agent.name + " received " + votes + " vote" + ((votes == 1) ? "" : "s"));
    }
}
