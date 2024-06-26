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
        }
    }
    #endregion singleton

    public int randomSeed = 0;

    private List<Agent> agents = new List<Agent>();
    private POI[] pois = null;

    // Start is called before the first frame update
    void Start()
    {
        if (randomSeed == 0)
            randomSeed = (int)System.DateTime.Now.Ticks;
        Random.InitState(randomSeed);

        pois = FindObjectsByType<POI>(FindObjectsSortMode.InstanceID);
    }

    public void AddAgent(Agent agent)
    {
        agents.Add(agent);
    }

    private POI RandomPOI()
    {
        int randomIndex = Random.Range(0, pois.Length);
        return pois[randomIndex];
    }

    public HashSet<POI> RandomPOIs(int n)
    {
        n = Mathf.Min(n, pois.Length);
        HashSet<POI> randomPOIs = new HashSet<POI>();
        for (int i = 0; i < n; i++)
        {
            POI randomPOI;
            do
            {
                randomPOI = RandomPOI();
            } while (!randomPOIs.Add(randomPOI));
        }
        return randomPOIs;
    }
}
