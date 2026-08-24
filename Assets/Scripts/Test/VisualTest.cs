using UnityEngine;

/// <summary>
/// Visual test to verify the interaction menu positioning
/// </summary>
public class VisualTest : MonoBehaviour
{
    public ClickableObject testObject;
    public Transform testPosition;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            TestMenuVisuals();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            TestMenuPositioning();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestPositioningAtCursor();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            TestCenteredVsOverObject();
        }
    }
    
    private void TestMenuVisuals()
    {
        Debug.Log("Testing Interaction Menu Visuals");
        
        // Hide any existing menu first
        if (InteractionMenu.Instance != null)
        {
            InteractionMenu.Instance.HideMenu();
        }
        
        if (testObject == null)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
            testObject = obj.AddComponent<ClickableObject>();
            
            testObject.objectName = "Visual Test Object";
            testObject.objectDescription = "Testing scale: 0.01";
            testObject.inspectText = "Examine";
            testObject.useText = "Interact";
            testObject.talkText = "Speak";
        }
        
        Vector3 menuPosition = testObject.transform.position + new Vector3(0, 2.5f, 0);
        InteractionMenu.Instance.ShowMenu(testObject, menuPosition);
        
        Debug.Log("Menu should be positioned OVER the test object, not in screen center");
        Debug.Log("Menu position: " + menuPosition);
        Debug.Log("Object position: " + testObject.transform.position);
        
        // Debug the screen position
        Vector3 screenPos = Camera.main.WorldToScreenPoint(menuPosition);
        Debug.Log("Screen position: " + screenPos + " (should match object, not center " + new Vector3(Screen.width/2, Screen.height/2, 0) + ")");
    }
    
    private void TestMenuPositioning()
    {
        Debug.Log("Testing Interaction Menu Positioning");
        
        if (testPosition == null)
        {
            testPosition = Camera.main.transform;
        }
        
        GameObject tempObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        tempObj.transform.position = testPosition.position + testPosition.forward * 3f;
        ClickableObject tempClickable = tempObj.AddComponent<ClickableObject>();
        
        tempClickable.objectName = "Position Test";
        tempClickable.objectDescription = "Testing menu positioning.";
        
        Vector3 menuPosition = tempClickable.transform.position + new Vector3(0, 2.5f, 0);
        InteractionMenu.Instance.ShowMenu(tempClickable, menuPosition);
        
        Debug.Log("Menu should float OVER the sphere, not at screen center");
        Debug.Log("Sphere position: " + tempClickable.transform.position);
        Debug.Log("Menu position: " + menuPosition);
        
        Destroy(tempObj, 5f);
    }
    
    private void TestPositioningAtCursor()
    {
        Debug.Log("Testing position at cursor");
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = new GameObject("Cursor Test Object");
            obj.transform.position = hit.point;
            ClickableObject clickable = obj.AddComponent<ClickableObject>();
            
            clickable.objectName = "Cursor Test";
            clickable.objectDescription = "Testing at cursor position";
            
            Vector3 menuPosition = hit.point + new Vector3(0, 0.5f, 0);
            InteractionMenu.Instance.ShowMenu(clickable, menuPosition);
            
            Debug.Log("Menu should appear at cursor position: " + hit.point);
            Debug.Log("Screen position: " + Camera.main.WorldToScreenPoint(menuPosition));
        }
        else
        {
            Debug.Log("No object hit at cursor");
        }
    }
    
    private void TestCenteredVsOverObject()
    {
        Debug.Log("=== Position Diagnostic ===");
        
        if (testObject == null)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
            testObject = obj.AddComponent<ClickableObject>();
        }
        
        Vector3 objectPos = testObject.transform.position;
        Vector3 menuPos = objectPos + new Vector3(0, 2.5f, 0);
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(menuPos);
        
            Debug.Log("Object position: " + objectPos);
            Debug.Log("Menu position: " + menuPos);
            Debug.Log("Screen position: " + screenPos);
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Debug.Log("Screen center: " + screenCenter);
            float distanceToCenter = Vector3.Distance(screenPos, screenCenter);
            Debug.Log("Distance from center: " + distanceToCenter);
            
            if (distanceToCenter < 50f)
            {
                Debug.LogWarning("ISSUE: Menu is at screen center! Not over object!");
            }
            else
            {
                Debug.Log("SUCCESS: Menu is positioned over the object");
            }
        
        InteractionMenu.Instance.ShowMenu(testObject, menuPos);
    }
    
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, Screen.height - 150, 300, 140));
        GUILayout.Label("Interaction Menu Test", new GUIStyle { fontSize = 16, fontStyle = FontStyle.Bold });
        GUILayout.Label("M: Test visuals");
        GUILayout.Label("N: Test positioning");
        GUILayout.Label("P: Test at cursor");
        GUILayout.Label("C: Diagnostic (check position)");
        GUILayout.EndArea();
    }
}