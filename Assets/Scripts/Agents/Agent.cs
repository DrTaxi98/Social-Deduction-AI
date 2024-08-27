using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [Range(1, 10)] public int maxTasks = 5;

    public Color nameTextColor = Color.white;

    private NavMeshAgent agent;

    private GameObject aliveBody;
    private GameObject deadBody;
    private FieldOfView fov;

    private HashSet<PointOfInterest> tasks = new HashSet<PointOfInterest>();
    private AgentInfo agentInfo = new AgentInfo();

    public Color Color { get; private set; }
    public PointOfInterest CurrentTask { get; private set; } = null;
    public Location CurrentLocation { get; set; } = null;
    public bool IsDead { get; private set; } = false;
    public bool CanReport { get; protected set; } = true;

    public void Init(Utils.NameColor agentColor)
    {
        name = agentColor.name;
        Color = agentColor.color;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        aliveBody = transform.GetChild(0).gameObject;
        deadBody = transform.GetChild(1).gameObject;

        aliveBody.GetComponent<Renderer>().material.color = Color;
        deadBody.GetComponent<Renderer>().material.color = Color;

        fov = GetComponentInChildren<FieldOfView>();

        NextTask();
    }

    public void AddSelfLocationInfo()
    {
        agentInfo.AddAgentLocationInfo(this);
    }

    public void AddAgentSeenInfo(Agent agentSeen)
    {
        agentInfo.AddAgentSeenInfo(this, agentSeen);
    }

    public void SetDeadBodyInfo(Agent agentSeeing, Agent deadBody)
    {
        agentInfo.SetDeadBodyInfo(agentSeeing, deadBody);
    }

    public void StartTask()
    {
        StartCoroutine(AccomplishTask());
    }

    public void ReportDeadBody(Agent deadBody)
    {
        SetDeadBodyInfo(this, deadBody);
        GameManager.Instance.StartMeeting();
    }

    public void Stop()
    {
        agent.isStopped = true;

        MonoBehaviour[] children = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour child in children)
        {
            child.StopAllCoroutines();
        }
    }

    public void Die()
    {
        Stop();
        IsDead = true;

        aliveBody.SetActive(false);
        deadBody.SetActive(true);
    }

    private void RandomTasks()
    {
        tasks = GameManager.Instance.RandomPOIs(maxTasks);
    }

    private void NextTask()
    {
        if (tasks.Count == 0)
            RandomTasks();

        PointOfInterest oldTask = CurrentTask;
        CurrentTask = null;
        float minSqrDistance = float.PositiveInfinity;

        foreach (PointOfInterest task in tasks)
        {
            float sqrDistance = Vector3.SqrMagnitude(task.transform.position - transform.position);
            if (sqrDistance < minSqrDistance)
            {
                CurrentTask = task;
                minSqrDistance = sqrDistance;
            }
        }

        if (CurrentTask == oldTask)
            StartTask();
        else
            agent.destination = CurrentTask.transform.position;
    }

    private IEnumerator AccomplishTask()
    {
        yield return new WaitForSeconds(CurrentTask.taskTime);

        EndTask();
    }

    private void EndTask()
    {
        tasks.Remove(CurrentTask);
        NextTask();
    }
}
