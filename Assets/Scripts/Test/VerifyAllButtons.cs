using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Simple test to verify all three buttons are created and positioned correctly
/// </summary>
public class VerifyAllButtons : MonoBehaviour
{
    public ClickableObject testObject;
    
    void Start()
    {
        if (testObject == null)
        {
            // Create a test object
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
            testObject = obj.AddComponent<ClickableObject>();
            
            // Configure it
            testObject.objectName = "Button Test Object";
            testObject.objectDescription = "Testing all buttons are visible";
            testObject.inspectText = "Inspect";
            testObject.useText = "Use";
            testObject.talkText = "Talk";
        }
        
        // Show the menu
        Vector3 menuPosition = testObject.transform.position + new Vector3(0, 2.5f, 0);
        InteractionMenu.Instance.ShowMenu(testObject, menuPosition);
        
        Debug.Log("Menu shown. Check if all three buttons are visible.");
        Debug.Log("Expected: Inspect (top), Use (middle), Talk (bottom)");
        
        // Wait a frame for the menu to be created
        Invoke("LogButtonStatus", 0.1f);
    }
    
    private void LogButtonStatus()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        Debug.Log("Total buttons found: " + buttons.Length);
        
        foreach (Button button in buttons)
        {
            TMPro.TextMeshProUGUI buttonText = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            string text = buttonText != null ? buttonText.text : "(no text)";
            Debug.Log("Button: " + button.name + " - Text: " + text);
        }
    }
}