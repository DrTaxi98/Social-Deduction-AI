using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class AgentCanvas : MonoBehaviour
{
    private Agent agent;
    private Vector3 offset;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<Agent>();
        offset = transform.localPosition;
        rotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(agent.transform.position + offset,
            rotation);
    }
}
