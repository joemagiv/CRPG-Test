using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Fixes Dialogue System UI warnings by properly configuring the custom UI
/// </summary>
[DefaultExecutionOrder(-100)] // Run early
public class DialogueUIWarningsFix : MonoBehaviour
{
    void Awake()
    {
        ConfigureDialogueUI();
        DisableUnusedComponents();
    }
    
    void ConfigureDialogueUI()
    {
        if (!DialogueManager.hasInstance)
        {
            Debug.Log("Dialogue Manager not available yet");
            return;
        }
        
        // Find our custom UI
        var customUI = FindObjectOfType<RightPanelDialogueUI>();
        
        if (customUI != null)
        {
            // Assign our custom UI to the Dialogue Manager
            if (customUI is IDialogueUI dialogueUI)
            {
                DialogueManager.dialogueUI = dialogueUI;
                Debug.Log("Assigned custom dialogue UI to Dialogue Manager");
            }
            else
            {
                Debug.LogError("Custom UI does not implement IDialogueUI interface");
            }
        }
        else
        {
            Debug.LogWarning("Custom dialogue UI (RightPanelDialogueUI) not found");
        }
    }
    
    void DisableUnusedComponents()
    {
        // Disable standard UI selector if it exists and isn't needed
        var standardSelector = FindObjectOfType<SelectorUseStandardUIElements>();
        if (standardSelector != null)
        {
            standardSelector.enabled = false;
            Debug.Log("Disabled standard UI selector elements");
        }
    }
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void InitializeEarly()
    {
        GameObject fixObject = new GameObject("DialogueUIWarningsFix");
        fixObject.AddComponent<DialogueUIWarningsFix>();
        DontDestroyOnLoad(fixObject);
    }
}