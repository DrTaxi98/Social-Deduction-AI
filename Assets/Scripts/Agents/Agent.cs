using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [Range(1, 10)] public int maxTasks = 5;

    public Color nameTextColor = Color.black;

    private NavMeshAgent agent;
    private GameObject aliveBody;
    private GameObject deadBody;
    private FieldOfView fov;
    private RBSAgent meetingAgent;

    private HashSet<PointOfInterest> tasks = new HashSet<PointOfInterest>();
    private ViewInfo selfInfo = null;

    private GUIStyle style = null;

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
        // meetingAgent = GetComponent<RBSAgent>();

        SetGUIStyle();

        NextTask();
    }

    public void SetSelfInfo()
    {
        selfInfo = new ViewInfo(this);
    }

    public void StartTask()
    {
        Debugger.Instance.TaskDebug(this, CurrentTask, true);

        StartCoroutine(AccomplishTask());
    }

    public void ReportDeadBody(ViewInfo deadBodyInfo)
    {
        Debugger.Instance.ReportDebug(this, deadBodyInfo);

        GameManager.Instance.StartMeeting(this);
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

        CurrentTask.RemoveAgent(this);
    }

    public string TakeTurnInMeeting()
    {
        // meetingAgent.TakeTurn();
        return "I am " + ((this is Killer) ? "" : "not ") + "the killer.";
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

        CurrentTask.AddAgent(this);

        Debugger.Instance.POIDebug(this, CurrentTask);

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
        CurrentTask.RemoveAgent(this);

        Debugger.Instance.TaskDebug(this, CurrentTask, false);

        tasks.Remove(CurrentTask);
        NextTask();
    }

    private void SetGUIStyle()
    {
        style = new GUIStyle();
        style.normal.textColor = nameTextColor;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.LowerCenter;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (style != null)
        {
            Vector3 position = transform.position + 2f * transform.up;
            Handles.Label(position, name, style);
        }
    }
#endif
}
