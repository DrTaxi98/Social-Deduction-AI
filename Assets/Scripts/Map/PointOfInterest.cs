using System.Linq;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    private string poiNumber;
    [Range(1f, 10f)] public float taskTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        poiNumber = name.Split(" ").ToList().Last();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Agent agent) && agent.CurrentTask == this)
        {
            agent.AccomplishTask();
        }
    }
}
