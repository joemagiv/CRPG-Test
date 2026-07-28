using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Sets up demonstration dialogue conversations
/// </summary>
public class DialogueDemoSetup : MonoBehaviour
{
    [Header("NPC References")]
    public NPCController friendlyNPC;
    public NPCController mysteriousNPC;
    public NPCController guardNPC;
    
    [Header("Conversation Titles")]
    public string friendlyConversation = "FriendlyGreeting";
    public string mysteriousConversation = "MysteriousEncounter";
    public string guardConversation = "GuardChallenge";
    
    void Start()
    {
        SetupConversations();
        SetupNPCs();
    }
    
    void SetupConversations()
    {
        // This would normally be done in the Dialogue System Editor
        // For demonstration, we'll show how to set up conversations programmatically
        
        // Note: In a real project, you would use the Dialogue System Editor to create
        // these conversations visually. This script shows the conceptual setup.
        
        Debug.Log("Dialogue Demo Setup Complete");
        Debug.Log("- Friendly NPC conversation: " + friendlyConversation);
        Debug.Log("- Mysterious NPC conversation: " + mysteriousConversation);
        Debug.Log("- Guard NPC conversation: " + guardConversation);
    }
    
    void SetupNPCs()
    {
        if (friendlyNPC != null)
        {
            friendlyNPC.SetNPCName("Friendly Villager");
            friendlyNPC.SetDialogueColor(new Color(0.4f, 0.8f, 0.4f)); // Green
            friendlyNPC.SetConversation(friendlyConversation);
        }
        
        if (mysteriousNPC != null)
        {
            mysteriousNPC.SetNPCName("Mysterious Stranger");
            mysteriousNPC.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f)); // Purple
            mysteriousNPC.SetConversation(mysteriousConversation);
        }
        
        if (guardNPC != null)
        {
            guardNPC.SetNPCName("City Guard");
            guardNPC.SetDialogueColor(new Color(0.8f, 0.6f, 0.2f)); // Orange
            guardNPC.SetConversation(guardConversation);
        }
    }
    
    /// <summary>
    /// Example of how to create a conversation programmatically
    /// Note: This is for demonstration. Normally you'd use the Dialogue System Editor.
    /// </summary>
    public void CreateExampleConversation()
    {
        // This shows the conceptual structure of a conversation
        // In practice, you would create this in the Dialogue System Editor UI
        
        /*
        Conversation example:
        
        Title: FriendlyGreeting
        
        NPC: Hello there, traveler! [Friendly]
        Player: (1) Hello! Nice to meet you.
               (2) What's new around here?
               (3) I have to go. [Requires Dexterity 12]
        
        If Player chooses 1:
        NPC: Nice to meet you too! I'm just tending to my garden.
        Player: (1) What are you growing?
               (2) Gardening seems relaxing.
               (3) Goodbye.
        
        If Player chooses 2:
        NPC: Oh, not much. Just the usual village gossip.
        Player: (1) Any interesting gossip?
               (2) Thanks for the info.
        
        If Player chooses 3 and passes Dexterity check:
        NPC: Oh, okay. Have a safe journey!
        [End conversation]
        
        If Player chooses 3 and fails Dexterity check:
        NPC: Wait, don't rush off! Let's chat a bit more.
        [Go to "Nice to meet you too" node]
        */
    }
    
    /// <summary>
    /// Example conversation with stat-based rolls
    /// </summary>
    public void CreateStatBasedConversation()
    {
        /*
        Conversation example: GuardChallenge
        
        Guard: Halt! What's your business here? [Serious]
        Player: (1) I'm just passing through. [No check]
               (2) I demand entry! [Intimidation: Strength + Charisma, Difficulty 15]
               (3) I have important information. [Persuasion: Charisma, Difficulty 12]
               (4) *Attempt to sneak past* [Stealth: Dexterity, Difficulty 18]
        
        If Player chooses 1:
        Guard: Very well, move along then.
        [End conversation]
        
        If Player chooses 2 and passes Intimidation:
        Guard: *Gulp* Right this way, sir!
        [End conversation - guard is intimidated]
        
        If Player chooses 2 and fails Intimidation:
        Guard: *Laughs* Nice try. Now I'm watching you extra close.
        [Add temporary -2 to all social rolls]
        [End conversation]
        
        If Player chooses 3 and passes Persuasion:
        Guard: Oh? What information do you have?
        Player: (1) *Bluff* The mayor sent me.
               (2) *Tell truth* I heard about smugglers in the area.
        
        If Player chooses 4 and passes Stealth:
        [Silently move past guard without being noticed]
        [End conversation]
        
        If Player chooses 4 and fails Stealth:
        Guard: Hey! Stop right there!
        [Combat encounter or arrest]
        */
    }
}