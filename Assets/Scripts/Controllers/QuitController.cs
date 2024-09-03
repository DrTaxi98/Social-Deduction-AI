using UnityEngine;

public class QuitController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
