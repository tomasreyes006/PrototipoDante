using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Referencia")]
    public Transform target; 

    [Header("Configuración")]
    public Vector3 offset = new Vector3(0f, 2f, -3f); 
    public float mouseSensitivity = 3f;
    public float rotationSmoothTime = 0.12f;

    private float pitch = 0f; 
    private float yaw = 0f;   
    private Vector3 rotationVelocity;

    void Start()
    {
        if (target == null && GameObject.FindWithTag("Player") != null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -35f, 60f);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 targetPosition = target.position + targetRotation * offset;

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
