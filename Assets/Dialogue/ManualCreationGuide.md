# Manual Creation Guide for Charisma Test Conversation

Since the Dialogue System doesn't have a direct JSON import button, here's how to create the Charisma test conversation manually:

## Step 1: Open the Dialogue Database Editor

1. In Unity, go to: **Window > Pixel Crushers > Dialogue System > Dialogue Database Editor**
2. Make sure you're in the "Conversations" tab

## Step 2: Create Actors

1. Go to the **Actors** tab
2. Click **Add** to create a new actor
3. **First Actor (NPC)**:
   - Name: `NPC`
   - Display Name: `Mysterious Merchant`
   - Description: `A merchant with a proposition`
   - Color: `#9966CC` (purple)
4. Click **Add** again for the second actor
5. **Second Actor (Player)**:
   - Name: `Player`
   - Display Name: `Player`
   - Description: `The player character`
   - Color: `#FFFFFF` (white)

## Step 3: Create the Conversation

1. Go back to the **Conversations** tab
2. Click **Add** and select **Conversation**
3. Name it: `CharismaTest`
4. Description: `A conversation with a charisma stat check`

## Step 4: Add Dialogue Entries

### Entry 1: NPC Opening Line
1. Click **Add Entry**
2. **Actor**: Select "NPC" (Mysterious Merchant)
3. **Text**: `Hello there, traveler! I have a proposition for you.`
4. Check **Is Start** (this marks the starting point)
5. Click **Apply**

### Entry 2: Player Response - "I'm listening"
1. **Right-click** Entry 1 and select **Add Player Response**
2. **Actor**: Select "Player"
3. **Text**: `I'm listening.`
4. Click **Apply**

### Entry 3: NPC Follow-up
1. **Right-click** Entry 2 and select **Add NPC Response**
2. **Actor**: Select "NPC"
3. **Text**: `Excellent! I need someone to convince the town guard to let my shipment through. Are you up for the task?`
4. Click **Apply**

### Entry 4: Player Response - Charisma Check
1. **Right-click** Entry 3 and select **Add Player Response**
2. **Actor**: Select "Player"
3. **Text**: `Absolutely! I'm very persuasive.`
4. **Conditions**: Click **Add Condition** and enter:
   ```
   CheckStatRoll("Charisma", 15)
   ```
5. Click **Apply**

### Entry 5: Success Response
1. **Right-click** Entry 4 and select **Add NPC Response**
2. **Actor**: Select "NPC"
3. **Text**: `*impressed* You seem confident! The guard is over there. Good luck!`
4. **Conditions**: Click **Add Condition** and enter:
   ```
   CheckStatRoll("Charisma", 15)
   ```
5. **Sequences**: Click **Add Sequence** and enter:
   ```
   SetVariable("Charisma", GetVariable("Charisma") + 1); ShowAlert("Your Charisma increased by 1!");
   ```
6. Click **Apply**

### Entry 6: Failure Response
1. **Right-click** Entry 4 again and select **Add NPC Response**
2. **Actor**: Select "NPC"
3. **Text**: `*skeptical* Hmm, maybe this isn't such a good idea after all...`
4. **Conditions**: Click **Add Condition** and enter:
   ```
   !CheckStatRoll("Charisma", 15)
   ```
5. **Sequences**: Click **Add Sequence** and enter:
   ```
   SetVariable("Reputation", GetVariable("Reputation") - 1); ShowAlert("Your Reputation decreased by 1!");
   ```
6. Click **Apply**

### Entry 7: Player Response - "I'll try"
1. **Right-click** Entry 3 and select **Add Player Response**
2. **Actor**: Select "Player"
3. **Text**: `I'll try, but no promises.`
4. Click **Apply**

### Entry 8: NPC Response to "I'll try"
1. **Right-click** Entry 7 and select **Add NPC Response**
2. **Actor**: Select "NPC"
3. **Text**: `Well, do your best. The guard can be quite stubborn.`
4. Click **Apply**

### Entry 9: Player Response - "Not interested"
1. **Right-click** Entry 1 (the opening line) and select **Add Player Response**
2. **Actor**: Select "Player"
3. **Text**: `Not interested.`
4. Click **Apply**

### Entry 10: NPC Response to "Not interested"
1. **Right-click** Entry 9 and select **Add NPC Response**
2. **Actor**: Select "NPC"
3. **Text**: `Suit yourself. Maybe someone else will help me then.`
4. Click **Apply**

## Step 5: Save the Database

1. Click **Save** in the Dialogue Database Editor
2. Close the editor

## Step 6: Create an NPC to Use This Conversation

```csharp
// Add this to your scene setup or NPC creation script:
GameObject npcObj = new GameObject("Mysterious Merchant");
NPCController npc = npcObj.AddComponent<NPCController>();
npc.SetNPCName("Mysterious Merchant");
npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f)); // Purple
npc.SetConversation("CharismaTest"); // Use the conversation title

// Add interaction components
npcObj.AddComponent<Usable>();
DialogueSystemTrigger trigger = npcObj.AddComponent<DialogueSystemTrigger>();
trigger.conversation = "CharismaTest";
trigger.trigger = DialogueSystemTriggerEvent.OnUse;
```

## Step 7: Test the Conversation

```csharp
// Add this test script to any GameObject:
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TestConversation : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (DialogueManager.hasInstance)
            {
                DialogueManager.StartConversation("CharismaTest");
                Debug.Log("Started CharismaTest conversation");
            }
        }
    }
}
```

## Troubleshooting

**Conversation not appearing**:
- Make sure you clicked **Save** in the Database Editor
- Check that the conversation title is exactly "CharismaTest"
- Verify the conversation appears in the database list

**Stat checks not working**:
- Ensure player has `CharacterStats` component
- Check that `StatBasedDialogueConditions` is enabled
- Verify stat names match exactly (case-sensitive)

**No response options**:
- Check that player responses are marked as "Is Player"
- Ensure entries are properly linked
- Verify the conversation has a start entry

## Alternative: Use the Automatic Creator

If manual creation is too time-consuming, use the `CreateCharismaTestConversation` script:

1. Attach `CreateCharismaTestConversation.cs` to any GameObject
2. In the Unity Editor, right-click the component
3. Select **Create Charisma Test Conversation**
4. The conversation will be created automatically

## Conversation Flow Diagram

```
NPC: "Hello there, traveler! I have a proposition for you."
├─ Player: "I'm listening." → NPC: "Excellent! I need someone to..."
│   ├─ Player: "Absolutely! I'm very persuasive." (Charisma DC 15)
│   │   ├─ Success: "You seem confident!" (+1 Charisma)
│   │   └─ Failure: "Hmm, maybe this isn't..." (-1 Reputation)
│   └─ Player: "I'll try, but no promises." → "Well, do your best."
└─ Player: "Not interested." → "Suit yourself."
```

This manual creation process ensures the conversation is properly set up in the Dialogue System's native format and will work reliably with all the other components we've created.