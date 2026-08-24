using UnityEngine;

/// <summary>
/// Test script to verify the interaction system works correctly
/// </summary>
public class InteractionSystemTest : MonoBehaviour
{
    public ClickableObject testObject;
    public IsometricCharacterController playerController;
    
    void Start()
    {
        Debug.Log("Starting Interaction System Test");
        
        // Verify all managers are initialized
        if (SimpleObjectUI.Instance == null)
        {
            Debug.LogError("SimpleObjectUI not initialized!");
        }
        else
        {
            Debug.Log("SimpleObjectUI initialized successfully");
        }
        
        if (InteractionMenu.Instance == null)
        {
            Debug.LogError("InteractionMenu not initialized!");
        }
        else
        {
            Debug.Log("InteractionMenu initialized successfully");
        }
        
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager not initialized!");
        }
        else
        {
            Debug.Log("InventoryManager initialized successfully");
        }
        
        // Test clickable object configuration
        if (testObject != null)
        {
            Debug.Log("Test object configured:");
            Debug.Log("- Name: " + testObject.objectName);
            Debug.Log("- Description: " + testObject.objectDescription);
            Debug.Log("- Inspect Text: " + testObject.inspectText);
            Debug.Log("- Use Text: " + testObject.useText);
            Debug.Log("- Talk Text: " + testObject.talkText);
            Debug.Log("- Is Pickupable: " + testObject.isPickupable);
            Debug.Log("- Item ID: " + testObject.itemId);
        }
        else
        {
            Debug.LogWarning("No test object assigned - some tests will be skipped");
        }
    }
    
    void Update()
    {
        // Test inventory functionality with key presses
        if (Input.GetKeyDown(KeyCode.I))
        {
            TestInventory();
        }
        
        if (Input.GetKeyDown(KeyCode.T) && testObject != null)
        {
            TestInteractionMenu();
        }
    }
    
    private void TestInventory()
    {
        Debug.Log("Testing Inventory System");
        
        // Add test items
        InventoryManager.Instance.AddItem("test_item_1", "Health Potion", "Restores 50 health points", 3);
        InventoryManager.Instance.AddItem("test_item_2", "Mana Potion", "Restores 30 mana points", 1);
        
        // Check inventory contents
        Debug.Log("Inventory contents:");
        foreach (var item in InventoryManager.Instance.GetAllItems())
        {
            Debug.Log("- " + item.itemName + " (ID: " + item.itemId + ", Qty: " + item.quantity + ")");
        }
        
        // Test item removal
        InventoryManager.Instance.RemoveItem("test_item_1", 1);
        Debug.Log("After removing 1 Health Potion: " + InventoryManager.Instance.GetItemCount("test_item_1"));
    }
    
    private void TestInteractionMenu()
    {
        Debug.Log("Testing Interaction Menu");
        
        // Show interaction menu for test object
        Vector3 menuPosition = testObject.transform.position + new Vector3(0, 2f, 0);
        InteractionMenu.Instance.ShowMenu(testObject, menuPosition);
    }
}