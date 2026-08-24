using UnityEngine;

/// <summary>
/// Simple test to verify inventory system works without Dialogue System dependencies
/// </summary>
public class SimpleInventoryTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Starting Simple Inventory Test");
        
        // Test basic inventory functionality
        TestBasicInventory();
        
        // Test clickable object integration
        TestClickableObjectIntegration();
    }
    
    private void TestBasicInventory()
    {
        Debug.Log("=== Testing Basic Inventory ===");
        
        // Clear inventory first
        InventoryManager.Instance.ClearInventory();
        
        // Add some test items
        InventoryManager.Instance.AddItem("health_potion", "Health Potion", "Restores 50 HP", 3);
        InventoryManager.Instance.AddItem("mana_potion", "Mana Potion", "Restores 30 MP", 1);
        InventoryManager.Instance.AddItem("gold_key", "Golden Key", "Opens the treasure chest", 1);
        
        // Test item count
        Debug.Log("Health Potion count: " + InventoryManager.Instance.GetItemCount("health_potion"));
        Debug.Log("Mana Potion count: " + InventoryManager.Instance.GetItemCount("mana_potion"));
        Debug.Log("Golden Key count: " + InventoryManager.Instance.GetItemCount("gold_key"));
        
        // Test has item
        Debug.Log("Has Health Potion: " + InventoryManager.Instance.HasItem("health_potion"));
        Debug.Log("Has Non-existent Item: " + InventoryManager.Instance.HasItem("non_existent"));
        
        // Test removal
        InventoryManager.Instance.RemoveItem("health_potion", 1);
        Debug.Log("After removing 1 Health Potion: " + InventoryManager.Instance.GetItemCount("health_potion"));
        
        // List all items
        Debug.Log("All inventory items:");
        foreach (var item in InventoryManager.Instance.GetAllItems())
        {
            Debug.Log("- " + item.itemName + " (ID: " + item.itemId + ", Qty: " + item.quantity + ")");
        }
        
        Debug.Log("=== Basic Inventory Test Complete ===");
    }
    
    private void TestClickableObjectIntegration()
    {
        Debug.Log("=== Testing Clickable Object Integration ===");
        
        // Create a test clickable object
        GameObject testObject = new GameObject("TestClickableObject");
        ClickableObject clickable = testObject.AddComponent<ClickableObject>();
        
        // Configure the object
        clickable.objectName = "Test Chest";
        clickable.objectDescription = "An old wooden chest that looks valuable.";
        clickable.inspectText = "Examine";
        clickable.useText = "Open";
        clickable.talkText = "Listen";
        clickable.inspectResult = "The chest is made of oak and has intricate carvings.";
        clickable.useResult = "You open the chest and find treasure!";
        clickable.talkResult = "The chest creaks ominously...";
        clickable.isPickupable = false;
        
        // Test that the object is configured correctly
        Debug.Log("Test object configured:");
        Debug.Log("- Name: " + clickable.objectName);
        Debug.Log("- Inspect Text: " + clickable.inspectText);
        Debug.Log("- Use Text: " + clickable.useText);
        Debug.Log("- Talk Text: " + clickable.talkText);
        Debug.Log("- Is Pickupable: " + clickable.isPickupable);
        
        // Test pickupable object
        GameObject pickupObject = new GameObject("TestPickupObject");
        ClickableObject pickup = pickupObject.AddComponent<ClickableObject>();
        pickup.objectName = "Health Potion";
        pickup.objectDescription = "A red potion that restores health.";
        pickup.isPickupable = true;
        pickup.itemId = "health_potion";
        pickup.itemQuantity = 1;
        
        Debug.Log("Pickup object configured:");
        Debug.Log("- Name: " + pickup.objectName);
        Debug.Log("- Is Pickupable: " + pickup.isPickupable);
        Debug.Log("- Item ID: " + pickup.itemId);
        
        Debug.Log("=== Clickable Object Integration Test Complete ===");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestBasicInventory();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TestClickableObjectIntegration();
        }
    }
}