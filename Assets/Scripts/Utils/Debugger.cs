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

    public bool locationDebug = false;
    public bool poiDebug = false;
    public bool taskDebug = false;
    public bool fovDebug = false;
    public bool killDebug = false;
    public bool reportDebug = false;
    public bool rbsDebug = false;
    public bool meetingDebug = false;

    public void LocationDebug(Agent agent, Location location)
    {
        if (locationDebug)
            Debug.Log(agent.name + " in " + location.name);
    }

    public void POIDebug(Agent agent, PointOfInterest poi)
    {
        if (poiDebug)
            Debug.Log(agent.name + " to " + poi.name);
    }

    public void TaskDebug(Agent agent, PointOfInterest poi, bool start)
    {
        if (taskDebug)
            Debug.Log(agent.name + (start ? " started" : " ended") + " task at " + poi.name);
    }

    public void FOVDebug(ViewInfo info)
    {
        if (fovDebug)
            Debug.Log(info);
    }

    public void KillCooldownDebug(Killer killer)
    {
        if (killDebug)
            Debug.Log(killer.name + " can kill");
    }

    public void KillDebug(Killer killer, Agent agent)
    {
        if (killDebug)
            Debug.Log(killer.name + " killed " + agent.name);
    }

    public void ReportDebug(DeadBodyInfo deadBodyInfo)
    {
        if (reportDebug)
            Debug.Log(deadBodyInfo.SeeingAgent.name + " reported " +
                deadBodyInfo.Agent.name + "'s dead body in " +
                deadBodyInfo.Location + " " +
                deadBodyInfo.TimeInterval);
    }

    public void RBSDBDebug(Agent agent, RBSDB database)
    {
        if (rbsDebug)
            Debug.Log(agent.name + " database:\n" + database);
    }

    public void RBSDatumDebug(Agent agent, RBSDatum datum)
    {
        if (rbsDebug)
            Debug.Log(agent.name + " new datum:\n" + datum);
    }

    public void SuspectDebug(Agent suspect, int suspectLevel)
    {
        if (meetingDebug)
            Debug.Log(suspect.name + " suspect level: " + suspectLevel);
    }

    public void VoteDebug(Agent agent, int votes)
    {
        if (meetingDebug)
            Debug.Log(agent.name + " received " + votes + " vote" + ((votes == 1) ? "" : "s"));
    }
}
