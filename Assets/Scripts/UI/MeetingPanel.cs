using UnityEngine;
using UnityEngine.UI;

public class MeetingPanel : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public float textBoxSpacing = 10f;
    public Transform content;
    public Scrollbar verticalScrollbar;

    private RectTransform contentRectTransform;
    private float offset;
    private int textBoxCount = 0;

    private void Awake()
    {
        contentRectTransform = content.GetComponent<RectTransform>();
        offset = textBoxPrefab.GetComponent<RectTransform>().rect.height + textBoxSpacing;
    }

    public void DisplayAgentMessage(Agent agent, string message)
    {
        GameObject textBoxGameObject = Instantiate(textBoxPrefab, content);
        textBoxGameObject.transform.localPosition += textBoxCount * offset * -textBoxPrefab.transform.up;

        TextBox textBox = textBoxGameObject.GetComponent<TextBox>();
        textBox.SetText(agent, message);

        textBoxCount++;
        AdjustContent();
        AdjustScrollbar();
    }

    private void AdjustContent()
    {
        float height = contentRectTransform.rect.height + offset;
        contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private void AdjustScrollbar()
    {
        verticalScrollbar.value = 0f;
    }
}
