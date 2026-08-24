# Dialogue System Integration Guide for Inventory Manager

The InventoryManager is designed to work standalone, but can be integrated with Pixel Crushers Dialogue System for advanced features like quest conditions based on inventory items.

## Manual Integration Steps

If you want to integrate the inventory system with Dialogue System, you have a few options:

### Option 1: Simple Lua Variable Updates

Add this code to your game manager or a Dialogue System callback script:

```csharp
using PixelCrushers.DialogueSystem;

public class DialogueSystemInventoryBridge : MonoBehaviour
{
    void OnEnable()
    {
        // Subscribe to inventory changes
        // You might need to add events to InventoryManager first
    }
    
    public void SyncInventoryToDialogueSystem()
    {
        if (InventoryManager.Instance != null)
        {
            // Set inventory count
            DialogueLua.SetVariable("InventoryCount", InventoryManager.Instance.GetAllItems().Count);
            
            // Set individual item variables
            foreach (var item in InventoryManager.Instance.GetAllItems())
            {
                string varName = "Has" + item.itemId.Replace(" ", "").Replace("-", "_");
                DialogueLua.SetVariable(varName, item.quantity);
            }
        }
    }
}
```

### Option 2: Use Dialogue System Conditions

In your Dialogue System conditions, you can reference the inventory variables:

- `InventoryCount > 0` - Check if player has any items
- `HasHealthPotion >= 1` - Check if player has a specific item
- `HasGoldKey == 0` - Check if player doesn't have an item

### Option 3: Direct Method Calls

You can call InventoryManager methods directly from Dialogue System Lua:

```lua
-- Check if player has an item
if InventoryManager.Instance:HasItem("HealthPotion", 1) then
    -- Player has health potion
end

-- Get item count
local potionCount = InventoryManager.Instance:GetItemCount("HealthPotion")
```

## Adding Events to InventoryManager

For automatic synchronization, you can extend InventoryManager with events:

```csharp
// Add to InventoryManager.cs
public event System.Action<InventoryItem> OnItemAdded;
public event System.Action<InventoryItem> OnItemRemoved;

// Call these in AddItem and RemoveItem methods
OnItemAdded?.Invoke(newItem);
OnItemRemoved?.Invoke(item);
```

## Troubleshooting

If you get compilation errors about missing DialogueLua:

1. Make sure you have Pixel Crushers Dialogue System installed
2. Check that your script has the correct namespace: `using PixelCrushers.DialogueSystem;`
3. Ensure your script is not in a conflicting namespace
4. Try using the full qualified name: `PixelCrushers.DialogueSystem.DialogueLua.SetVariable()`

## Current Implementation

The current InventoryManager works standalone and doesn't require Dialogue System. The core functionality (adding/removing items, checking inventory) works perfectly without integration. You can add Dialogue System integration later when needed for specific quest conditions.