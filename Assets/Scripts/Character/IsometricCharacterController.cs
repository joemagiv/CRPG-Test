using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Character controller for top-down isometric movement
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class IsometricCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float stoppingDistance = 0.1f;
    
    [Header("Animation Parameters")]
    public string speedParameter = "Speed";
    public string isMovingParameter = "IsMoving";
    
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 targetPosition;
    private bool isMoving = false;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        // Configure NavMeshAgent
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        agent.autoBraking = true;
    }
    
    void Update()
    {
        // Update animation based on movement
        if (animator != null)
        {
            float speed = agent.velocity.magnitude / moveSpeed;
            animator.SetFloat(speedParameter, speed);
            animator.SetBool(isMovingParameter, speed > 0.1f);
        }
        
        // Rotate character to face movement direction
        if (agent.velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Move character to a specific position
    /// </summary>
    /// <param name="position">Target world position</param>
    public void MoveToPosition(Vector3 position)
    {
        if (agent.isActiveAndEnabled)
        {
            targetPosition = position;
            agent.SetDestination(position);
            isMoving = true;
        }
    }
    
    /// <summary>
    /// Stop character movement
    /// </summary>
    public void StopMovement()
    {
        if (agent.isActiveAndEnabled)
        {
            agent.ResetPath();
            isMoving = false;
        }
    }
    
    /// <summary>
    /// Check if character is currently moving
    /// </summary>
    /// <returns>True if character is moving</returns>
    public bool IsMoving()
    {
        return isMoving && agent.remainingDistance > stoppingDistance;
    }
    
    /// <summary>
    /// Get current target position
    /// </summary>
    /// <returns>Current target position</returns>
    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}