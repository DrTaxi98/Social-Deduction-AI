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

    public List<SeeingInfo> SeeingInfo { get; } = new List<SeeingInfo>();
    public DeadBodyInfo DeadBodyInfo { get; private set; } = null;

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
                        ViewInfo info;

                        if (otherAgent.IsDead)
                        {
                            DeadBodyInfo deadBodyInfo = new DeadBodyInfo(agent, otherAgent);
                            info = deadBodyInfo;

                            if (agent.CanReport)
                            {
                                DeadBodyInfo = deadBodyInfo;
                            }
                        }
                        else
                        {
                            SeeingInfo agentSeenInfo = new SeeingInfo(agent, otherAgent);
                            int index = SeeingInfo.LastIndexOf(agentSeenInfo);

                            if (index != -1 && SeeingInfo[index].UpdateSeeingInfo(agentSeenInfo))
                            {
                                info = SeeingInfo[index];
                            }
                            else
                            {
                                SeeingInfo.Add(agentSeenInfo);
                                info = agentSeenInfo;
                            }
                        }

                        Debugger.Instance.FOVDebug(info);
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
