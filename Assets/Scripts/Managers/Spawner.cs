using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Utils.AgentColor[] agentColors = { new Utils.AgentColor("Red", Color.red), new Utils.AgentColor("Green", Color.green),
                                              new Utils.AgentColor("Blue", Color.blue), new Utils.AgentColor("Yellow", Color.yellow),
                                              new Utils.AgentColor("Magenta", Color.magenta), new Utils.AgentColor("Cyan", Color.cyan),
                                              new Utils.AgentColor("Orange", Utils.orange), new Utils.AgentColor("Black", Color.black)
                                            };

    public GameObject agentToSpawn = null;
    public GameObject killerToSpawn = null;
    [Range(4, 8)] public int agentsNumber = 5;

    private Transform spawns = null;
    public Transform agentsParent = null;

    // Start is called before the first frame update
    void Start()
    {
        spawns = GameObject.FindWithTag("Respawn").transform;

        Spawn();
    }

    private void Spawn()
    {
        int n = Mathf.Min(agentsNumber, spawns.childCount, agentColors.Length);
        int killerIndex = Random.Range(0, n);
        for (int i = 0; i < n; i++) {
            GameObject gameObjectToSpawn = (i == killerIndex) ? killerToSpawn : agentToSpawn;
            Transform spawnTransform = spawns.GetChild(i);
            
            GameObject agentGameObject = Instantiate(gameObjectToSpawn, spawnTransform.position, spawnTransform.rotation, agentsParent);

            Agent agent = agentGameObject.GetComponent<Agent>();
            agent.Init(agentColors[i]);

            GameManager.Instance.AddAgent(agent);
        }
    }
}
