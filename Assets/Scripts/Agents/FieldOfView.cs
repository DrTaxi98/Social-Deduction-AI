using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(1f, 10f)] public float radius = 5f;
    [Range(0f, 360f)] public float angle = 120f;
    [Range(0.02f, 1f)] public float resampleTime = 0.2f;
    public LayerMask agentMask;
    public LayerMask obstacleMask;

    public float transparency = 0.15f;

    private Agent agent;
    private List<Transform> currentlySeenAgents = new List<Transform>();

    private Color losColor;
    private Color fovColor;

    public List<ViewInfo> SeenAgentInfoList { get; } = new List<ViewInfo>();
    public ViewInfo DeadBodyInfo { get; private set; } = null;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<Agent>();

        losColor = agent.Color;
        fovColor = losColor;
        fovColor.a = transparency;

        StartCoroutine(See());
    }

    private IEnumerator See()
    {
        while (true)
        {
            SeeAgents();

            if (DeadBodyInfo != null)
            {
                agent.ReportDeadBody(DeadBodyInfo);
            }

            yield return new WaitForSeconds(resampleTime);

            currentlySeenAgents.Clear();
        }
    }

    private void SeeAgents()
    {
        Collider[] otherColliders = Physics.OverlapSphere(transform.position, radius, agentMask);
        foreach (Collider otherCollider in otherColliders)
        {
            Transform otherTransform = otherCollider.transform;
            if (otherTransform.parent != agent.transform)
            {
                Vector3 direction = otherTransform.position - transform.position;
                if (Vector3.Angle(transform.forward, direction) < angle / 2)
                {
                    float distance = direction.magnitude;
                    RaycastHit hit;
                    if (!Physics.Raycast(transform.position, direction, out hit, distance, obstacleMask))
                    {
                        currentlySeenAgents.Add(otherTransform);

                        Agent otherAgent = otherTransform.GetComponentInParent<Agent>();
                        ViewInfo seenAgentInfo = new ViewInfo(agent, otherAgent);

                        if (otherAgent.IsDead)
                        {
                            if (agent.CanReport)
                            {
                                DeadBodyInfo = seenAgentInfo;
                            }
                        }
                        else
                        {
                            int index = SeenAgentInfoList.LastIndexOf(seenAgentInfo);
                            if (index != -1 && SeenAgentInfoList[index].UpdateToTimestamp(seenAgentInfo))
                            {
                                seenAgentInfo = SeenAgentInfoList[index];
                            }
                            else
                            {
                                SeenAgentInfoList.Add(seenAgentInfo);
                            }
                        }

                        Debugger.Instance.FOVDebug(agent, seenAgentInfo);
                    }
                }
            }
        }
    }

    private Vector3 AngleDir(float angleDeg)
    {
        angleDeg += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 angleDir = AngleDir(-angle / 2);

        Handles.color = fovColor;
        Handles.DrawSolidArc(transform.position, transform.up, angleDir, angle, radius);

        Gizmos.color = losColor;
        foreach (Transform seenAgent in currentlySeenAgents)
        {
            Gizmos.DrawLine(transform.position, seenAgent.position);
        }
    }
#endif
}
