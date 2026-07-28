using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Helper class to handle input system compatibility issues
/// </summary>
public static class InputSystemCompatibility
{
    /// <summary>
    /// Check if pointer is over UI - works with both input systems
    /// </summary>
    public static bool IsPointerOverUI()
    {
        if (EventSystem.current == null)
            return false;
            
        // Try the new Input System method first
        try
        {
            // This will work if using Input System package
            if (EventSystem.current.IsPointerOverGameObject())
                return true;
        }
        catch
        {
            // Fall back to legacy method if there are issues
        }
        
        // Legacy fallback
        return EventSystem.current.IsPointerOverGameObject();
    }
    
    /// <summary>
    /// Create a compatible EventSystem for the current input setup
    /// </summary>
    public static void EnsureCompatibleEventSystem()
    {
        if (Object.FindAnyObjectByType<EventSystem>() != null)
            return;
            
        GameObject eventSystemObject = new GameObject("EventSystem");
        EventSystem eventSystem = eventSystemObject.AddComponent<EventSystem>();
        
        // Try Input System first
        try
        {
            // Check if Input System is available
            var inputSystemType = System.Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputSystemType != null)
            {
                eventSystemObject.AddComponent(inputSystemType);
                Debug.Log("Using Input System UI Input Module");
                return;
            }
        }
        catch
        {
            // Fall back to legacy
        }
        
        // Use legacy input module
        eventSystemObject.AddComponent<StandaloneInputModule>();
        Debug.Log("Using Legacy Standalone Input Module");
    }
}