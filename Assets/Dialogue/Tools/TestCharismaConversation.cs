using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TestCharismaConversation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestConversation();
        }
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TestStatRolls();
        }
    }
    
    void TestConversation()
    {
        Debug.Log("=== Testing Charisma Conversation ===");
        
        if (!DialogueManager.hasInstance)
        {
            Debug.LogError("Dialogue Manager not available");
            return;
        }
        
        // Check if conversation exists
        var conversation = DialogueManager.masterDatabase.GetConversation("CharismaTest");
        if (conversation == null)
        {
            Debug.LogError("CharismaTest conversation not found");
            return;
        }
        
        Debug.Log("Found conversation: " + conversation.Title);
        Debug.Log("Conversation ID: " + conversation.id);
            Debug.Log("Number of entries: " + (conversation.dialogueEntries != null ? conversation.dialogueEntries.Count : 0));
        
        // Start the conversation
        DialogueManager.StartConversation("CharismaTest");
        Debug.Log("Started CharismaTest conversation");
    }
    
    void TestStatRolls()
    {
        Debug.Log("=== Testing Stat Rolls ===");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
            return;
        }
        
        CharacterStats stats = player.GetComponent<CharacterStats>();
        if (stats == null)
        {
            Debug.LogError("CharacterStats component not found on player");
            return;
        }
        
        // Test Charisma roll
        int charisma = stats.GetStatValue("Charisma");
        int roll = stats.RollStat("Charisma");
        bool success = stats.CheckStatRoll("Charisma", 15);
        
        Debug.Log("Charisma stat: " + charisma);
        Debug.Log("Charisma roll: " + roll);
        Debug.Log("Roll vs DC 15: " + (success ? "SUCCESS" : "FAIL"));
        
        // Test other stats
        string[] testStats = {"Strength", "Intelligence", "Dexterity"};
        foreach (string statName in testStats)
        {
            int value = stats.GetStatValue(statName);
            Debug.Log(statName + ": " + value);
        }
    }
}