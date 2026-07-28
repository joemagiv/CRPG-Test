using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Simple and reliable object UI system
/// </summary>
public class SimpleObjectUI : MonoBehaviour
{
    public static SimpleObjectUI Instance { get; private set; }
    
    private GameObject currentNameDisplay;
    private GameObject currentDescriptionPopup;
    
    /// <summary>
    /// Whether a description popup is currently active (name is kept visible for it)
    /// </summary>
    public bool IsDescriptionActive => currentDescriptionPopup != null;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Clean up any existing UI elements
            CleanupExistingUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Clean up any existing UI elements from previous sessions
    /// </summary>
    private void CleanupExistingUI()
    {
        // Destroy any objects with SimpleNameDisplay or SimpleDescriptionPopup components
        SimpleNameDisplay[] nameDisplays = FindObjectsByType<SimpleNameDisplay>(FindObjectsSortMode.None);
        foreach (SimpleNameDisplay display in nameDisplays)
        {
            if (display != null && display.gameObject != null)
            {
                Destroy(display.gameObject);
            }
        }
        
        SimpleDescriptionPopup[] descriptionPopups = FindObjectsByType<SimpleDescriptionPopup>(FindObjectsSortMode.None);
        foreach (SimpleDescriptionPopup popup in descriptionPopups)
        {
            if (popup != null && popup.gameObject != null)
            {
                Destroy(popup.gameObject);
            }
        }
        
        Debug.Log("Cleaned up existing UI elements");
    }
    
    /// <summary>
    /// Show object name at world position
    /// </summary>
    public void ShowName(string name, Vector3 worldPosition)
    {
        if (string.IsNullOrEmpty(name))
            return;
            
        Debug.Log($"Showing name display for: {name}");
        
        // Reuse the existing name display instead of creating a new one. This
        // prevents an orphaned (unreferenced) display from leaking when ShowName
        // is called again while a description popup is keeping the old one alive.
        if (currentNameDisplay == null)
        {
            currentNameDisplay = CreateNameDisplay();
        }
        
        // Set up the name display
        SimpleNameDisplay nameDisplay = currentNameDisplay.GetComponent<SimpleNameDisplay>();
        if (nameDisplay != null)
        {
            nameDisplay.Setup(name, worldPosition);
        }
        else
        {
            Debug.LogError("SimpleNameDisplay component not found!");
        }
    }
    
    /// <summary>
    /// Hide the current name display
    /// </summary>
    public void HideName()
    {
        if (currentNameDisplay != null)
        {
            // Don't hide name if description is being shown
            if (currentDescriptionPopup == null)
            {
                Debug.Log("Hiding name display");
                Destroy(currentNameDisplay);
                currentNameDisplay = null;
            }
            else
            {
                Debug.Log("Not hiding name display - description is active");
            }
        }
    }
    
    /// <summary>
    /// Show description at world position
    /// </summary>
    public void ShowDescription(string description, Vector3 worldPosition)
    {
        // Don't hide name display - let it fade out with description
        // Hide any existing description popup
        HideDescription();
        
        if (string.IsNullOrEmpty(description))
            return;
            
        // Create description popup
        currentDescriptionPopup = CreateDescriptionPopup();
        
        // Set up the popup and link it to the current name display so it sits
        // directly below the name (prevents overlap) and the two fade together.
        SimpleDescriptionPopup popup = currentDescriptionPopup.GetComponent<SimpleDescriptionPopup>();
        if (popup != null)
        {
            popup.Setup(description, worldPosition);

            if (currentNameDisplay != null)
            {
                popup.linkedNameDisplay = currentNameDisplay.GetComponent<SimpleNameDisplay>();
            }
        }
        else
        {
            Debug.LogError("SimpleDescriptionPopup component not found!");
        }
        
        // Don't fade out the name display - let it stay visible with the description
        // The name display will be destroyed when the description is destroyed
        Debug.Log("Keeping name display visible with description");
    }
    
    /// <summary>
    /// Hide the current description popup
    /// </summary>
    public void HideDescription()
    {
        if (currentDescriptionPopup != null)
        {
            Destroy(currentDescriptionPopup);
            currentDescriptionPopup = null;
        }
    }
    
    /// <summary>
    /// Destroy the name display (called by description popup)
    /// </summary>
    public void DestroyNameDisplay()
    {
        if (currentNameDisplay != null)
        {
            SimpleNameDisplay nameDisplay = currentNameDisplay.GetComponent<SimpleNameDisplay>();
            if (nameDisplay != null)
            {
                // Trigger fade out in the name display
                nameDisplay.FadeOutAndDestroy();
            }
            else
            {
                // Fallback: destroy immediately if no component found
                Destroy(currentNameDisplay);
            }
            currentNameDisplay = null;
        }
    }
    
    /// <summary>
    /// Create name display prefab
    /// </summary>
    private GameObject CreateNameDisplay()
    {
        GameObject obj = new GameObject("NameDisplay");
        
        // Add Canvas
        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1000;
        
        // Add Canvas Scaler
        CanvasScaler scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Add Graphic Raycaster
        obj.AddComponent<GraphicRaycaster>();
        
        // Add panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(obj.transform);
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.7f);
        
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(200, 30);
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(panel.transform);
        
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.fontSize = 18;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        text.rectTransform.sizeDelta = new Vector2(190, 25);
        text.rectTransform.anchoredPosition = Vector2.zero;
        
        // Add SimpleNameDisplay component
        SimpleNameDisplay nameDisplay = obj.AddComponent<SimpleNameDisplay>();
        nameDisplay.textComponent = text;
        nameDisplay.panelRect = panelRect;
        
        return obj;
    }
    
    /// <summary>
    /// Create description popup prefab
    /// </summary>
    private GameObject CreateDescriptionPopup()
    {
        GameObject obj = new GameObject("DescriptionPopup");
        
        // Add Canvas
        Canvas canvas = obj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1001;
        
        // Add Canvas Scaler
        CanvasScaler scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Add Graphic Raycaster
        obj.AddComponent<GraphicRaycaster>();
        
        // Add panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(obj.transform);
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(300, 200);
        panelRect.anchoredPosition = Vector2.zero;
        
        // Add text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(panel.transform);
        
        TextMeshProUGUI text = textObj.AddComponent<TextMeshProUGUI>();
        text.fontSize = 18; // Larger font size
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.TopLeft;
        text.rectTransform.sizeDelta = new Vector2(280, 180);
        text.rectTransform.anchoredPosition = new Vector2(0, -10);
        text.textWrappingMode = TMPro.TextWrappingModes.Normal;
        
        // Add SimpleDescriptionPopup component
        SimpleDescriptionPopup popup = obj.AddComponent<SimpleDescriptionPopup>();
        popup.textComponent = text;
        popup.panelRect = panelRect;
        popup.lifetime = 5f;
        
        return obj;
    }
}