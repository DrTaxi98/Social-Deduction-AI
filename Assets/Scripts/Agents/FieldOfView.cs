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

    private Color losColor;
    private Color fovColor;
    private List<Transform> agentsSeen = new List<Transform>();

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
        while(true)
        {
            SeeAgents();
            yield return new WaitForSeconds(resampleTime);
            agentsSeen.Clear();
        }
    }

    private void SeeAgents()
    {
        Collider[] otherColliders = Physics.OverlapSphere(transform.position, radius, agentMask);
        foreach (Collider otherCollider in otherColliders)
        {
            if (otherCollider.transform != agent.transform)
            {
                Transform otherTransform = otherCollider.transform;
                Vector3 verticalAdj = VerticalAdj(otherTransform.position);
                Vector3 direction = (verticalAdj - transform.position);
                if (Vector3.Angle(transform.forward, direction) < angle / 2)
                {
                    float distance = direction.magnitude;
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, direction, out hit, distance, obstacleMask) &&
                        hit.transform == otherTransform)
                    {
                        agentsSeen.Add(otherTransform);

                        Agent otherAgent = otherTransform.GetComponent<Agent>();
                        if (otherAgent.IsDead)
                            agent.ReportDeadBody(otherAgent);
                        else
                            agent.AddAgentSeen(otherAgent, otherAgent.CurrentLocation);
                    }
                }
            }
        }
    }

    private Vector3 VerticalAdj(Vector3 position)
    {
        return new Vector3(position.x, transform.position.y, position.z);
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
        foreach (Transform agentSeen in agentsSeen)
        {
            Vector3 verticalAdj = VerticalAdj(agentSeen.position);
            Gizmos.DrawLine(transform.position, verticalAdj);
        }
    }
#endif
}
