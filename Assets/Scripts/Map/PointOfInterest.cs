using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PointOfInterest : MonoBehaviour
{
    [Range(1f, 10f)] public float taskTime = 5f;

    private string poiNumber;

    // Start is called before the first frame update
    void Start()
    {
        poiNumber = name.Split(" ").ToList().Last();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Agent agent) && agent.CurrentTask == this)
        {
            agent.StartTask();
        }
    }
}
