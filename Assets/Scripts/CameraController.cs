using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.0f, 30.0f)] public float movementSpeed = 20f;
    [Range(0.0f, 30.0f)] public float zoomSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Move();
        movement += Zoom();
        transform.Translate(movement * Time.deltaTime);
    }

    private Vector3 Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput) * movementSpeed;
        return movement;
    }

    private Vector3 Zoom()
    {
        float zoomInput = Input.GetAxisRaw("Mouse ScrollWheel");
        Vector3 zoom = -transform.up * zoomInput * zoomSpeed * 1000f;
        return zoom;
    }
}
