using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Complete test script demonstrating all dialogue system functionality
/// </summary>
public class CompleteDialogueSystemTest : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool spawnTestNPCs = true;
    public bool showDebugInfo = true;
    
    [Header("References")]
    public GameObject player;
    public Transform[] npcSpawnPoints;
    
    private DialogueSystemIntegration integration;
    
    void Start()
    {
        SetupTestEnvironment();
    }
    
    void SetupTestEnvironment()
    {
        // Create or get integration component
        integration = FindObjectOfType<DialogueSystemIntegration>();
        if (integration == null)
        {
            integration = gameObject.AddComponent<DialogueSystemIntegration>();
        }
        
        // Configure integration
        integration.player = player;
        integration.npcSpawnPoints = npcSpawnPoints;
        
        // Create test NPCs if enabled
        if (spawnTestNPCs)
        {
            CreateTestNPCs();
        }
        
        // Set up player
        SetupTestPlayer();
        
        if (showDebugInfo)
        {
            ShowTestInstructions();
        }
    }
    
    void CreateTestNPCs()
    {
        if (integration == null) return;
        
        // Create NPC spawn points if none provided
        if (npcSpawnPoints == null || npcSpawnPoints.Length == 0)
        {
            CreateDefaultSpawnPoints();
        }
        
        // Create friendly NPC
        GameObject friendlyNPC = integration.CreateBasicNPC(
            "Friendly Villager", 
            new Color(0.4f, 0.8f, 0.4f), 
            "FriendlyGreeting"
        );
        
        if (npcSpawnPoints.Length > 0)
        {
            friendlyNPC.transform.position = npcSpawnPoints[0].position;
        }
        
        // Create mysterious NPC
        GameObject mysteriousNPC = integration.CreateBasicNPC(
            "Mysterious Stranger", 
            new Color(0.6f, 0.4f, 0.8f), 
            "MysteriousEncounter"
        );
        
        if (npcSpawnPoints.Length > 1)
        {
            mysteriousNPC.transform.position = npcSpawnPoints[1].position;
        }
        
        // Create guard NPC
        GameObject guardNPC = integration.CreateBasicNPC(
            "City Guard", 
            new Color(0.8f, 0.6f, 0.2f), 
            "GuardChallenge"
        );
        
        if (npcSpawnPoints.Length > 2)
        {
            guardNPC.transform.position = npcSpawnPoints[2].position;
        }
        
        Debug.Log("Created test NPCs:");
        Debug.Log("- Friendly Villager (Green) - FriendlyGreeting");
        Debug.Log("- Mysterious Stranger (Purple) - MysteriousEncounter");
        Debug.Log("- City Guard (Orange) - GuardChallenge");
    }
    
    void CreateDefaultSpawnPoints()
    {
        // Create spawn points around the player
        npcSpawnPoints = new Transform[3];
        
        for (int i = 0; i < 3; i++)
        {
            GameObject spawnPoint = new GameObject("NPC_SpawnPoint_" + (i + 1));
            spawnPoint.transform.position = player.transform.position + new Vector3(
                (i - 1) * 3f, 
                0, 
                -3f - i * 2f
            );
            npcSpawnPoints[i] = spawnPoint.transform;
        }
    }
    
    void SetupTestPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            // Add or configure CharacterStats
            var stats = player.GetComponent<CharacterStats>();
            if (stats == null)
            {
                stats = player.AddComponent<CharacterStats>();
            }
            
            // Customize stats for testing
            if (stats != null)
            {
                // Add some specific stats for our test conversations
                stats.stats.Clear();
                stats.stats.Add(new CharacterStats.Stat("Strength", 14));
                stats.stats.Add(new CharacterStats.Stat("Dexterity", 12));
                stats.stats.Add(new CharacterStats.Stat("Intelligence", 10));
                stats.stats.Add(new CharacterStats.Stat("Charisma", 16));
                stats.stats.Add(new CharacterStats.Stat("Perception", 11));
                stats.stats.Add(new CharacterStats.Stat("Intimidation", 8));
                stats.stats.Add(new CharacterStats.Stat("Persuasion", 14));
                stats.stats.Add(new CharacterStats.Stat("Stealth", 9));
                
                Debug.Log("Player stats configured for testing:");
                foreach (var stat in stats.stats)
                {
                    Debug.Log("- " + stat.statName + ": " + stat.currentValue);
                }
            }
            
            // Add StatBasedDialogueConditions
            var statConditions = player.GetComponent<StatBasedDialogueConditions>();
            if (statConditions == null)
            {
                statConditions = player.AddComponent<StatBasedDialogueConditions>();
            }
            
            // Add Selector for NPC interaction
            var selector = player.GetComponent<Selector>();
            if (selector == null)
            {
                selector = player.AddComponent<Selector>();
                selector.selectAt = Selector.SelectAt.MousePosition;
                selector.useKey = KeyCode.E;
            }
        }
    }
    
    void ShowTestInstructions()
    {
        Debug.Log("=== DIALOGUE SYSTEM TEST INSTRUCTIONS ===");
        Debug.Log("1. Approach any NPC (green capsule) to interact");
        Debug.Log("2. Press 'E' or click to start conversation");
        Debug.Log("3. Use number keys (1-9) to select dialogue options");
        Debug.Log("4. Different NPCs have different colored text:");
        Debug.Log("   - Friendly Villager: Green");
        Debug.Log("   - Mysterious Stranger: Purple");
        Debug.Log("   - City Guard: Orange");
        Debug.Log("5. Some dialogue options require stat rolls");
        Debug.Log("6. Press F1 to test a conversation manually");
        Debug.Log("7. Press F2 to test stat rolls");
        Debug.Log("==========================================");
    }
    
    void Update()
    {
        HandleTestInput();
    }
    
    void HandleTestInput()
    {
        // Test conversation
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestConversation();
        }
        
        // Test stat rolls
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TestAllStats();
        }
        
        // Test specific stat roll
        if (Input.GetKeyDown(KeyCode.F3))
        {
            TestSpecificStatRoll();
        }
    }
    
    void TestConversation()
    {
        if (DialogueManager.hasInstance)
        {
            // Try to start a test conversation
            DialogueManager.StartConversation("FriendlyGreeting");
            Debug.Log("Attempting to start FriendlyGreeting conversation");
        }
        else
        {
            Debug.LogError("Dialogue Manager not available");
        }
    }
    
    void TestAllStats()
    {
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                Debug.Log("=== TESTING ALL STAT ROLLS ===");
                
                foreach (var stat in stats.stats)
                {
                    int roll = stats.RollStat(stat.statName);
                    bool success = stats.CheckStatRoll(stat.statName, 15);
                    Debug.Log(stat.statName + ": " + roll + " vs DC 15 - " + (success ? "SUCCESS" : "FAIL"));
                }
                
                Debug.Log("===============================");
            }
        }
    }
    
    void TestSpecificStatRoll()
    {
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                // Test a challenging roll
                string statName = "Persuasion";
                int difficulty = 18;
                
                int roll = stats.RollStat(statName);
                bool success = stats.CheckStatRoll(statName, difficulty);
                
                Debug.Log("=== SPECIAL STAT ROLL TEST ===");
                Debug.Log(statName + " Roll: " + roll + " vs DC " + difficulty);
                Debug.Log("Result: " + (success ? "SUCCESS!" : "FAILURE!"));
                Debug.Log("===============================");
                
            // Show in dialogue UI if available
            var dialogueUI = Object.FindAnyObjectByType<RightPanelDialogueUI>();
                if (dialogueUI != null && dialogueUI.dialogueText != null)
                {
                    dialogueUI.dialogueText.text = "[" + statName + " Roll: " + roll + " vs DC " + difficulty + " - " + (success ? "SUCCESS!" : "FAILURE!") + "]";
                }
            }
        }
    }
    
    /// <summary>
    /// Example of how to create a conversation with stat checks using Lua
    /// This would be used in the Dialogue System's conversation editor
    /// </summary>
    public void ExampleStatBasedConversationSetup()
    {
        /*
        In the Dialogue System Editor, you would create a conversation like this:
        
        Title: GuardChallenge
        
        Guard: Halt! State your business. [Serious tone]
        
        Player Options:
        1. "I'm just passing through." (No stat check)
           -> Guard: Very well, move along.
           -> [End conversation]
        
        2. "Step aside, I have important business!" (Intimidation check)
           Condition: CheckStatRoll("Intimidation", 15)
           -> Guard: *nervously* Of course, sir! Right this way!
           -> [End conversation - success]
           
           Else:
           -> Guard: *laughs* Nice try. Not happening.
           -> [Add temporary -2 to Intimidation]
           -> [End conversation - failure]
        
        3. "I have information about the smugglers." (Persuasion check)
           Condition: CheckStatRoll("Persuasion", 12)
           -> Guard: Oh? Tell me more...
           -> [Continue to information branch]
           
           Else:
           -> Guard: Yeah, right. I've heard that one before.
           -> [End conversation]
        
        4. *Attempt to sneak past* (Stealth check - hidden option)
           Condition: CheckStatRoll("Stealth", 18)
           -> [Silently move past guard without being noticed]
           -> [End conversation - success]
           
           Else:
           -> Guard: Hey! Stop right there!
           -> [Trigger combat or arrest sequence]
        */
    }
}