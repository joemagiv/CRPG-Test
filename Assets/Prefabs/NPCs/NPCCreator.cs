using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Creates NPCs with dialogue functionality
/// </summary>
public class NPCCreator : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Mysterious Merchant";
    public Color dialogueColor = new Color(0.6f, 0.4f, 0.8f); // Purple
    public string conversationTitle = "CharismaTest";
    public Vector3 spawnPosition = new Vector3(0, 0, 3);
    
    [ContextMenu("Create NPC")]
    public void CreateNPC()
    {
        GameObject npcObj = new GameObject(npcName);
        npcObj.transform.position = spawnPosition;
        
        // Add and configure NPC Controller
        NPCController npc = npcObj.AddComponent<NPCController>();
        npc.SetNPCName(npcName);
        npc.SetDialogueColor(dialogueColor);
        npc.SetConversation(conversationTitle);
        
        // Add interaction components
        AddInteractionComponents(npcObj);
        
        // Add visual representation
        AddVisualRepresentation(npcObj);
        
        Debug.Log("Created NPC: " + npcName);
        Debug.Log("- Position: " + spawnPosition);
        Debug.Log("- Conversation: " + conversationTitle);
        Debug.Log("- Approach and press E to interact");
    }
    
    void AddInteractionComponents(GameObject obj)
    {
        // Add Usable component for player interaction
        Usable usable = obj.AddComponent<Usable>();
        usable.overrideName = npcName;
        usable.overrideUseMessage = "Talk";
        usable.maxUseDistance = 5f;
        
        // Add Dialogue System Trigger
        DialogueSystemTrigger trigger = obj.AddComponent<DialogueSystemTrigger>();
        trigger.conversation = conversationTitle;
        trigger.trigger = DialogueSystemTriggerEvent.OnUse;
        
        Debug.Log("Added interaction components");
    }
    
    void AddVisualRepresentation(GameObject obj)
    {
        // Add collider
        CapsuleCollider capsule = obj.AddComponent<CapsuleCollider>();
        capsule.radius = 0.3f;
        capsule.height = 1.8f;
        capsule.center = new Vector3(0, 0.9f, 0);
        
        // Add visual indicator
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        indicator.transform.SetParent(obj.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.6f, 1.8f, 0.6f);
        
        Renderer renderer = indicator.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = dialogueColor;
        
        Debug.Log("Added visual representation");
    }
    
    [ContextMenu("Test Conversation")]
    public void TestConversation()
    {
        if (DialogueManager.hasInstance)
        {
            var conversation = DialogueManager.masterDatabase.GetConversation(conversationTitle);
            if (conversation != null)
            {
                DialogueManager.StartConversation(conversationTitle);
                Debug.Log("Started conversation: " + conversationTitle);
            }
            else
            {
                Debug.LogError("Conversation '" + conversationTitle + "' not found");
                Debug.Log("Please create it manually first (see ManualCreationGuide.md)");
            }
        }
        else
        {
            Debug.LogError("Dialogue Manager not available");
        }
    }
    
    [ContextMenu("Open Manual Guide")]
    public void OpenManualGuide()
    {
        Debug.Log("=== MANUAL CREATION GUIDE ===");
        Debug.Log("1. Open Dialogue Database Editor");
        Debug.Log("2. Create Actors:");
        Debug.Log("   - NPC: 'Mysterious Merchant' (purple)");
        Debug.Log("   - Player: 'Player' (white)");
        Debug.Log("3. Create Conversation: 'CharismaTest'");
        Debug.Log("4. Add dialogue entries (see guide for details)");
        Debug.Log("5. Save database");
        Debug.Log("6. Click 'Create NPC' to make this NPC");
    }
}