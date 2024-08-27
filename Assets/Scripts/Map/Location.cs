using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Location : MonoBehaviour
{
    private string locationName;
    private GUIStyle style = null;

    // Start is called before the first frame update
    void Start()
    {
        locationName = name.Split(' ').Last();
        SetGUIStyle();
    }

    private void SetGUIStyle()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
    }

    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponentInParent<Agent>();
        if (agent != null)
        {
            agent.CurrentLocation = this;

            Debugger.Instance.LocationDebug(agent, this);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (style != null)
        {
            Handles.Label(transform.position, locationName, style);
        }
    }
#endif
}
