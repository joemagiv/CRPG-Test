# NPC and Dialogue System - Disco Elysium Style

## Overview

This implementation provides a basic NPC and Dialogue system using the PixelCrushers Dialogue System, with a UI style inspired by Disco Elysium.

## Features

- **NPC System**: Easy-to-use NPC controller with dialogue triggers
- **Disco Elysium Style UI**: Wide black band on the right side of the screen
- **Colored Dialogue**: Different characters have different colored fonts
- **Keyboard Shortcuts**: Use number keys (1-9) to select dialogue options
- **Stat-Based Rolls**: Implement RPG-style stat checks for dialogue options
- **Integration**: Seamless integration with PixelCrushers Dialogue System

## Setup Instructions

### 1. Basic Setup

1. **Add Dialogue Manager**: Ensure the Dialogue System's `Dialogue Manager` prefab is in your scene
2. **Set Up Player**: Add these components to your player GameObject:
   - `CharacterStats` - For stat-based rolls
   - `StatBasedDialogueConditions` - For Lua stat check functions
   - `Selector` - For NPC interaction

### 2. Create NPCs

Use the `NPCController` script on any GameObject:

```csharp
// Basic setup
NPCController npc = gameObject.AddComponent<NPCController>();
npc.SetNPCName("Friendly Villager");
npc.SetDialogueColor(new Color(0.4f, 0.8f, 0.4f)); // Green
npc.SetConversation("FriendlyGreeting");
```

### 3. Set Up UI

Add the `RightPanelDialogueUI` component to your canvas or use the prefab creator:

```csharp
// Create UI programmatically
var creator = gameObject.AddComponent<RightPanelDialogueUIPrefabCreator>();
GameObject ui = creator.CreateDialogueUI();
```

### 4. Create Conversations

Use the PixelCrushers Dialogue System Editor to create conversations. For stat-based options:

```lua
-- In conversation conditions:
CheckStatRoll("Strength", 15)  -- Check if Strength roll >= 15
GetStatValue("Intelligence")   -- Get current Intelligence value
RollStat("Charisma")           -- Roll Charisma and return result
```

## Example Conversation Structure

```
Title: GuardChallenge

Guard: Halt! What's your business here?

Player Options:
1. "Just passing through." (No check)
   -> Guard: Move along then.

2. "Let me pass!" (Intimidation DC 15)
   Success: Guard: *gulps* Right away, sir!
   Failure: Guard: Nice try. Not happening.

3. "I have information." (Persuasion DC 12)
   Success: Guard: What information?
   Failure: Guard: Yeah, right.

4. *Sneak past* (Stealth DC 18 - hidden)
   Success: [Silently move past]
   Failure: Guard: Hey! Stop!
```

## Keyboard Controls

- **Number Keys (1-9)**: Select dialogue options
- **Mouse Click**: Click on dialogue options
- **F1**: Test conversation (debug)
- **F2**: Test all stat rolls (debug)
- **F3**: Test specific stat roll (debug)

## Components

### Core Components

- `NPCController.cs` - Main NPC controller
- `CharacterStats.cs` - Stat system for rolls
- `StatBasedDialogueConditions.cs` - Lua functions for stat checks
- `RightPanelDialogueUI.cs` - Custom UI controller
- `DiscoElysiumResponseButton.cs` - Response button component

### Utility Components

- `DialogueDemoSetup.cs` - Example conversation setup
- `DialogueSystemTest.cs` - Basic test functionality
- `CompleteDialogueSystemTest.cs` - Comprehensive testing
- `DialogueSystemIntegration.cs` - System integration

## Customization

### UI Customization

Modify these properties on `RightPanelDialogueUI`:
- `panelWidth` - Width of the dialogue panel
- `panelHeight` - Height of the dialogue panel
- `panelRightMargin` - Distance from right edge
- `responseButtonHeight` - Height of response buttons
- `defaultNPCColor` - Default text color

### Stat Customization

Edit the `CharacterStats` component:
- Add/remove stats in the inspector
- Modify base values and ranges
- Add temporary modifiers during gameplay

## Troubleshooting

**Issue: Dialogue doesn't appear**
- Ensure Dialogue Manager is in the scene
- Check that NPC has proper conversation title set
- Verify UI components are active

**Issue: Stat checks not working**
- Ensure player has CharacterStats component
- Check that StatBasedDialogueConditions is enabled
- Verify stat names match exactly

**Issue: Keyboard shortcuts don't work**
- Check that response buttons are properly set up
- Ensure no other UI is blocking input
- Verify number keys aren't bound to other actions

## Performance Notes

- The system uses Unity Events for dialogue callbacks
- Stat rolls are calculated on-demand
- UI is only active during conversations
- Response buttons are pooled and reused

## Future Enhancements

- Save/load dialogue state
- More advanced stat effects
- Animation integration
- Voice acting support
- Localization
- Mobile touch controls

## License

This implementation uses the PixelCrushers Dialogue System, which has its own licensing terms. All custom code is provided as-is for educational and demonstration purposes.