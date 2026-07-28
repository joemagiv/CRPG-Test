using UnityEngine;
using PixelCrushers.DialogueSystem;
using System.IO;

public class DialogueImporter : MonoBehaviour
{
    public TextAsset jsonFile;
    public TextAsset csvFile;
    
    [ContextMenu("Import JSON Conversation")]
    public void ImportJSONConversation()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file not assigned");
            return;
        }
        
        try
        {
            string jsonContent = jsonFile.text;
            DialogueDatabase database = DialogueDatabase.CreateInstance<DialogueDatabase>();
            
            // Parse and import the JSON
            // Note: This is a simplified example - the actual Dialogue System has built-in import methods
            Debug.Log("JSON Conversation Content:");
            Debug.Log(jsonContent);
            
            // In a real implementation, you would use:
            // DialogueSystem.ImportJSON(jsonContent);
            
            Debug.Log("JSON conversation would be imported here");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to import JSON: " + e.Message);
        }
    }
    
    [ContextMenu("Import CSV Conversation")]
    public void ImportCSVConversation()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file not assigned");
            return;
        }
        
        try
        {
            string csvContent = csvFile.text;
            Debug.Log("CSV Conversation Content:");
            Debug.Log(csvContent);
            
            // Parse CSV and create conversation entries
            string[] lines = csvContent.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    Debug.Log("Line: " + string.Join(" | ", parts));
                }
            }
            
            Debug.Log("CSV conversation would be imported here");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to import CSV: " + e.Message);
        }
    }
    
    [ContextMenu("Create Test NPC")]
    public void CreateTestNPC()
    {
        GameObject npcObj = new GameObject("Mysterious Merchant");
        NPCController npc = npcObj.AddComponent<NPCController>();
        npc.SetNPCName("Mysterious Merchant");
        npc.SetDialogueColor(new Color(0.6f, 0.4f, 0.8f)); // Purple
        npc.SetConversation("CharismaTest");
        
        // Add visual representation
        CapsuleCollider capsule = npcObj.AddComponent<CapsuleCollider>();
        capsule.radius = 0.3f;
        capsule.height = 1.8f;
        
        GameObject indicator = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        indicator.transform.SetParent(npcObj.transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = new Vector3(0.6f, 1.8f, 0.6f);
        indicator.GetComponent<Renderer>().material.color = new Color(0.6f, 0.4f, 0.8f);
        
        Debug.Log("Created test NPC for CharismaTest conversation");
    }
}