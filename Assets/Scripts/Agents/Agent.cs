using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject aliveBody;
    private GameObject deadBody;

    public Color Color { get; private set; }

    [Range(1, 10)] public int maxTasks = 5;
    private HashSet<PointOfInterest> tasks = new HashSet<PointOfInterest>();

    public PointOfInterest CurrentTask { get; private set; } = null;
    public Location CurrentLocation { get; set; } = null;

    public bool IsDead { get; private set; } = false;

    private AgentInfo agentInfo = new AgentInfo();

    private GUIStyle style = new GUIStyle();
    public Color nameColor = Color.white;

    public void Init(Utils.AgentColor agentColor)
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

        style.normal.textColor = nameColor;

        NextTask();
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
            AccomplishTask();
        else
            agent.destination = CurrentTask.transform.position;
    }

    public void AccomplishTask()
    {
        StartCoroutine(DoTask());
    }

    private IEnumerator DoTask()
    {
        yield return new WaitForSeconds(CurrentTask.taskTime);

        TaskAccomplished();
    }

    private void TaskAccomplished()
    {
        tasks.Remove(CurrentTask);
        NextTask();
    }

    public void Die()
    {
        Stop();
        IsDead = true;

        aliveBody.SetActive(false);
        deadBody.SetActive(true);
    }

    public void AddAgentSeen(Agent agent, Location location)
    {
        agentInfo.AddAgentSeen(agent, location);
        if (name == "Red")
            Debug.Log(name + " saw " + agent.name + " in " + location.name);
    }

    public void ReportDead(Agent dead)
    {
        SetDead(dead, dead.CurrentLocation);
        GameManager.Instance.StartMeeting();
    }

    public void SetDead(Agent agent, Location location)
    {
        agentInfo.SetDead(agent, location);
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

    public void SetAgentLocation()
    {
        agentInfo.AddAgentLocation(this, CurrentLocation);
    }

    protected virtual void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(-1f, 2f, 1.5f), name, style);
    }
}
