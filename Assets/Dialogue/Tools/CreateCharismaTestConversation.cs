using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Simplified script to help create the Charisma test conversation
/// </summary>
public class CreateCharismaTestConversation : MonoBehaviour
{
    /// <summary>
    /// This method provides instructions for manually creating the conversation
    /// since the Dialogue System API for programmatic creation is complex
    /// </summary>
    [ContextMenu("Show Creation Instructions")]
    public void ShowCreationInstructions()
    {
        Debug.Log("=== Charisma Test Conversation Creation ===");
        Debug.Log("Since the Dialogue System API is complex, please use the manual creation guide:");
        Debug.Log("1. Open Assets/Dialogue/ManualCreationGuide.md");
        Debug.Log("2. Follow the step-by-step instructions");
        Debug.Log("3. Or use the SimpleCharismaTestSetup.cs script for basic setup");
    }
    
    /// <summary>
    /// Test if the conversation exists and can be started
    /// </summary>
    [ContextMenu("Test Charisma Conversation")]
    public void TestConversation()
    {
        if (!DialogueManager.hasInstance)
        {
            Debug.LogError("Dialogue Manager not available");
            return;
        }
        
        // Check if conversation exists
        var conversation = DialogueManager.masterDatabase.GetConversation("CharismaTest");
        if (conversation != null)
        {
            DialogueManager.StartConversation("CharismaTest");
            Debug.Log("Started CharismaTest conversation");
        }
        else
        {
            Debug.LogError("CharismaTest conversation not found. Please create it first using the manual guide.");
        }
    }
    
    /// <summary>
    /// Create a test NPC that uses this conversation
    /// </summary>
    [ContextMenu("Create Test NPC")]
    public void CreateTestNPC()
    {
        GameObject npcObj = new GameObject("Mysterious Merchant");
        npcObj.transform.position = new Vector3(0, 0, 3);
        
        NPCController npc = npcObj.AddComponent<NPCController>();
        npc.SetNPCName("Mysterious Merchant");
        npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f));
        npc.SetConversation("CharismaTest");
        
        // Add interaction components
        Usable usable = npcObj.AddComponent<Usable>();
        usable.overrideName = "Mysterious Merchant";
        usable.overrideUseMessage = "Talk";
        
        DialogueSystemTrigger trigger = npcObj.AddComponent<DialogueSystemTrigger>();
        trigger.conversation = "CharismaTest";
        trigger.trigger = DialogueSystemTriggerEvent.OnUse;
        
        // Add visual representation
        AddVisualRepresentation(npcObj);
        
        Debug.Log("Created test NPC. Approach and press E to interact.");
        Debug.Log("Note: You must first create the CharismaTest conversation manually.");
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
        renderer.material.color = new Color(0.6f, 0.4f, 0.8f);
    }
}