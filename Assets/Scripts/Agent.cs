using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    public void Init(Utils.AgentColor agentColor)
    {
        name = agentColor.name;
        GetComponentInChildren<Renderer>().material.color = agentColor.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
