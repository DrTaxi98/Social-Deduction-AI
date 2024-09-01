using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.0f, 30.0f)] public float movementSpeed = 10f;
    [Range(0.0f, 30.0f)] public float zoomSpeed = 10f;

    public Vector3 min = new Vector3(-16f, 6f, -20f);
    public Vector3 max = new Vector3(16f, 30f, 6f);

    private Vector3 defaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Move();
        movement += Zoom();
        Vector3 newPosition = transform.position + movement * Time.deltaTime;

        SetPosition(newPosition);
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
        Vector3 zoom = 1000f * zoomInput * zoomSpeed * -transform.up;
        return zoom;
    }

    private void SetPosition(Vector3 position)
    {
        transform.position = Clamp(position);
    }

    private Vector3 Clamp(Vector3 vector)
    {
        vector.x = Mathf.Clamp(vector.x, min.x, max.x);
        vector.y = Mathf.Clamp(vector.y, min.y, max.y);
        vector.z = Mathf.Clamp(vector.z, min.z, max.z);

        return vector;
    }

    private void ResetCamera()
    {
        SetPosition(defaultPosition);
    }

    private void OnDisable()
    {
        ResetCamera();
    }
}
