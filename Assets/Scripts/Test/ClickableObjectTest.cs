using UnityEngine;

/// <summary>
/// Test script to set up clickable objects in a scene
/// </summary>
public class ClickableObjectTest : MonoBehaviour
{
    public GameObject clickableObjectPrefab;
    public GameObject textPopupPrefab;
    
    void Start()
    {
        // Ensure we have the database
        if (ObjectDatabase.Instance == null)
        {
            GameObject dbObject = new GameObject("ObjectDatabase");
            dbObject.AddComponent<ObjectDatabase>();
        }
        
        // Create some test objects
        CreateTestObject(new Vector3(-2, 1, 0), "obj1");
        CreateTestObject(new Vector3(0, 1, 0), "obj2");
        CreateTestObject(new Vector3(2, 1, 0), "obj3");
        CreateTestObject(new Vector3(4, 1, 0), "default_test"); // This will use default text
    }
    
    void CreateTestObject(Vector3 position, string objectId)
    {
        if (clickableObjectPrefab == null)
        {
            Debug.LogError("Clickable object prefab not assigned!");
            return;
        }
        
        GameObject obj = Instantiate(clickableObjectPrefab, position, Quaternion.identity);
        ClickableObject clickable = obj.GetComponent<ClickableObject>();
        
        if (clickable != null)
        {
            // Set up the text popup prefab
            // Note: ClickableObject now uses SimpleObjectUI singleton automatically
            
            // Load data from ID
            clickable.LoadFromId(objectId);
            
            // If no ID found or for testing, set some default values
            if (string.IsNullOrEmpty(objectId) || objectId == "default_test")
            {
                clickable.objectName = "Test Object";
                clickable.objectDescription = "This is a test object created by the ClickableObjectTest script.";
            }
        }
    }
}