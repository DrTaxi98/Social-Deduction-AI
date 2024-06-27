using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [Range(1f, 10f)] public float taskTime = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Agent agent) && agent.CurrentTask == this)
        {
            agent.AccomplishTask();
        }
    }
}
