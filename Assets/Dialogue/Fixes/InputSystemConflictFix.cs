using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Permanently fixes the Input System conflict with Dialogue System
/// </summary>
[DefaultExecutionOrder(-100)] // Run early to prevent errors
public class InputSystemConflictFix : MonoBehaviour
{
    void Awake()
    {
        FixInputSystemConflict();
    }
    
    void FixInputSystemConflict()
    {
        // Disable the legacy GUIRoot that's causing the Input System conflict
        var guiRoot = FindObjectOfType<PixelCrushers.DialogueSystem.UnityGUI.GUIRoot>();
        
        if (guiRoot != null)
        {
            // Disable the component to prevent it from processing input
            guiRoot.enabled = false;
            
            // Optionally destroy it if you're not using legacy GUI at all
            // Destroy(guiRoot);
            
            Debug.Log("Fixed Input System conflict: Disabled legacy GUIRoot component");
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeEarly()
    {
        // This ensures the fix runs before any Dialogue System initialization
        GameObject fixObject = new GameObject("InputSystemConflictFix");
        fixObject.AddComponent<InputSystemConflictFix>();
        DontDestroyOnLoad(fixObject);
    }
}