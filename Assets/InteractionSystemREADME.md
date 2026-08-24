# Interaction Menu System - Debugging Guide

## Current State (as of latest fix)

The InteractionMenu has been completely rewritten with a simpler, more robust approach:

### Architecture
- **InteractionMenu singleton**: Created once, builds a canvas with 3 buttons
- **Screen Space Overlay**: Menu renders on top of the 3D world
- **World-to-Screen Positioning**: Uses `Camera.WorldToScreenPoint` + offset
- **Position Before Show**: Menu position is set BEFORE `SetActive(true)`

### Files
- `Assets/Scripts/UI/InteractionMenu.cs` - Main menu logic
- `Assets/Scripts/Interactable/ClickableObject.cs` - Triggers menu on click
- `Assets/Scripts/Inventory/InventoryManager.cs` - Inventory system
- `Assets/Scripts/Test/DebugButtonPositions.cs` - Debug helper (press D)

### How to Debug

1. **Attach DebugButtonPositions.cs** to any GameObject
2. **Click an object** to show the menu
3. **Press D** to dump the menu state to console:
   - Confirms Instance exists
   - Shows canvas active state
   - Lists all 3 buttons with positions/sizes/text
   - Shows panel position

### Common Issues & Fixes

**Buttons not showing:**
- Check console for `[InteractionMenu] Menu built with 3 buttons` log
- Press D to verify buttons exist but might be off-screen
- Verify `panelRect.position` is correct screen coordinate

**Menu at center of screen:**
- Menu position is set in `PositionMenu()` using `WorldToScreenPoint`
- Offset is +200px above object
- Should follow object in Update()

**Gray square in center:**
- Caused by menu showing at (0,0) before positioning
- Fixed by positioning BEFORE SetActive(true)
- Also ensure no duplicate InteractionMenu instances

### Testing Checklist
- [ ] Click object → menu appears over object
- [ ] 3 buttons visible: Inspect, Use, Talk
- [ ] Buttons clickable → correct response
- [ ] Escape closes menu
- [ ] Menu follows object if it moves

## Previous Issues (Resolved)
- Duplicate menu instances causing orphaned panels
- Buttons overlapping due to wrong anchors
- Menu flashing at center before positioning
- TextMeshPro vs UnityEngine.UI.Text confusion
