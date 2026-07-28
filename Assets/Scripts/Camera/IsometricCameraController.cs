using UnityEngine;

/// <summary>
/// Isometric camera controller for top-down adventure games
/// </summary>
public class IsometricCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform target;
    public float height = 10f;
    public float distance = 10f;
    public float rotationAngle = 45f; // Isometric angle
    public float smoothSpeed = 5f;
    
    [Header("Movement Boundaries")]
    public Vector2 minPosition = new Vector2(-50f, -50f);
    public Vector2 maxPosition = new Vector2(50f, 50f);
    
    private Vector3 offset;
    private Vector3 targetPosition;
    
    void Start()
    {
        if (target != null)
        {
            // Calculate initial offset based on isometric angle
            CalculateOffset();
        }
    }
    
    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate target position with offset
            targetPosition = target.position + offset;
            
            // Apply movement boundaries
            targetPosition.x = Mathf.Clamp(targetPosition.x, minPosition.x, maxPosition.x);
            targetPosition.z = Mathf.Clamp(targetPosition.z, minPosition.y, maxPosition.y);
            
            // Smoothly move camera to target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            
            // Make camera look at target
            transform.LookAt(target.position);
        }
    }
    
    /// <summary>
    /// Calculate camera offset based on isometric settings
    /// </summary>
    private void CalculateOffset()
    {
        // Convert angle to radians
        float radians = rotationAngle * Mathf.Deg2Rad;
        
        // Calculate offset using trigonometry
        float xOffset = distance * Mathf.Cos(radians);
        float yOffset = height;
        float zOffset = distance * Mathf.Sin(radians);
        
        offset = new Vector3(xOffset, yOffset, zOffset);
    }
    
    /// <summary>
    /// Set camera target
    /// </summary>
    /// <param name="newTarget">New target transform</param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            CalculateOffset();
        }
    }
    
    /// <summary>
    /// Set camera height
    /// </summary>
    /// <param name="newHeight">New camera height</param>
    public void SetHeight(float newHeight)
    {
        height = newHeight;
        CalculateOffset();
    }
    
    /// <summary>
    /// Set camera distance
    /// </summary>
    /// <param name="newDistance">New camera distance</param>
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        CalculateOffset();
    }
    
    /// <summary>
    /// Set camera rotation angle
    /// </summary>
    /// <param name="newAngle">New rotation angle in degrees</param>
    public void SetRotationAngle(float newAngle)
    {
        rotationAngle = newAngle;
        CalculateOffset();
    }
}