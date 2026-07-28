using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Properly sets up an interactive NPC with dialogue
/// </summary>
public class ProperNPCSetup : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Test NPC";
    public Color dialogueColor = Color.white;
    public string conversationTitle = "TestConversation";
    public Vector3 spawnPosition = Vector3.zero;
    
    [ContextMenu("Setup Complete NPC")]
    public void SetupCompleteNPC()
    {
        GameObject npcObj = new GameObject(npcName);
        npcObj.transform.position = spawnPosition;
        
        // Add and configure all required components
        SetupNPCController(npcObj);
        SetupUsable(npcObj);
        SetupDialogueTrigger(npcObj);
        SetupCollider(npcObj);
        SetupVisuals(npcObj);
        
        Debug.Log("NPC setup complete: " + npcName);
        Debug.Log("- Position: " + spawnPosition);
        Debug.Log("- Conversation: " + conversationTitle);
        Debug.Log("- Click the NPC to interact (make sure conversation exists)");
    }
    
    void SetupNPCController(GameObject obj)
    {
        NPCController npc = obj.AddComponent<NPCController>();
        npc.SetNPCName(npcName);
        npc.SetDialogueColor(dialogueColor);
        npc.SetConversation(conversationTitle);
    }
    
    void SetupUsable(GameObject obj)
    {
        Usable usable = obj.AddComponent<Usable>();
        usable.overrideName = npcName;
        usable.overrideUseMessage = "Talk";
        usable.maxUseDistance = 5f;
        // Note: Usable doesn't have useKey property in this version
        // Configuration is done through the Selector component on the player
        
        Debug.Log("Usable component configured");
    }
    
    void SetupDialogueTrigger(GameObject obj)
    {
        DialogueSystemTrigger trigger = obj.AddComponent<DialogueSystemTrigger>();
        trigger.conversation = conversationTitle;
        trigger.trigger = DialogueSystemTriggerEvent.OnUse;
        // Note: actor property doesn't exist in DialogueSystemTrigger
        // The actor is determined by the conversation setup
        
        Debug.Log("Dialogue trigger configured for: " + conversationTitle);
    }
    
    void SetupCollider(GameObject obj)
    {
        // Add capsule collider for better interaction
        CapsuleCollider capsule = obj.AddComponent<CapsuleCollider>();
        capsule.radius = 0.3f;
        capsule.height = 1.8f;
        capsule.center = new Vector3(0, 0.9f, 0);
        capsule.isTrigger = false;
        
        Debug.Log("Collider added for interaction");
    }
    
    void SetupVisuals(GameObject obj)
    {
        // Add visual representation
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        indicator.transform.SetParent(obj.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.6f, 1.8f, 0.6f);
        
        Renderer renderer = indicator.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = dialogueColor;
        
        Debug.Log("Visual representation added");
    }
    
    [ContextMenu("Test NPC Interaction")]
    public void TestNPCInteraction()
    {
        // Find the NPC we created
        GameObject npc = GameObject.Find(npcName);
        if (npc != null)
        {
            Debug.Log("NPC found: " + npc.name);
            
            // Check components
            NPCController npcController = npc.GetComponent<NPCController>();
            Usable usable = npc.GetComponent<Usable>();
            DialogueSystemTrigger trigger = npc.GetComponent<DialogueSystemTrigger>();
            Collider collider = npc.GetComponent<Collider>();
            
            Debug.Log("Components check:");
            Debug.Log("- NPCController: " + (npcController != null));
            Debug.Log("- Usable: " + (usable != null));
            Debug.Log("- DialogueSystemTrigger: " + (trigger != null));
            Debug.Log("- Collider: " + (collider != null));
            
            if (npcController != null)
            {
                Debug.Log("Conversation set to: " + npcController.conversationEntry);
            }
        }
        else
        {
            Debug.LogError("NPC not found: " + npcName);
        }
    }
    
    [ContextMenu("Test Conversation Directly")]
    public void TestConversationDirectly()
    {
        if (DialogueManager.hasInstance)
        {
            var conversation = DialogueManager.masterDatabase.GetConversation(conversationTitle);
            if (conversation != null)
            {
                DialogueManager.StartConversation(conversationTitle);
                Debug.Log("Started conversation directly: " + conversationTitle);
            }
            else
            {
                Debug.LogError("Conversation not found: " + conversationTitle);
                Debug.Log("Available conversations:");
                foreach (var conv in DialogueManager.masterDatabase.conversations)
                {
                    Debug.Log("- " + conv.Title);
                }
            }
        }
        else
        {
            Debug.LogError("Dialogue Manager not available");
        }
    }
}