using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Final setup script for the complete dialogue system
/// </summary>
public class DialogueSystemFinalSetup : MonoBehaviour
{
    [Header("Setup Options")]
    public bool setupPlayer = true;
    public bool createTestNPC = true;
    public bool showInstructions = true;
    
    void Start()
    {
        if (setupPlayer)
        {
            SetupPlayer();
        }
        
        if (createTestNPC)
        {
            CreateTestNPC();
        }
        
        if (showInstructions)
        {
            ShowSetupInstructions();
        }
        ConfigureCustomUI();
    }

    void ConfigureCustomUI()
{
    if (DialogueManager.hasInstance)
    {
        // Set our custom UI as the active dialogue UI
        var customUI = FindObjectOfType<RightPanelDialogueUI>();
        if (customUI != null)
        {
            if (customUI is IDialogueUI dialogueUIInterface)
            {
                DialogueManager.dialogueUI = dialogueUIInterface;
            }
            Debug.Log("Custom dialogue UI assigned to Dialogue Manager");
        }
        // Disable standard UI selector if not needed
        var standardSelector = FindObjectOfType<SelectorUseStandardUIElements>();
        if (standardSelector != null)
        {
            standardSelector.enabled = false;
            Debug.Log("Disabled standard UI selector elements");
        }
    }
}
    
    void SetupPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            player = new GameObject("Player");
            player.tag = "Player";
        }
        
        // Add essential components
        player.AddComponent<CharacterStats>();
        player.AddComponent<StatBasedDialogueConditions>();
        
        // Add selector for NPC interaction
        Selector selector = player.AddComponent<Selector>();
        selector.selectAt = Selector.SelectAt.MousePosition;
        selector.useKey = KeyCode.E;
        
        Debug.Log("Player setup complete");
    }
    
    void CreateTestNPC()
    {
        GameObject npcObj = new GameObject("Mysterious Merchant");
        npcObj.transform.position = new Vector3(0, 0, 3);
        
        // Add NPC Controller
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
        CapsuleCollider capsule = npcObj.AddComponent<CapsuleCollider>();
        capsule.radius = 0.3f;
        capsule.height = 1.8f;
        capsule.center = new Vector3(0, 0.9f, 0);
        
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        indicator.transform.SetParent(npcObj.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.6f, 1.8f, 0.6f);
        indicator.GetComponent<Renderer>().material.color = new Color(0.6f, 0.4f, 0.8f);
        
        Debug.Log("Test NPC created - approach and press E to interact");
    }
    
    void ShowSetupInstructions()
    {
        Debug.Log("=== DIALOGUE SYSTEM SETUP COMPLETE ===");
        Debug.Log("");
        Debug.Log("WHAT'S BEEN SET UP:");
        Debug.Log("- Player with CharacterStats and interaction components");
        Debug.Log("- Test NPC (Mysterious Merchant) with CharismaTest conversation");
        Debug.Log("- Disco Elysium-style dialogue UI");
        Debug.Log("");
        Debug.Log("WHAT YOU NEED TO DO:");
        Debug.Log("1. Create the 'CharismaTest' conversation manually");
        Debug.Log("   (See Assets/Dialogue/ManualCreationGuide.md)");
        Debug.Log("2. Approach the Mysterious Merchant (purple capsule)");
        Debug.Log("3. Press E to interact and test the conversation");
        Debug.Log("4. Use number keys (1-9) to select responses");
        Debug.Log("");
        Debug.Log("EXPECTED BEHAVIOR:");
        Debug.Log("- NPC greets you and offers a task");
        Debug.Log("- One response requires Charisma 15+ check");
        Debug.Log("- Success: +1 Charisma, positive response");
        Debug.Log("- Failure: -1 Reputation, skeptical response");
        Debug.Log("");
        Debug.Log("TROUBLESHOOTING:");
        Debug.Log("- If conversation not found: Create it manually first");
        Debug.Log("- If stats not working: Check CharacterStats component");
        Debug.Log("- If UI not showing: Check RightPanelDialogueUI setup");
    }
    
    [ContextMenu("Test Charisma Conversation")]
    public void TestConversation()
    {
        if (DialogueManager.hasInstance)
        {
            DialogueManager.StartConversation("CharismaTest");
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
        Debug.Log("File: Assets/Dialogue/ManualCreationGuide.md");
        Debug.Log("");
        Debug.Log("STEPS:");
        Debug.Log("1. Open Dialogue Database Editor");
        Debug.Log("2. Create Actors tab:");
        Debug.Log("   - NPC: Name='NPC', Display='Mysterious Merchant'");
        Debug.Log("   - Player: Name='Player', Display='Player'");
        Debug.Log("3. Create Conversation 'CharismaTest'");
        Debug.Log("4. Add dialogue entries (detailed in guide)");
        Debug.Log("5. Save database");
    }
}