using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Location : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Agent agent = other.GetComponentInParent<Agent>();
        if (agent != null)
        {
            agent.CurrentLocation = this;
        }
    }
}
