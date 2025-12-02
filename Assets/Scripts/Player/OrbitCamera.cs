using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                 // Player transform

    [Header("Offset")]
    public Vector3 offset = new Vector3(0f, 3f, -5f); // Behind & above player

    [Header("Rotation Settings")]
    public float rotationSpeed = 3f;         // How fast camera rotates with mouse
    public float minPitch = -20f;           // Min vertical angle
    public float maxPitch = 60f;            // Max vertical angle

    private float currentYaw;               // Horizontal angle around player
    private float currentPitch;             // Vertical angle

    private void Start()
    {
        if (target != null)
        {
            // Initialize angles from current camera rotation
            Vector3 euler = transform.eulerAngles;
            currentYaw = euler.y;
            currentPitch = euler.x;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Read mouse movement (you can swap for controller axes if needed)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * rotationSpeed;
        currentPitch -= mouseY * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // Build rotation from angles
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Position camera at target + rotated offset
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = desiredPosition;

        // Look slightly above the target center (so we see the character nicely)
        Vector3 lookPoint = target.position + Vector3.up * 1.5f;
        transform.LookAt(lookPoint);
    }
}
