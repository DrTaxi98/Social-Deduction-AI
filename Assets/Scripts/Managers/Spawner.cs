using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Utils.NameColor[] agentsNamesColors = { new Utils.NameColor("Red", Color.red), new Utils.NameColor("Green", Color.green),
                                              new Utils.NameColor("Blue", Color.blue), new Utils.NameColor("Yellow", Color.yellow),
                                              new Utils.NameColor("Magenta", Color.magenta), new Utils.NameColor("Cyan", Color.cyan),
                                              new Utils.NameColor("Orange", Utils.Orange), new Utils.NameColor("Black", Color.black)
                                            };

    public GameObject agentPrefab = null;
    public GameObject killerPrefab = null;
    public Transform spawnsParent = null;
    public Transform agentsParent = null;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnsParent == null)
            spawnsParent = GameObject.FindWithTag("Respawn").transform;

        Spawn();
    }

    private void Spawn()
    {
        int n = Mathf.Min(GameManager.Instance.agentCount, spawnsParent.childCount, agentsNamesColors.Length);
        int killerIndex = Random.Range(0, n);
        for (int i = 0; i < n; i++) {
            GameObject prefab = (i == killerIndex) ? killerPrefab : agentPrefab;
            Transform spawnTransform = spawnsParent.GetChild(i);
            
            GameObject agentGameObject = Instantiate(prefab, spawnTransform.position, spawnTransform.rotation, agentsParent);

            Agent agent = agentGameObject.GetComponent<Agent>();
            agent.Init(agentsNamesColors[i]);

            GameManager.Instance.AddAgent(agent);
        }
    }
}
