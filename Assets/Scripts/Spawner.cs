using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Utils.AgentColor[] agentColors = { new Utils.AgentColor("Red", Color.red), new Utils.AgentColor("Green", Color.green),
                                              new Utils.AgentColor("Blue", Color.blue), new Utils.AgentColor("Yellow", Color.yellow),
                                              new Utils.AgentColor("Magenta", Color.magenta), new Utils.AgentColor("Cyan", Color.cyan),
                                              new Utils.AgentColor("Orange", Utils.orange), new Utils.AgentColor("Black", Color.black)
                                            };

    public GameObject agentToSpawn = null;
    [Range(4, 8)] public int agentsNumber = 5;

    private Transform spawns = null;
    private Transform agentsParent = null;

    // Start is called before the first frame update
    void Start()
    {
        spawns = GameObject.FindWithTag("Respawn").transform;
        agentsParent = GameObject.Find("Agents").transform;

        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < agentsNumber && i < spawns.childCount && i < agentColors.Length; i++) {
            Transform spawnTransform = spawns.GetChild(i);
            GameObject agentGameObject = Instantiate(agentToSpawn, spawnTransform.position, spawnTransform.rotation, agentsParent);

            Agent agent = agentGameObject.GetComponent<Agent>();
            agent.Init(agentColors[i]);

            GameManager.Instance.AddAgent(agent);
        }
    }
}
