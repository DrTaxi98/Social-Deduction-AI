using TMPro;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    public TMP_Text agentNameText;
    public TMP_Text messageText;

    public void SetText(Agent agent, string message)
    {
        agentNameText.text = agent.name;
        agentNameText.color = agent.Color;
        if (agent is Killer)
            agentNameText.fontStyle |= FontStyles.Underline;

        messageText.text = message;
    }
}
