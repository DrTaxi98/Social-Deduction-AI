using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private HashSet<PointOfInterest> tasks = new HashSet<PointOfInterest>();
    private AgentInfo agentInfo = new AgentInfo();

    private GUIStyle style = new GUIStyle();

    public Color Color { get; private set; }
    public PointOfInterest CurrentTask { get; private set; } = null;
    public Location CurrentLocation { get; set; } = null;
    public bool IsDead { get; private set; } = false;

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

        style.normal.textColor = nameTextColor;

        NextTask();
    }

    public void SetAgentLocation()
    {
        agentInfo.AddAgentLocation(this, CurrentLocation);
    }

    public void AddAgentSeen(Agent agent, Location location)
    {
        agentInfo.AddAgentSeen(agent, location);
        if (name.CompareTo("Red") == 0)
            Debug.Log(name + " saw " + agent.name + " in " + location.name);
    }

    public void SetDeadBodyLocation(Agent deadBody, Location location)
    {
        agentInfo.SetDeadBodyLocation(deadBody, location);
    }

    public void StartTask()
    {
        StartCoroutine(AccomplishTask());
    }

    public void ReportDeadBody(Agent deadBody)
    {
        SetDeadBodyLocation(deadBody, deadBody.CurrentLocation);
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

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        Handles.Label(transform.position + new Vector3(-1f, 2f, 1.5f), name, style);
    }
#endif
}
