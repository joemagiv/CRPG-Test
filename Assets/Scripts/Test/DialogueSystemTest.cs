using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Test script to demonstrate the dialogue system functionality
/// </summary>
public class DialogueSystemTest : MonoBehaviour
{
    [Header("Scene References")]
    public GameObject player;
    public RightPanelDialogueUI dialogueUI;
    public DialogueDemoSetup dialogueDemo;
    
    [Header("Test NPCs")]
    public NPCController testNPC1;
    public NPCController testNPC2;
    
    void Start()
    {
        SetupTestEnvironment();
    }
    
    void SetupTestEnvironment()
    {
        // Ensure Dialogue Manager exists
        EnsureDialogueManager();
        
        // Set up player with stats
        SetupPlayer();
        
        // Set up test NPCs
        SetupTestNPCs();
        
        // Set up dialogue demo
        if (dialogueDemo != null)
        {
            dialogueDemo.enabled = true;
        }
        
        Debug.Log("Dialogue System Test Environment Ready!");
        Debug.Log("- Player has CharacterStats component");
        Debug.Log("- Test NPCs configured with different conversations");
        Debug.Log("- Disco Elysium style UI active");
        Debug.Log("- Approach NPCs and press 'E' to interact");
    }
    
    void EnsureDialogueManager()
    {
        if (DialogueManager.instance == null)
        {
            // Instantiate Dialogue Manager prefab
            GameObject managerPrefab = Resources.Load<GameObject>("Dialogue Manager");
            if (managerPrefab != null)
            {
                Instantiate(managerPrefab);
                Debug.Log("Dialogue Manager instantiated");
            }
            else
            {
                Debug.LogError("Dialogue Manager prefab not found in Resources");
            }
        }
    }
    
    void SetupPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            // Add CharacterStats if not present
            var stats = player.GetComponent<CharacterStats>();
            if (stats == null)
            {
                stats = player.AddComponent<CharacterStats>();
                Debug.Log("Added CharacterStats to player");
            }
            
            // Add StatBasedDialogueConditions
            var statConditions = player.GetComponent<StatBasedDialogueConditions>();
            if (statConditions == null)
            {
                statConditions = player.AddComponent<StatBasedDialogueConditions>();
                Debug.Log("Added StatBasedDialogueConditions to player");
            }
            
            // Add Selector component for NPC interaction
            var selector = player.GetComponent<Selector>();
            if (selector == null)
            {
                selector = player.AddComponent<Selector>();
                selector.selectAt = Selector.SelectAt.MousePosition;
                selector.useKey = KeyCode.E;
                Debug.Log("Added Selector to player for NPC interaction");
            }
        }
    }
    
    void SetupTestNPCs()
    {
        if (testNPC1 != null)
        {
            testNPC1.SetNPCName("Test NPC 1");
            testNPC1.SetDialogueColor(Color.cyan);
            testNPC1.SetConversation("TestConversation1");
        }
        
        if (testNPC2 != null)
        {
            testNPC2.SetNPCName("Test NPC 2");
            testNPC2.SetDialogueColor(Color.magenta);
            testNPC2.SetConversation("TestConversation2");
        }
    }
    
    void Update()
    {
        // Simple test: Press F1 to start a test conversation
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartTestConversation();
        }
        
        // Press F2 to test stat roll
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestStatRoll();
        }
    }
    
    void StartTestConversation()
    {
        if (DialogueManager.instance != null)
        {
            // This would normally be triggered by interacting with an NPC
            DialogueManager.StartConversation("TestConversation1");
            Debug.Log("Started test conversation");
        }
        else
        {
            Debug.LogError("Dialogue Manager not available");
        }
    }
    
    void TestStatRoll()
    {
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                int strengthRoll = stats.RollStat("Strength");
                bool success = stats.CheckStatRoll("Strength", 15);
                
                Debug.Log("Strength Roll Test: " + strengthRoll + " (vs DC 15) - " + (success ? "SUCCESS" : "FAIL"));
            }
        }
    }
}