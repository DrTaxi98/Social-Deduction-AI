using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PointOfInterest : MonoBehaviour
{
    [Range(1f, 10f)] public float taskTime = 5f;

    private string poiNumber;
    private GUIStyle style = null;
    private List<Agent> agents = new List<Agent>();
    private Color defaultColor = Color.clear;
    private Color color;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        poiNumber = name.Split(' ').Last();
        radius = transform.lossyScale.x / 2;
        color = defaultColor;
        SetGUIStyle();
    }

    public void AddAgent(Agent agent)
    {
        agents.Add(agent);
        color = agent.Color;
    }

    public void RemoveAgent(Agent agent)
    {
        agents.Remove(agent);

        if (agents.Count > 0)
            color = agents.Last().Color;
        else
            color = defaultColor;
    }

    private void SetGUIStyle()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
    }

    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponentInParent<Agent>();
        if (agent != null && agent.CurrentTask == this)
        {
            agent.StartTask();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (style != null)
        {
            Handles.color = color;
            Handles.DrawSolidDisc(transform.position, transform.up, radius);
            Handles.Label(transform.position, poiNumber, style);
        }
    }
#endif
}
