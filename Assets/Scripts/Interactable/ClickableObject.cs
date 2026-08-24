using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

using PixelCrushers.DialogueSystem;

/// <summary>
/// Component that makes an object clickable with hover highlighting and text display.
/// When a DialogueSystemTrigger is present on the same GameObject (or set via
/// <see cref="dialogueTrigger"/>), the "Talk" interaction launches that trigger's
/// conversation.
/// </summary>
[RequireComponent(typeof(Collider))]
public class ClickableObject : MonoBehaviour
{
    // Static reference to last clicked object for movement prevention
    public static ClickableObject lastClickedObject = null;

    // Reference to the Dialogue System trigger that handles this object's conversation.
    // Optional; wired automatically if a DialogueSystemTrigger is on this object.
    public DialogueSystemTrigger dialogueTrigger;
    
    [Header("Display Settings")]
    public string objectName = "Object";
    public string objectDescription = "This is a clickable object.";
    public string objectId = ""; // For future database/CSV integration

    [Header("Interaction Options")]
    public string inspectText = "Inspect";
    public string useText = "Use";
    public string talkText = "Talk";
    public string inspectResult = ""; // What happens when inspected (default to objectDescription if empty)
    public string useResult = ""; // What happens when used
    public string talkResult = ""; // What happens when talked to
    
    [Header("Item Properties")]
    public bool isPickupable = false; // Can this item be picked up?
    public string itemId = ""; // Unique identifier for inventory system
    public int itemQuantity = 1; // Quantity if stackable
    
    [Header("Highlight Settings")]
    public Material highlightMaterial;
    public Material originalMaterial;
    public float highlightOutlineWidth = 0.02f;
    public Color highlightOutlineColor = new Color(0f, 0.8f, 1f, 1f);
    
    [Header("Text Display Settings")]
    public float textDisplayDuration = 5f;

    // When true, the Talk interaction launches the dialogue conversation
    // via the Dialogue System trigger instead of showing talkResult text.
    public bool startConversationOnTalk = false;

    private Renderer objectRenderer;
    private bool isHighlighted = false;
    private Camera mainCamera;

    // --- New Input System support ------------------------------------------------
    // Projects using UnityEngine.InputSystem (with InputSystemUIInputModule and no
    // legacy StandaloneInputModule) never receive OnMouseEnter/OnMouseExit/OnMouseDown.
    // A single static Update polls the mouse each frame, raycasts from the camera,
    // and forwards hover/click events to the ClickableObject under the cursor.
    private static Camera cachedMainCamera;
    private static ClickableObject currentHover;
    private static bool mainCameraDirty = true;
    private static ClickableObject activeUpdater = null;
    // --- end New Input System support -------------------------------------------

    void Awake()
    {
        // Get renderer component (root first, then children for rigged models)
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null) objectRenderer = GetComponentInChildren<Renderer>();

        // Get or create highlight material
        if (highlightMaterial == null)
        {
            highlightMaterial = MaterialSetup.GetHighlightMaterial();
        }

        // Get main camera
        mainCamera = Camera.main;
        if (mainCamera == null) mainCamera = Camera.main;

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
        
        // Ensure InteractionMenu singleton exists
        if (InteractionMenu.Instance == null)
        {
            GameObject interactionMenu = new GameObject("InteractionMenu");
            interactionMenu.AddComponent<InteractionMenu>();
            Debug.Log("Created InteractionMenu instance");
        }
        
        // Ensure InventoryManager singleton exists
        if (InventoryManager.Instance == null)
        {
            GameObject inventoryManager = new GameObject("InventoryManager");
            inventoryManager.AddComponent<InventoryManager>();
            Debug.Log("Created InventoryManager instance");
        }

        Debug.Log($"ClickableObject initialized on {gameObject.name}. Renderer: {objectRenderer != null}, Material: {highlightMaterial != null}");

        // Auto-wire the Dialogue System trigger if one is on this object.
        if (dialogueTrigger == null) dialogueTrigger = GetComponent<DialogueSystemTrigger>();
        if (dialogueTrigger != null) startConversationOnTalk = true;
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
            
        Debug.Log("OnMouseDown called - showing interaction menu");
        
        // Show interaction menu instead of description popup
        if (InteractionMenu.Instance != null)
        {
            Vector3 menuPosition = transform.position + new Vector3(0, 2f, 0);
            InteractionMenu.Instance.ShowMenu(this, menuPosition);
        }
        
