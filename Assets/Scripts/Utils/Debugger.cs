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

    public void FOVDebug(Agent agent, ViewInfo seenAgentInfo)
    {
        if (fovDebug)
            Debug.Log(agent.name + " saw " + seenAgentInfo);
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

    public void ReportDebug(Agent agent, ViewInfo deadBodyInfo)
    {
        if (reportDebug)
            Debug.Log(agent.name + " reported " + deadBodyInfo);
    }
}
