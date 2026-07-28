using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// Click-to-move input controller for top-down isometric games
/// </summary>
[RequireComponent(typeof(Camera))]
public class ClickToMove : MonoBehaviour
{
    [Header("Input Settings")]
    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    
    [Header("Character Reference")]
    public IsometricCharacterController characterController;
    
    private Camera mainCamera;
    private Mouse mouse;
    
    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        mouse = Mouse.current;
    }
    
    void Update()
    {
        // Check for mouse click using Input System
        if (mouse.leftButton.wasPressedThisFrame && !IsPointerOverUI())
        {
            HandleMouseClick();
        }
    }
    
    /// <summary>
    /// Handle mouse click for movement
    /// </summary>
    private void HandleMouseClick()
    {
        if (characterController == null || mainCamera == null)
            return;
        
        // Create ray from mouse position
        Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
        RaycastHit hit;
        
        // Check if we hit the ground
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
        {
            // Check if there are any obstacles at the target position
            if (!Physics.CheckSphere(hit.point, characterController.GetComponent<UnityEngine.AI.NavMeshAgent>().radius, obstacleLayerMask))
            {
                // Move character to clicked position
                characterController.MoveToPosition(hit.point);
            }
        }
    }
    
    /// <summary>
    /// Check if pointer is over UI (works with both legacy and new input system)
    /// </summary>
    /// <returns>True if pointer is over UI</returns>
    private bool IsPointerOverUI()
    {
        // Check if pointer is over UI using EventSystem
        if (EventSystem.current != null)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }
        return false;
    }
    
    /// <summary>
    /// Set the character controller reference
    /// </summary>
    /// <param name="controller">Character controller to control</param>
    public void SetCharacterController(IsometricCharacterController controller)
    {
        characterController = controller;
    }
}