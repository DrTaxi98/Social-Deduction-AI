using UnityEngine;

public class Location : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Agent agent))
        {
            agent.CurrentLocation = this;
        }
    }
}
