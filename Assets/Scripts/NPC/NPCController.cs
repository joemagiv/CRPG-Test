using UnityEngine;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Basic NPC controller that handles dialogue interaction.
/// Supports both click-to-talk (OnMouseDown) and proximity use (Selector / OnUse).
/// Also shows a world-anchored hover highlight + name box (like ClickableObject)
/// instead of the Dialogue System Selector's top-screen default GUI.
/// </summary>
public class NPCController : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcName = "Generic NPC";
    public Color dialogueColor = Color.white;
    public string conversationEntry = "";

    [Header("Visual Settings")]
    public GameObject visualIndicator;
    public float interactionRange = 5f;

    [Header("Hover Highlight")]
    [Tooltip("Material applied to the NPC while hovered.")]
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer objectRenderer;
    private bool isHighlighted = false;

    [Header("Interaction")]
    [Tooltip("If true, clicking the NPC starts the conversation directly.")]
    public bool clickToTalk = true;

    private DialogueSystemTrigger dialogueTrigger;
    private Usable usableComponent;
    private Transform cachedPlayer;

    void Awake()
    {
        dialogueTrigger = GetComponent<DialogueSystemTrigger>() ?? gameObject.AddComponent<DialogueSystemTrigger>();
        usableComponent = GetComponent<Usable>() ?? gameObject.AddComponent<Usable>();

        ConfigureDialogueTrigger();
        ConfigureUsableComponent();

        // Prepare reusable highlight material (falls back to ClickableObject's).
        if (highlightMaterial == null)
        {
            highlightMaterial = MaterialSetup.GetHighlightMaterial();
        }

        objectRenderer = GetComponentInChildren<Renderer>();
        if (objectRenderer != null && objectRenderer.material != null)
        {
            originalMaterial = objectRenderer.material;
        }

        // The Dialogue System Selector draws a duplicate name box at the top of
        // the screen. We render our own world-anchored name box instead, so turn
        // the legacy default GUI off on the player's Selector.
        DisableSelectorDefaultGUI();
    }

    // Fallback: ensure the conversation is wired even if Awake didn't apply it
    // (e.g. prefab instance or script ordering issues).
    void Start()
    {
        if (dialogueTrigger != null && string.IsNullOrEmpty(dialogueTrigger.conversation))
        {
            ConfigureDialogueTrigger();
        }

        // The player may not exist yet at Awake time; retry here.
        DisableSelectorDefaultGUI();
    }

    void ConfigureDialogueTrigger()
    {
        if (dialogueTrigger == null) return;

        dialogueTrigger.trigger = DialogueSystemTriggerEvent.OnUse;
        dialogueTrigger.conversation = conversationEntry;
        // Make this NPC the conversant so speaker info (and thus Disco Elysium
        // name/color) resolves to this controller.
        dialogueTrigger.conversationConversant = transform;
    }

    void ConfigureUsableComponent()
    {
        if (usableComponent == null) return;

        usableComponent.overrideName = npcName;
        usableComponent.overrideUseMessage = "Talk";
        usableComponent.maxUseDistance = interactionRange;
    }

    /// <summary>
    /// Disables the Dialogue System Selector's legacy top-screen GUI on the player
    /// so it doesn't draw a duplicate name box. Our world-anchored box is used instead.
    /// </summary>
    private void DisableSelectorDefaultGUI()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        var selector = player.GetComponent<Selector>();
        if (selector != null)
        {
            selector.useDefaultGUI = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

    void OnMouseEnter()
    {
        if (DialogueManager.IsConversationActive) return;

        // Ensure the Selector's duplicate top-screen box stays off.
        DisableSelectorDefaultGUI();

        SetHighlight(true);
        ShowHoverName();
    }

    void OnMouseExit()
    {
        SetHighlight(false);

        // Keep the name box if a description/popup is active; otherwise hide it.
        if (SimpleObjectUI.Instance == null || !SimpleObjectUI.Instance.IsDescriptionActive)
        {
            HideHoverName();
        }
    }

    /// <summary>
    /// Click-to-talk: starts the conversation when the player clicks the NPC.
    /// When <see cref="clickToTalk"/> is false the click is handled by the
    /// ClickableObject component on the same object, which shows the Interact
    /// menu (Inspect / Use / Talk) first.
    /// </summary>
    void OnMouseDown()
    {
        if (!clickToTalk) return;
        if (string.IsNullOrEmpty(conversationEntry)) return;
        if (DialogueManager.IsConversationActive) return;

        if (dialogueTrigger != null)
        {
            dialogueTrigger.OnUse(GetPlayerTransform());
        }
        else
        {
            DialogueManager.StartConversation(conversationEntry);
        }
    }

    /// <summary>
    /// Show the NPC name in a world-anchored box above the NPC (via SimpleObjectUI).
    /// </summary>
    private void ShowHoverName()
    {
        if (SimpleObjectUI.Instance != null && !string.IsNullOrEmpty(npcName))
        {
            Vector3 hoverPosition = transform.position + new Vector3(0, 2f, 0);
            SimpleObjectUI.Instance.ShowName(npcName, hoverPosition);
        }
    }

    private void HideHoverName()
    {
        if (SimpleObjectUI.Instance != null)
        {
            SimpleObjectUI.Instance.HideName();
        }
    }

    /// <summary>
    /// Enable/disable the hover highlight on the NPC's renderer.
    /// </summary>
    private void SetHighlight(bool enable)
    {
        if (objectRenderer == null || highlightMaterial == null) return;

        if (originalMaterial == null && objectRenderer.material != null)
        {
            originalMaterial = objectRenderer.material;
        }

        if (enable && !isHighlighted && originalMaterial != null)
        {
            objectRenderer.material = highlightMaterial;
            isHighlighted = true;
        }
        else if (!enable && isHighlighted && originalMaterial != null)
        {
            objectRenderer.material = originalMaterial;
            isHighlighted = false;
        }
    }

    private Transform GetPlayerTransform()
    {
        if (cachedPlayer == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            cachedPlayer = playerObj != null ? playerObj.transform : null;
        }
        return cachedPlayer;
    }

    /// <summary>
    /// Set the conversation for this NPC.
    /// </summary>
    public void SetConversation(string conversationTitle)
    {
        conversationEntry = conversationTitle;
        if (dialogueTrigger != null)
        {
            dialogueTrigger.conversation = conversationTitle;
        }
    }

    /// <summary>
    /// Set the NPC name.
    /// </summary>
    public void SetNPCName(string name)
    {
        npcName = name;
        if (usableComponent != null)
        {
            usableComponent.overrideName = name;
        }
    }

    /// <summary>
    /// Set the dialogue color for this NPC.
    /// </summary>
    public void SetDialogueColor(Color color)
    {
        dialogueColor = color;
    }
}
