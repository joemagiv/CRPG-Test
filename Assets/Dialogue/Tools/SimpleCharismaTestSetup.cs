using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Simple script to set up the Charisma test conversation
/// </summary>
public class SimpleCharismaTestSetup : MonoBehaviour
{
    void Start()
    {
        // This script provides a simple way to test the conversation
        // without needing to import JSON files
        
        Debug.Log("=== Charisma Test Conversation Setup ===");
        Debug.Log("To use this conversation:");
        Debug.Log("1. Create it manually in the Dialogue Database Editor");
        Debug.Log("2. Or use the CreateCharismaTestConversation script");
        Debug.Log("3. Then attach this to an NPC or use the test method");
    }
    
    /// <summary>
    /// Manually start the Charisma test conversation
    /// </summary>
    public void StartCharismaTest()
    {
        if (DialogueManager.hasInstance)
        {
            // Check if conversation exists
            var conversation = DialogueManager.masterDatabase.GetConversation("CharismaTest");
            if (conversation != null)
            {
                DialogueManager.StartConversation("CharismaTest");
                Debug.Log("Started CharismaTest conversation");
            }
            else
            {
                Debug.LogError("CharismaTest conversation not found. Please create it first.");
                Debug.Log("Use the CreateCharismaTestConversation script to create it automatically.");
            }
        }
        else
        {
            Debug.LogError("Dialogue Manager not available");
        }
    }
    
    /// <summary>
    /// Create a test NPC that uses this conversation
    /// </summary>
    public void SetupTestNPC()
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