using TMPro;
using UnityEngine;

public class NameTag : MonoBehaviour
{
    private Transform mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.transform;

        Agent agent = GetComponentInParent<Agent>();
        TMP_Text tmp_text = GetComponent<TMP_Text>();
        tmp_text.text = agent.name;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.rotation;
    }
}
