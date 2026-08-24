using UnityEngine;

/// <summary>
/// Test script to find the perfect world space scale for the interaction menu
/// </summary>
public class WorldSpaceScaleTest : MonoBehaviour
{
    public float testScale = 0.005f;
    public Vector3 testOffset = new Vector3(0, 0.5f, 0.3f);
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Equals))
        {
            testScale += 0.001f;
            Debug.Log("Increased scale to: " + testScale);
            TestScale();
        }
        
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            testScale -= 0.001f;
            Debug.Log("Decreased scale to: " + testScale);
            TestScale();
        }
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testOffset.y += 0.1f;
            Debug.Log("Increased Y offset to: " + testOffset.y);
            TestPosition();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testOffset.y -= 0.1f;
            Debug.Log("Decreased Y offset to: " + testOffset.y);
            TestPosition();
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            testOffset.z += 0.1f;
            Debug.Log("Increased Z offset to: " + testOffset.z);
            TestPosition();
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            testOffset.z -= 0.1f;
            Debug.Log("Decreased Z offset to: " + testOffset.z);
            TestPosition();
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveOptimalSettings();
        }
    }
    
    private void TestScale()
    {
        // Create a test menu with current scale
        GameObject testObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        testObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
        testObject.transform.localScale = Vector3.one * 0.5f;
        
        ClickableObject clickable = testObject.AddComponent<ClickableObject>();
        clickable.objectName = "Scale Test Object";
        clickable.objectDescription = "Testing scale: " + testScale;
        
        // Show menu with test scale
        Vector3 menuPosition = testObject.transform.position + testOffset;
        InteractionMenu.Instance.ShowMenu(clickable, menuPosition);
        
        // Apply test scale to the menu
        Canvas menuCanvas = InteractionMenu.Instance.gameObject.GetComponentInChildren<Canvas>();
        if (menuCanvas != null)
        {
            menuCanvas.transform.localScale = Vector3.one * testScale;
        }
        
        Debug.Log("Testing scale: " + testScale + " at position: " + menuPosition);
        
        // Clean up
        Destroy(testObject, 10f);
    }
    
    private void TestPosition()
    {
        Debug.Log("Testing position offset: " + testOffset);
        
        // If menu is active, update its position
        if (InteractionMenu.Instance != null && InteractionMenu.Instance.gameObject.activeSelf)
        {
            GameObject testObject = GameObject.Find("Scale Test Object(Clone)");
            if (testObject != null)
            {
                Vector3 menuPosition = testObject.transform.position + testOffset;
                InteractionMenu.Instance.gameObject.transform.position = menuPosition;
                Debug.Log("Updated menu position to: " + menuPosition);
            }
        }
    }
    
    private void SaveOptimalSettings()
    {
        Debug.Log("Optimal settings found:");
        Debug.Log("- Scale: " + testScale);
        Debug.Log("- Offset: " + testOffset);
        Debug.Log("Update these values in InteractionMenu.cs:");
        Debug.Log("panelRect.localScale = new Vector3(" + testScale + "f, " + testScale + "f, " + testScale + "f);");
        Debug.Log("Vector3 menuWorldPosition = worldPosition + new Vector3(0, " + testOffset.y + "f, " + testOffset.z + "f);");
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.Label("World Space Scale Test");
        GUILayout.Label("Current Scale: " + testScale);
        GUILayout.Label("Current Offset: " + testOffset);
        GUILayout.Label("Controls:");
        GUILayout.Label("+/-: Adjust scale");
        GUILayout.Label("Arrow keys: Adjust position");
        GUILayout.Label("S: Save optimal settings");
        GUILayout.EndArea();
    }
}