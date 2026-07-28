# Dialogue System Conversations

This folder contains conversation templates and tools for the PixelCrushers Dialogue System.

## Files Created

### JSON Conversations
- `JSON/CharismaTestConversation.json` - A complete conversation with a Charisma stat check

### CSV Conversations
- `CSV/CharismaTestConversation.csv` - Alternative CSV format of the same conversation

### Tools
- `Tools/DialogueImporter.cs` - Script to import conversations programmatically
- `Tools/TestCharismaConversation.cs` - Test script for the Charisma conversation

## How to Use

### Importing JSON Conversation

1. **Manual Import**:
   - Open Dialogue Database Editor (`Window > Pixel Crushers > Dialogue System > Dialogue Database Editor`)
   - Click "Import" button
   - Select `CharismaTestConversation.json`
   - Click "Import"

2. **Programmatic Import**:
   - Attach `DialogueImporter.cs` to a GameObject in your scene
   - Assign the JSON file in the inspector
   - Click "Import JSON Conversation" in the context menu

### Using the Conversation

After importing, create an NPC that uses this conversation:

```csharp
GameObject npcObj = new GameObject("Mysterious Merchant");
NPCController npc = npcObj.AddComponent<NPCController>();
npc.SetNPCName("Mysterious Merchant");
npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f)); // Purple
npc.SetConversation("CharismaTest"); // Use the conversation title
```

### Testing the Conversation

Attach `TestCharismaConversation.cs` to any GameObject in your scene:
- Press **T** to start the CharismaTest conversation
- Press **Y** to test stat rolls

### Conversation Structure

**CharismaTest Conversation**:

1. **NPC**: "Hello there, traveler! I have a proposition for you."
2. **Player Options**:
   - "I'm listening." → Follow-up question
   - "Not interested." → End conversation

3. **Follow-up**: "Excellent! I need someone to convince the town guard..."
4. **Player Options**:
   - "Absolutely! I'm very persuasive." (Charisma DC 15 check)
     - **Success**: "You seem confident!" + Charisma +1
     - **Failure**: "Hmm, maybe this isn't such a good idea..." + Reputation -1
   - "I'll try, but no promises." → "Well, do your best."

## Troubleshooting

**Conversation not found**:
- Verify the JSON was imported correctly
- Check that the conversation title is exactly "CharismaTest"
- Ensure the conversation appears in the Dialogue Database Editor

**Stat checks not working**:
- Verify player has `CharacterStats` component
- Check that `StatBasedDialogueConditions` is enabled
- Ensure stat names match exactly (case-sensitive)

**UI not showing**:
- Check that `RightPanelDialogueUI` is properly set up
- Verify all UI references are assigned
- Ensure the dialogue panel is active

## Creating New Conversations

To create new conversations:

1. **Copy the template**: Duplicate `CharismaTestConversation.json`
2. **Edit the JSON**: Modify the conversation structure
3. **Import**: Use the Dialogue Database Editor to import
4. **Test**: Use the test script to verify it works

## File Structure

```
Assets/Dialogue/
├── JSON/              # JSON conversation files
│   └── CharismaTestConversation.json
├── CSV/               # CSV conversation files  
│   └── CharismaTestConversation.csv
├── Tools/             # Utility scripts
│   ├── DialogueImporter.cs
│   └── TestCharismaConversation.cs
└── README.md          # This file
```

## Notes

- The JSON format follows the PixelCrushers Dialogue System structure
- Conversations can be edited in the Dialogue Database Editor after import
- Stat checks use Lua functions: `CheckStatRoll("StatName", difficulty)`
- Stat modifications use Lua: `SetVariable("StatName", value)`