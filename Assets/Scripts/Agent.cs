using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    private NavMeshAgent agent;

    [Range(1, 10)] public int maxTasks = 5;
    private HashSet<POI> tasks = new HashSet<POI>();

    public void Init(Utils.AgentColor agentColor)
    {
        name = agentColor.name;
        GetComponentInChildren<Renderer>().material.color = agentColor.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        RandomTasks();
        NextTask();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RandomTasks()
    {
        tasks = GameManager.Instance.RandomPOIs(maxTasks);
    }

    public void NextTask()
    {
        if (tasks.Count == 0)
            RandomTasks();

        POI nextTask = null;
        float minSqrDistance = float.PositiveInfinity;

        foreach (POI task in tasks)
        {
            float sqrDistance = Vector3.SqrMagnitude(task.transform.position - transform.position);
            if (sqrDistance < minSqrDistance)
            {
                nextTask = task;
                minSqrDistance = sqrDistance;
            }
        }

        agent.destination = nextTask.transform.position;
    }

    public void RemoveTask(POI task)
    {
        tasks.Remove(task);
    }
}
