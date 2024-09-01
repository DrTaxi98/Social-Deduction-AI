using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            Init();
        }
    }
    #endregion

    [Range(4, 8)] public int agentCount = 5;
    public int randomSeed = 0;

    public Meeting meeting;

    private List<Agent> agents = new List<Agent>();
    private PointOfInterest[] pois = null;

    private CameraController cameraController;

    private List<Agent> AliveAgents
    {
        get
        {
            List<Agent> aliveAgents = new List<Agent>(agents);
            aliveAgents.RemoveAll((agent) => agent.IsDead);
            return aliveAgents;
        }
    }

    private void Init()
    {
        if (randomSeed == 0)
            randomSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(randomSeed);
    }

    // Start is called before the first frame update
    void Start()
    {
        pois = FindObjectsByType<PointOfInterest>(FindObjectsSortMode.InstanceID);
        cameraController = Camera.main.GetComponentInParent<CameraController>();
    }

    public void AddAgent(Agent agent)
    {
        agents.Add(agent);
    }

    public HashSet<PointOfInterest> RandomPOIs(int n)
    {
        n = Mathf.Min(n, pois.Length);
        HashSet<PointOfInterest> randomPOIs = new HashSet<PointOfInterest>();
        for (int i = 0; i < n; i++)
        {
            PointOfInterest randomPOI;
            do
            {
                randomPOI = RandomPOI();
            } while (!randomPOIs.Add(randomPOI));
        }
        return randomPOIs;
    }

    public void StartMeeting(Agent agent)
    {
        StopAgents();
        cameraController.enabled = false;
        meeting.StartMeeting(AliveAgents, agent);
    }

    private PointOfInterest RandomPOI()
    {
        int randomIndex = Random.Range(0, pois.Length);
        return pois[randomIndex];
    }

    private void StopAgents()
    {
        foreach (Agent agent in agents)
        {
            if (!agent.IsDead)
            {
                agent.Stop();
                agent.SetSelfInfo();
            }
        }
    }
}