        // Prevent movement when clicking on objects
        ConsumeMouseClick();
    }

    // --- New Input System polling ------------------------------------------------
    // Every frame, if this is the first ClickableObject awake, refresh the main
    // camera reference and run a hover/click poll. Only one ClickableObject ever
    // updates; the static method is invoked from whichever instance is present.
    void Update()
    {
        if (activeUpdater != this) return;
        // Only poll when the legacy OnMouse* path is unavailable. If a legacy
        // StandaloneInputModule is present, Unity's OnMouseDown/OnMouseEnter/
        // OnMouseExit fire natively and we don't need to poll.
        if (HasLegacyInputModule()) return;
        PollNewInputSystem();
    }

    private static bool HasLegacyInputModule()
    {
        EventSystem es = EventSystem.current;
        if (es == null) return false;
        return es.GetComponent<StandaloneInputModule>() != null;
    }

    void OnEnable()
    {
        TryElectUpdater();
    }

    void OnDisable()
    {
        if (activeUpdater == this)
        {
            activeUpdater = null;
            TryElectUpdater();
        }
    }

    private static void TryElectUpdater()
    {
        if (activeUpdater != null) return;
        ClickableObject[] all = Object.FindObjectsByType<ClickableObject>(FindObjectsSortMode.None);
        foreach (var c in all)
        {
            if (c != null && c.isActiveAndEnabled)
            {
                activeUpdater = c;
                break;
            }
        }
    }

    /// <summary>
    /// Static poll that handles hover enter/exit and click forwarding when the
    /// project uses the new Unity Input System. Legacy OnMouse* callbacks are
    /// invoked by Unity directly only under the legacy input module, so this
    /// bridges that gap without requiring a scene-level input manager.
    /// </summary>
    private void PollNewInputSystem()
    {
        if (InputSystemCompatibility.IsPointerOverUI())
        {
            ClearHover();
            return;
        }

        if (mainCameraDirty || cachedMainCamera == null)
        {
            cachedMainCamera = Camera.main;
            mainCameraDirty = false;
        }

        if (cachedMainCamera == null) return;

        ClickableObject hitObject = null;
        Mouse currentMouse = Mouse.current;
        if (currentMouse != null)
        {
            Ray ray = cachedMainCamera.ScreenPointToRay(currentMouse.position.ReadValue());
            RaycastHit hit;
            // LayerMask of ~0 catches the Default layer the NPC is on.
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0))
            {
                hitObject = hit.collider.GetComponentInParent<ClickableObject>();
            }
        }
        else
        {
            // Fallback: legacy mouse for the editor / no-input-system path.
            Vector3 mousePos = Input.mousePosition;
            Ray ray = cachedMainCamera.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitObject = hit.collider.GetComponentInParent<ClickableObject>();
            }
        }

        if (hitObject != currentHover)
        {
            if (currentHover != null) currentHover.OnMouseExitInputSystem();
            currentHover = hitObject;
            if (currentHover != null) currentHover.OnMouseEnterInputSystem();
        }

        if (currentMouse != null && currentMouse.leftButton.wasPressedThisFrame)
        {
            if (currentHover != null)
            {
                currentHover.OnMouseDownInputSystem();
            }
        }
    }

    private static void ClearHover()
    {
        if (currentHover != null)
        {
            currentHover.OnMouseExitInputSystem();
            currentHover = null;
        }
    }
    // --- end New Input System polling -------------------------------------------

    /// <summary>
    /// Input-System-facing hover enter. Mirrors OnMouseEnter for projects using
    /// the new Unity Input System (which doesn't invoke OnMouseEnter).
    /// </summary>
    public void OnMouseEnterInputSystem()
    {
        if (ShouldIgnoreMouseEvent())
            return;

        SetHighlight(true);
        ShowHoverName();
    }

    /// <summary>
    /// Input-System-facing hover exit. Mirrors OnMouseExit for projects using
    /// the new Unity Input System.
    /// </summary>
    public void OnMouseExitInputSystem()
    {
        if (ShouldIgnoreMouseEvent())
            return;

        SetHighlight(false);
        Debug.Log("OnMouseExit (input system) called - hiding hover name");

        if (SimpleObjectUI.Instance == null || !SimpleObjectUI.Instance.IsDescriptionActive)
        {
            HideHoverName();
        }
        else
        {
            Debug.Log("Not hiding name - description is being shown");
        }
    }
    
    /// <summary>
    /// Input-System-facing click. Mirrors OnMouseDown for projects using the new
    /// Unity Input System. Shows the Interaction menu over this object.
    /// Returns true if the click was handled.
    /// </summary>
    public bool OnMouseDownInputSystem()
    {
        if (ShouldIgnoreMouseEvent())
            return false;

        // Don't show the Interact menu while a conversation is already active.
        if (PixelCrushers.DialogueSystem.DialogueManager.IsConversationActive)
            return false;

        Debug.Log("OnMouseDown (input system) called - showing interaction menu");

        if (InteractionMenu.Instance != null)
        {
            Vector3 menuPosition = transform.position + new Vector3(0, 2f, 0);
            InteractionMenu.Instance.ShowMenu(this, menuPosition);
        }

        ConsumeMouseClick();
        return true;
    }

    // --- New Input System polling ------------------------------------------------
    
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
    /// Start the conversation associated with this object's Dialogue System trigger,
    /// if one is present. Returns true if a conversation was started.
    /// </summary>
    public bool StartConversation()
    {
        if (dialogueTrigger != null && dialogueTrigger.enabled)
        {
            // OnUse(Transform) starts the trigger's conversation when trigger == OnUse.
            dialogueTrigger.OnUse((Transform)null);
            return true;
        }
        return false;
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