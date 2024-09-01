using UnityEngine;

public class MeetingController : MonoBehaviour
{
    private Meeting meeting;

    // Start is called before the first frame update
    void Start()
    {
        meeting = GetComponent<Meeting>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            meeting.NextTurn();
    }
}
