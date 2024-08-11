using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class NameTag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Agent agent = GetComponentInParent<Agent>();
        TMP_Text tmp_text = GetComponent<TMP_Text>();
        tmp_text.text = agent.name;
        tmp_text.color = agent.nameTextColor;
    }
}
