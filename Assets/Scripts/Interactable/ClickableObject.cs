using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// Component that makes an object clickable with hover highlighting and text display
/// </summary>
[RequireComponent(typeof(Collider))]
public class ClickableObject : MonoBehaviour
{
    // Static reference to last clicked object for movement prevention
    public static ClickableObject lastClickedObject = null;
    
    [Header("Display Settings")]
    public string objectName = "Object";
    public string objectDescription = "This is a clickable object.";
    public string objectId = ""; // For future database/CSV integration
    
    [Header("Highlight Settings")]
    public Material highlightMaterial;
    public Material originalMaterial;
    public float highlightOutlineWidth = 0.02f;
    public Color highlightOutlineColor = new Color(0f, 0.8f, 1f, 1f);
    
    [Header("Text Display Settings")]
    public float textDisplayDuration = 5f;
    
    private Renderer objectRenderer;
    private bool isHighlighted = false;
    private Camera mainCamera;
    
    void Awake()
    {
        // Get renderer component
        objectRenderer = GetComponent<Renderer>();
        
        // Get or create highlight material
        if (highlightMaterial == null)
        {
            highlightMaterial = MaterialSetup.GetHighlightMaterial();
        }
        
        // Get main camera
        mainCamera = Camera.main;
        
        // Initialize original material if we have a renderer
        if (objectRenderer != null && objectRenderer.material != null)
        {
            originalMaterial = objectRenderer.material;
        }
        
        // Ensure SimpleObjectUI singleton exists
        if (SimpleObjectUI.Instance == null)
        {
            GameObject uiManager = new GameObject("SimpleObjectUI");
            uiManager.AddComponent<SimpleObjectUI>();
            Debug.Log("Created SimpleObjectUI instance");
        }

        Debug.Log($"ClickableObject initialized on {gameObject.name}. Renderer: {objectRenderer != null}, Material: {highlightMaterial != null}");
    }
    
    /// <summary>
    /// Prevent movement when clicking on objects by consuming the click event
    /// </summary>
    private void ConsumeMouseClick()
    {
        // This method prevents the click from propagating to movement systems
        // In Unity, we can't directly consume events, but we can set a flag
        // that other systems can check
        ClickableObject.lastClickedObject = this;
        Debug.Log("Mouse click consumed by " + objectName);
    }
    
    // Removed - using SimpleObjectUI singleton instead
    
    /// <summary>
    /// Show the object name on hover
    /// </summary>
    private void ShowHoverName()
    {
        if (SimpleObjectUI.Instance != null && !string.IsNullOrEmpty(objectName))
        {
            Vector3 hoverPosition = transform.position + new Vector3(0, 2f, 0);
            SimpleObjectUI.Instance.ShowName(objectName, hoverPosition);
        }
    }
    
    /// <summary>
    /// Hide the hover name display
    /// </summary>
    private void HideHoverName()
    {
        if (SimpleObjectUI.Instance != null)
        {
            SimpleObjectUI.Instance.HideName();
        }
    }
    
    /// <summary>
    /// Check if we should ignore mouse events (UI overlap, missing components, etc.)
    /// </summary>
    private bool ShouldIgnoreMouseEvent()
    {
        // Check if pointer is over UI (compatible with both input systems)
        if (InputSystemCompatibility.IsPointerOverUI())
            return true;
            
        // Check if we have required components
        if (objectRenderer == null || highlightMaterial == null)
            return true;
            
        return false;
    }
    
    void OnMouseEnter()
    {
        // Check if we should ignore this mouse event
        if (ShouldIgnoreMouseEvent())
            return;
            
        SetHighlight(true);
        ShowHoverName();
    }
    
    void OnMouseExit()
    {
        if (ShouldIgnoreMouseEvent())
            return;
            
        SetHighlight(false);
        Debug.Log("OnMouseExit called - hiding hover name");
        
        // Only hide name if we're not in the process of showing a description
        if (SimpleObjectUI.Instance == null || !SimpleObjectUI.Instance.IsDescriptionActive)
        {
            HideHoverName();
        }
        else
        {
            Debug.Log("Not hiding name - description is being shown");
        }
    }
    
    void OnMouseDown()
    {
        if (ShouldIgnoreMouseEvent())
            return;
            
        Debug.Log("OnMouseDown called - showing description");
        // Don't hide hover name - let it fade out with description
        // Show description popup (name stays visible)
        ShowDescriptionPopup();
        
        // Prevent movement when clicking on objects
        ConsumeMouseClick();
    }
    
    /// <summary>
    /// Enable or disable the highlight effect
    /// </summary>
    private void SetHighlight(bool enable)
    {
        if (objectRenderer == null || highlightMaterial == null)
            return;
            
        // Ensure we have a valid original material
        if (originalMaterial == null && objectRenderer.material != null)
        {
            originalMaterial = objectRenderer.material;
        }
            
        if (enable && !isHighlighted && originalMaterial != null)
        {
            // Store original material and apply highlight
            objectRenderer.material = highlightMaterial;
            isHighlighted = true;
        }
        else if (!enable && isHighlighted && originalMaterial != null)
        {
            // Restore original material
            objectRenderer.material = originalMaterial;
            isHighlighted = false;
        }
    }
    
    /// <summary>
    /// Show description popup (without the name)
    /// </summary>
    private void ShowDescriptionPopup()
    {
        // Don't hide hover name here - let SimpleObjectUI handle the fade out
        if (SimpleObjectUI.Instance != null && !string.IsNullOrEmpty(objectDescription))
        {
            Vector3 descPosition = transform.position + new Vector3(0, 2.5f, 0);
            SimpleObjectUI.Instance.ShowDescription(objectDescription, descPosition);
        }
        else
        {
            Debug.LogWarning("Cannot show description: SimpleObjectUI.Instance is null or objectDescription is empty");
        }
    }
    
    /// <summary>
    /// Hide the current text popup if it exists
    /// </summary>
    private void HideTextPopup()
    {
        if (SimpleObjectUI.Instance != null)
        {
            SimpleObjectUI.Instance.HideDescription();
        }
    }
    
    /// <summary>
    /// Set object information from database/CSV (future implementation)
    /// </summary>
    public void SetObjectData(string name, string description, string id = "")
    {
        objectName = name;
        objectDescription = description;
        objectId = id;
    }
    
    /// <summary>
    /// Load object data from ID using the database system
    /// </summary>
    public void LoadFromId(string id)
    {
        objectId = id;
        
        if (ObjectDatabase.Instance != null)
        {
            ObjectDatabase.ObjectData data = ObjectDatabase.Instance.GetObjectDataById(id);
            
            if (data != null)
            {
                // Use data from database
                objectName = data.name;
                objectDescription = data.description;
                Debug.Log("Loaded data for ID: " + id + " - " + objectName);
            }
            else
            {
                // Fall back to default values
                Debug.LogWarning("No database entry found for ID: " + id + ". Using default values.");
            }
        }
        else
        {
            Debug.LogWarning("ObjectDatabase not found. Using default values.");
        }
    }
}