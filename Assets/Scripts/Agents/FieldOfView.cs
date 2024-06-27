using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(1f, 10f)] public float radius = 3f;
    [Range(0f, 360f)] public float angle = 90f;

    [Range(0.02f, 1f)] public float resampleTime = 0.2f;

    public LayerMask agentMask;
    public LayerMask raycastMask;

    private Agent agent;

    private List<Transform> agentsSeen = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<Agent>();
        StartCoroutine(See());
    }

    private IEnumerator See()
    {
        while(true)
        {
            SeeAgents();
            yield return new WaitForSeconds(resampleTime);
        }
    }

    private void SeeAgents()
    {
        agentsSeen.Clear();
        Collider[] otherColliders = Physics.OverlapSphere(transform.position, radius, agentMask);
        foreach (Collider otherCollider in otherColliders)
        {
            Transform otherTransform = otherCollider.transform;
            Vector3 verticalAdj = VerticalAdj(otherTransform.position);
            Vector3 direction = (verticalAdj - transform.position);
            if (Vector3.Angle(transform.forward, direction) < angle / 2)
            {
                float distance = direction.magnitude;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, distance, raycastMask) &&
                    hit.transform == otherTransform)
                {
                    agentsSeen.Add(otherTransform);

                    Agent otherAgent = otherTransform.GetComponent<Agent>();
                    if (otherAgent.IsDead)
                        agent.ReportDead(otherAgent);
                    else
                        agent.AddAgentSeen(otherAgent, otherAgent.CurrentLocation);
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

    private void OnDrawGizmos()
    {
        Vector3 angleDirA = AngleDir(-angle / 2);
        Vector3 angleDirB = AngleDir(angle / 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angleDirA * radius);
        Gizmos.DrawLine(transform.position, transform.position + angleDirB * radius);

        Handles.color = Color.yellow;
        Handles.DrawWireArc(transform.position, transform.up, angleDirA, angle, radius);

        Gizmos.color = Color.red;
        foreach (Transform agentSeen in agentsSeen)
        {
            Vector3 verticalAdj = VerticalAdj(agentSeen.position);
            Gizmos.DrawLine(transform.position, verticalAdj);
        }
    }
}
