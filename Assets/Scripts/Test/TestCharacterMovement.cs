using UnityEngine;

/// <summary>
/// Test script to verify character movement functionality
/// </summary>
public class TestCharacterMovement : MonoBehaviour
{
    public IsometricCharacterController characterController;
    public Transform[] testPoints;
    
    private int currentTestPoint = 0;
    private float testTimer = 0f;
    private float testInterval = 3f;
    
    void Update()
    {
        testTimer += Time.deltaTime;
        
        if (testTimer >= testInterval && testPoints != null && testPoints.Length > 0)
        {
            testTimer = 0f;
            
            // Move to next test point
            if (characterController != null)
            {
                characterController.MoveToPosition(testPoints[currentTestPoint].position);
                Debug.Log("Moving character to test point: " + currentTestPoint);
            }
            
            currentTestPoint = (currentTestPoint + 1) % testPoints.Length;
        }
    }
    
    void OnDrawGizmos()
    {
        if (testPoints != null)
        {
            Gizmos.color = Color.green;
            
            foreach (Transform point in testPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawSphere(point.position, 0.5f);
                }
            }
        }
    }
}