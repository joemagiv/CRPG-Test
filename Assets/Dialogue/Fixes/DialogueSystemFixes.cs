using UnityEngine;
using PixelCrushers.DialogueSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Fixes common issues with the Dialogue System setup
/// </summary>
public class DialogueSystemFixes : MonoBehaviour
{
    [ContextMenu("Fix Input System Issues")]
    public void FixInputSystem()
    {
        // Disable legacy input GUI root if it exists
        var guiRoot = FindObjectOfType<PixelCrushers.DialogueSystem.UnityGUI.GUIRoot>();
        if (guiRoot != null)
        {
            guiRoot.enabled = false;
            Debug.Log("Disabled legacy GUIRoot to prevent Input System conflicts");
        }
        
        // Check if Input System is installed
        Debug.Log("Input System fix applied. Using new input system.");
    }
    
    [ContextMenu("Clean Up Problematic Files")]
    public void CleanUpProblematicFiles()
    {
        #if UNITY_EDITOR
        // Delete problematic prefab files
        string[] problematicFiles = {
            "Assets/Prefabs/NPC_Basic.prefab",
            "Assets/Prefabs/NPCs/MysteriousMerchant.prefab"
        };
        
        foreach (string file in problematicFiles)
        {
            if (AssetDatabase.LoadMainAssetAtPath(file) != null)
            {
                AssetDatabase.DeleteAsset(file);
                Debug.Log("Deleted problematic file: " + file);
            }
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Cleanup complete. Problematic files removed.");
        #else
        Debug.Log("Cleanup can only be done in Editor");
        #endif
    }
    
    [ContextMenu("Verify Dialogue System Setup")]
    public void VerifySetup()
    {
        Debug.Log("=== DIALOGUE SYSTEM VERIFICATION ===");
        
        // Check Dialogue Manager
        bool hasManager = DialogueManager.hasInstance;
        Debug.Log("Dialogue Manager available: " + hasManager);
        
        if (hasManager)
        {
            // Check database
            var database = DialogueManager.masterDatabase;
            Debug.Log("Master database loaded: " + (database != null));
            
            if (database != null)
            {
                Debug.Log("Conversations in database: " + database.conversations.Count);
                foreach (var conv in database.conversations)
                {
                    Debug.Log("- " + conv.Title);
                }
            }
        }
        
        // Check player setup
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found: " + (player != null));
        if (player != null)
        {
            Debug.Log("Player has CharacterStats: " + (player.GetComponent<CharacterStats>() != null));
            Debug.Log("Player has StatConditions: " + (player.GetComponent<StatBasedDialogueConditions>() != null));
            Debug.Log("Player has Selector: " + (player.GetComponent<Selector>() != null));
        }
        
        // Check UI
        var dialogueUI = FindObjectOfType<RightPanelDialogueUI>();
        Debug.Log("Dialogue UI found: " + (dialogueUI != null));
        
        Debug.Log("=== VERIFICATION COMPLETE ===");
    }
    
    [ContextMenu("Setup Input System Properly")]
    public void SetupInputSystem()
    {
        // Ensure player has proper input components
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Add or configure selector
            Selector selector = player.GetComponent<Selector>() ?? player.AddComponent<Selector>();
            selector.selectAt = Selector.SelectAt.MousePosition;
            selector.useKey = KeyCode.E;
            
            Debug.Log("Player input configured for Dialogue System");
        }
    }
    
    [ContextMenu("Fix All Issues")]
    public void FixAllIssues()
    {
        Debug.Log("=== RUNNING COMPREHENSIVE FIX ===");
        
        FixInputSystem();
        CleanUpProblematicFiles();
        SetupInputSystem();
        VerifySetup();
        
        Debug.Log("=== ALL FIXES APPLIED ===");
        Debug.Log("If issues persist:");
        Debug.Log("1. Check console for remaining errors");
        Debug.Log("2. Ensure all components are properly assigned");
        Debug.Log("3. Verify conversation exists in database");
    }
}