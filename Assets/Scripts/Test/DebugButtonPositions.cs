using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Debug script to verify interaction menu state
/// </summary>
public class DebugButtonPositions : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DebugMenuState();
        }
    }
    
    private void DebugMenuState()
    {
        Debug.Log("=== InteractionMenu Debug ===");
        
        if (InteractionMenu.Instance == null)
        {
            Debug.LogError("InteractionMenu.Instance is NULL!");
            return;
        }
        
        // Use reflection to check private fields
        var fields = typeof(InteractionMenu).GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        GameObject menuGO = null;
        RectTransform panel = null;
        bool active = false;
        
        foreach (var f in fields)
        {
            if (f.Name == "menuGO") menuGO = f.GetValue(InteractionMenu.Instance) as GameObject;
            else if (f.Name == "panelRect") panel = f.GetValue(InteractionMenu.Instance) as RectTransform;
            else if (f.Name == "isMenuActive") active = (bool)f.GetValue(InteractionMenu.Instance);
        }
        
        Debug.Log("Menu GO: " + (menuGO != null ? menuGO.name : "null"));
        if (menuGO != null)
        {
            Debug.Log("  Active in hierarchy: " + menuGO.activeInHierarchy);
            Debug.Log("  Button count: " + menuGO.GetComponentsInChildren<Button>().Length);
            
            Button[] btns = menuGO.GetComponentsInChildren<Button>();
            foreach (Button b in btns)
            {
                RectTransform rt = b.GetComponent<RectTransform>();
                TextMeshProUGUI t = b.GetComponentInChildren<TextMeshProUGUI>();
                Debug.Log("  Button: " + b.name + " pos=" + (rt != null ? rt.position.ToString() : "n/a") + 
                          " size=" + (rt != null ? rt.sizeDelta.ToString() : "n/a") +
                          " text=" + (t != null ? t.text : "null"));
            }
        }
        
        Debug.Log("Panel: " + (panel != null ? "pos=" + panel.position + " size=" + panel.sizeDelta : "null"));
        Debug.Log("Menu active: " + active);
        Debug.Log("=== End Debug ===");
    }
}