using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Main integration script for the NPC and Dialogue System
/// Handles setup and provides utility functions
/// </summary>
public class DialogueSystemIntegration : MonoBehaviour
{
    [Header("System Components")]
    public GameObject player;
    public RightPanelDialogueUI dialogueUI;
    public DialogueSystemTest testSystem;
    
    [Header("NPC Prefabs")]
    public GameObject friendlyNPCPrefab;
    public GameObject mysteriousNPCPrefab;
    public GameObject guardNPCPrefab;
    
    [Header("Spawn Points")]
    public Transform[] npcSpawnPoints;
    
    void Awake()
    {
        InitializeSystems();
    }
    
    void InitializeSystems()
    {
        // Ensure player is set up
        SetupPlayer();
        
        // Initialize dialogue UI
        InitializeDialogueUI();
        
        // Spawn test NPCs
        SpawnTestNPCs();
        
        // Initialize test system
        if (testSystem != null)
        {
            testSystem.enabled = true;
        }
        
        Debug.Log("Dialogue System Integration Complete!");
    }
    
    void SetupPlayer()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (player != null)
        {
            // Add essential components if missing
            EnsureComponent<CharacterStats>(player, "CharacterStats");
            EnsureComponent<StatBasedDialogueConditions>(player, "StatBasedDialogueConditions");
            EnsureComponent<Selector>(player, "Selector");
            
            // Configure selector
            var selector = player.GetComponent<Selector>();
            if (selector != null)
            {
                // Configure targeting method (CenterOfScreen, MousePosition, or CustomPosition)
                selector.selectAt = Selector.SelectAt.MousePosition;
                
                // Configure input method
                selector.useKey = KeyCode.E; // Common interaction key
                selector.useButton = "Fire2"; // Controller button
            }
        }
    }
    
    void EnsureComponent<T>(GameObject obj, string componentName) where T : Component
    {
        var component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
            Debug.Log("Added " + componentName + " to " + obj.name);
        }
    }
    
    void InitializeDialogueUI()
    {
        if (dialogueUI == null)
        {
            // Try to find existing UI
            dialogueUI = Object.FindAnyObjectByType<RightPanelDialogueUI>();
        }
        
        if (dialogueUI == null)
        {
            // Create UI if none exists
            CreateDefaultDialogueUI();
        }
        
        // Ensure UI is properly configured
        if (dialogueUI != null)
        {
            dialogueUI.enabled = true;
        }
    }
    
    void CreateDefaultDialogueUI()
    {
        // Create UI using the prefab creator
        var creator = gameObject.AddComponent<RightPanelDialogueUIPrefabCreator>();
        GameObject uiObj = creator.CreateDialogueUI();
        dialogueUI = uiObj.GetComponent<RightPanelDialogueUI>();
        
        // Make it persistent
        DontDestroyOnLoad(uiObj);
        Debug.Log("Created default right panel dialogue UI");
    }
    
    void SpawnTestNPCs()
    {
        if (npcSpawnPoints == null || npcSpawnPoints.Length == 0) return;
        
        // Spawn friendly NPC
        if (friendlyNPCPrefab != null && npcSpawnPoints.Length > 0)
        {
            SpawnNPC(friendlyNPCPrefab, npcSpawnPoints[0], "Friendly Villager", new Color(0.4f, 0.8f, 0.4f), "FriendlyGreeting");
        }
        
        // Spawn mysterious NPC
        if (mysteriousNPCPrefab != null && npcSpawnPoints.Length > 1)
        {
            SpawnNPC(mysteriousNPCPrefab, npcSpawnPoints[1], "Mysterious Stranger", new Color(0.6f, 0.4f, 0.8f), "MysteriousEncounter");
        }
        
        // Spawn guard NPC
        if (guardNPCPrefab != null && npcSpawnPoints.Length > 2)
        {
            SpawnNPC(guardNPCPrefab, npcSpawnPoints[2], "City Guard", new Color(0.8f, 0.6f, 0.2f), "GuardChallenge");
        }
    }
    
    void SpawnNPC(GameObject prefab, Transform spawnPoint, string npcName, Color dialogueColor, string conversation)
    {
        GameObject npcObj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        var npcController = npcObj.GetComponent<NPCController>();
        
        if (npcController != null)
        {
            npcController.SetNPCName(npcName);
            npcController.SetDialogueColor(dialogueColor);
            npcController.SetConversation(conversation);
        }
        
        Debug.Log("Spawned " + npcName + " at " + spawnPoint.name);
    }
    
    /// <summary>
    /// Create a basic NPC GameObject with dialogue components
    /// </summary>
    public GameObject CreateBasicNPC(string name, Color color, string conversationTitle)
    {
        GameObject npcObj = new GameObject(name + "_NPC");
        
        // Add NPC Controller
        NPCController npcController = npcObj.AddComponent<NPCController>();
        npcController.SetNPCName(name);
        npcController.SetDialogueColor(color);
        npcController.SetConversation(conversationTitle);
        
        // Add visual representation (simple capsule for testing)
        CapsuleCollider capsule = npcObj.AddComponent<CapsuleCollider>();
        capsule.radius = 0.3f;
        capsule.height = 1.8f;
        capsule.center = new Vector3(0, 0.9f, 0);
        
        // Add rigidbody for physics
        Rigidbody rb = npcObj.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        // Add a simple visual indicator
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        indicator.transform.SetParent(npcObj.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.6f, 1.8f, 0.6f);
        
        Renderer renderer = indicator.GetComponent<Renderer>();
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.material.color = color;
        
        // Set layer to NPC or similar
        npcObj.layer = LayerMask.NameToLayer("NPC");
        
        return npcObj;
    }
    
    /// <summary>
    /// Utility function to start a conversation with an NPC
    /// </summary>
    public void StartConversationWithNPC(NPCController npc)
    {
        if (npc != null && DialogueManager.hasInstance)
        {
            // Note: CurrentActor is read-only, actor is determined by the conversation setup
            DialogueManager.StartConversation(npc.conversationEntry);
        }
    }
    
    /// <summary>
    /// Test stat roll function
    /// </summary>
    public void TestStatRoll(string statName, int difficulty)
    {
        if (player != null)
        {
            var stats = player.GetComponent<CharacterStats>();
            if (stats != null)
            {
                int roll = stats.RollStat(statName);
                bool success = stats.CheckStatRoll(statName, difficulty);
                
                Debug.Log(statName + " Roll: " + roll + " vs DC " + difficulty + " - " + (success ? "SUCCESS" : "FAIL"));
                
                // You could also show this in UI
                if (dialogueUI != null && dialogueUI.dialogueText != null)
                {
                    dialogueUI.dialogueText.text = "[" + statName + " Roll: " + roll + " vs DC " + difficulty + " - " + (success ? "SUCCESS" : "FAIL") + "]";
                }
            }
        }
    }
}