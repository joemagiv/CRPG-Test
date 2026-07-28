using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Quick template to set up the Charisma test conversation
/// </summary>
public class CharismaTestTemplate : MonoBehaviour
{
    [Header("NPC Setup")]
    public GameObject npcPrefab;
    public Vector3 npcPosition = new Vector3(0, 0, 3);
    
    [Header("Conversation Setup")]
    public string conversationTitle = "CharismaTest";
    
    void Start()
    {
        SetupNPC();
        Debug.Log("Charisma test template ready!");
        Debug.Log("- NPC created and positioned");
        Debug.Log("- Approach the NPC and press E to interact");
        Debug.Log("- You must create the conversation manually first (see ManualCreationGuide.md)");
    }
    
    [ContextMenu("Setup NPC")]
    public void SetupNPC()
    {
        if (npcPrefab == null)
        {
            // Create a basic NPC if no prefab assigned
            GameObject npcObj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npcObj.name = "Mysterious Merchant";
            npcObj.transform.position = npcPosition;
            
            // Remove collider from primitive
            Destroy(npcObj.GetComponent<Collider>());
            
            // Add NPC components
            NPCController npc = npcObj.AddComponent<NPCController>();
            npc.SetNPCName("Mysterious Merchant");
            npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f));
            npc.SetConversation(conversationTitle);
            
            // Add interaction components
            Usable usable = npcObj.AddComponent<Usable>();
            usable.overrideName = "Mysterious Merchant";
            usable.overrideUseMessage = "Talk";
            usable.maxUseDistance = 5f;
            
            DialogueSystemTrigger trigger = npcObj.AddComponent<DialogueSystemTrigger>();
            trigger.conversation = conversationTitle;
            trigger.trigger = DialogueSystemTriggerEvent.OnUse;
            
            // Add proper collider
            CapsuleCollider capsule = npcObj.AddComponent<CapsuleCollider>();
            capsule.radius = 0.3f;
            capsule.height = 1.8f;
            capsule.center = new Vector3(0, 0.9f, 0);
            
            Debug.Log("Created NPC from primitive");
        }
        else
        {
            // Instantiate the prefab
            GameObject npcObj = Instantiate(npcPrefab, npcPosition, Quaternion.identity);
            npcObj.name = "Mysterious Merchant";
            
            // Configure the NPC
            NPCController npc = npcObj.GetComponent<NPCController>() ?? npcObj.AddComponent<NPCController>();
            npc.SetNPCName("Mysterious Merchant");
            npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f));
            npc.SetConversation(conversationTitle);
            
            Debug.Log("Instantiated NPC from prefab");
        }
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
                Debug.LogError("Conversation '" + conversationTitle + "' not found. Please create it manually first.");
                Debug.Log("See Assets/Dialogue/ManualCreationGuide.md for instructions.");
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
        Debug.Log("1. Open Dialogue Database Editor: Window > Pixel Crushers > Dialogue System > Dialogue Database Editor");
        Debug.Log("2. Go to Actors tab and create:");
        Debug.Log("   - NPC: Name='NPC', Display='Mysterious Merchant', Color=#9966CC");
        Debug.Log("   - Player: Name='Player', Display='Player', Color=#FFFFFF");
        Debug.Log("3. Go to Conversations tab and create conversation 'CharismaTest'");
        Debug.Log("4. Add dialogue entries following the structure in ManualCreationGuide.md");
        Debug.Log("5. Save the database");
        Debug.Log("6. Click 'Test Conversation' button above to test");
    }
}