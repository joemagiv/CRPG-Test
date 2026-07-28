using UnityEngine;

/// <summary>
/// Test script to verify character animation is working properly
/// </summary>
[RequireComponent(typeof(Animator))]
public class TestCharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private float speed = 0f;
    private bool isMoving = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Character animation test started. Press W/S to change speed, Space to toggle movement.");
    }
    
    void Update()
    {
        // Test input for changing animation states
        if (Input.GetKey(KeyCode.W))
        {
            speed = Mathf.Min(speed + Time.deltaTime * 2f, 1f);
            isMoving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            speed = Mathf.Max(speed - Time.deltaTime * 2f, 0f);
            isMoving = speed > 0.1f;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isMoving = !isMoving;
        }
        
        // Update animator parameters
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsMoving", isMoving);
        }
        
        // Debug current state
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Current Speed: " + speed + ", IsMoving: " + isMoving);
            Debug.Log("Current Animator State: " + GetCurrentAnimatorStateInfo());
        }
    }
    
    private string GetCurrentAnimatorStateInfo()
    {
        if (animator == null) return "No animator";
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return "State: " + stateInfo.IsName("Idle") + " (Idle), " + 
               stateInfo.IsName("Walk") + " (Walk), " + 
               stateInfo.IsName("Run") + " (Run)";
    }
}