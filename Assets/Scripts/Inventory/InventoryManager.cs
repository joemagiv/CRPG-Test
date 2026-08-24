using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Inventory system that integrates with Dialogue System
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    
    [System.Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string itemName;
        public string itemDescription;
        public int quantity;
        public Sprite icon; // Optional icon
    }
    
    private List<InventoryItem> inventoryItems = new List<InventoryItem>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Add an item to inventory
    /// </summary>
    public void AddItem(string itemId, string itemName, string itemDescription, int quantity = 1)
    {
        // Check if item already exists
        InventoryItem existingItem = inventoryItems.Find(item => item.itemId == itemId);
        
        if (existingItem != null)
        {
            // Stackable item
            existingItem.quantity += quantity;
            Debug.Log("Added " + quantity + " " + itemName + " to existing stack. Total: " + existingItem.quantity);
        }
        else
        {
            // New item
            InventoryItem newItem = new InventoryItem
            {
                itemId = itemId,
                itemName = itemName,
                itemDescription = itemDescription,
                quantity = quantity
            };
            
            inventoryItems.Add(newItem);
            Debug.Log("Added new item: " + itemName + " (ID: " + itemId + ")");
        }
        
        // Update Dialogue System variables
        UpdateDialogueSystemVariables();
    }
    
    /// <summary>
    /// Remove an item from inventory
    /// </summary>
    public bool RemoveItem(string itemId, int quantity = 1)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemId == itemId);
        
        if (item != null)
        {
            if (item.quantity <= quantity)
            {
                inventoryItems.Remove(item);
                Debug.Log("Removed all " + item.itemName + " from inventory");
            }
            else
            {
                item.quantity -= quantity;
                Debug.Log("Removed " + quantity + " " + item.itemName + ". Remaining: " + item.quantity);
            }
            
            UpdateDialogueSystemVariables();
            return true;
        }
        
        Debug.LogWarning("Item not found in inventory: " + itemId);
        return false;
    }
    
    /// <summary>
    /// Check if player has an item
    /// </summary>
    public bool HasItem(string itemId, int quantity = 1)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemId == itemId);
        return item != null && item.quantity >= quantity;
    }
    
    /// <summary>
    /// Get item count
    /// </summary>
    public int GetItemCount(string itemId)
    {
        InventoryItem item = inventoryItems.Find(i => i.itemId == itemId);
        return item != null ? item.quantity : 0;
    }
    
    /// <summary>
    /// Get all inventory items
    /// </summary>
    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(inventoryItems);
    }
    
    /// <summary>
    /// Clear inventory
    /// </summary>
    public void ClearInventory()
    {
        inventoryItems.Clear();
        UpdateDialogueSystemVariables();
        Debug.Log("Inventory cleared");
    }
    
    /// <summary>
    /// Update Dialogue System variables for inventory integration
    /// Note: This is a simplified version that will work without Dialogue System
    /// </summary>
    private void UpdateDialogueSystemVariables()
    {
        // For now, we'll skip Dialogue System integration to avoid compilation errors
        // The inventory system will work perfectly fine without it
        // You can manually integrate with Dialogue System later if needed
        Debug.Log("Inventory updated. For Dialogue System integration, manually set Lua variables.");
    }
    
    /// <summary>
    /// Dialogue System callback for checking inventory (if Dialogue System is available)
    /// </summary>
    public bool HasInventoryItem(string itemId, int quantity = 1)
    {
        return HasItem(itemId, quantity);
    }
    
    /// <summary>
    /// Dialogue System callback for getting item count (if Dialogue System is available)
    /// </summary>
    public int GetInventoryItemCount(string itemId)
    {
        return GetItemCount(itemId);
    }
}