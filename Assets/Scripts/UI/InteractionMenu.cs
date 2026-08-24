using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Interaction menu that shows Inspect, Use, and Talk buttons over clicked objects.
/// Uses he same pattern as SimpleObjectUI + SimpleDescriptionPopup:
/// Canvas root + child Panel (with set sizeDelta), positioned via panelRect.position.
/// </summary>
public class InteractionMenu : MonoBehaviour
{
    public static InteractionMenu Instance { get; private set; }
    
    private GameObject menuGO;          // Canvas root (inactive, full-screen canvas)
    private RectTransform panelRect;    // The child Panel we position via .position
    
    private Button inspectButton;
    private Button useButton;
    private Button talkButton;
    private TextMeshProUGUI inspectButtonText;
    private TextMeshProUGUI useButtonText;
    private TextMeshProUGUI talkButtonText;
    
    private ClickableObject currentTarget;
    private Vector3 targetWorldPosition;
    private Camera mainCamera;
    private bool isMenuActive = false;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
    
    void Start() => mainCamera = Camera.main;
    
    /// <summary> Build a fresh canvas+panel+buttons (same structure as SimpleDescriptionPopup) </summary>
    private void BuildMenu()
    {
        menuGO = new GameObject("InteractionMenuCanvas");
        
        Canvas canvas = menuGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 2001; // above SimpleDescriptionPopup (1001)
        
        CanvasScaler scaler = menuGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        menuGO.AddComponent<GraphicRaycaster>();
        
        // Make canvas RectTransform full-screen (like SimpleDescriptionPopup does implicitly)
        RectTransform canvasRT = menuGO.GetComponent<RectTransform>();
        canvasRT.anchorMin = Vector2.zero;
        canvasRT.anchorMax = Vector2.one;
        canvasRT.offsetMin = Vector2.zero;
        canvasRT.offsetMax = Vector2.zero;
        canvasRT.anchoredPosition = Vector2.zero;
        
        // Child panel - this is the sized, positioned element (EXACT pattern as description popup)
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(menuGO.transform); // no "false" arg — match SimpleObjectUI
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
        
        panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(300, 200); // same as description popup
        panelRect.anchoredPosition = Vector2.zero;
        
        // Buttons as children of the panel
        CreateButton("InspectButton", "Inspect", 55, panel.transform);
        CreateButton("UseButton", "Use", -5, panel.transform);
        CreateButton("TalkButton", "Talk", -65, panel.transform);
        
        menuGO.SetActive(false);
    }
    
    private void CreateButton(string name, string label, float yOffset, Transform parent)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        
        RectTransform btnRT = btnObj.AddComponent<RectTransform>();
        btnRT.sizeDelta = new Vector2(260, 35);
        btnRT.anchoredPosition = new Vector2(0, yOffset);
        
        Image btnImg = btnObj.AddComponent<Image>();
        btnImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnImg;
        
        ColorBlock cb = new ColorBlock();
        cb.normalColor = new Color(0.2f, 0.2f, 0.2f, 1f);
        cb.highlightedColor = new Color(0.35f, 0.35f, 0.35f, 1f);
        cb.pressedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        cb.disabledColor = new Color(0.1f, 0.1f, 0.1f, 0.5f);
        cb.colorMultiplier = 1f;
        cb.fadeDuration = 0.1f;
        btn.colors = cb;
        
        GameObject txtObj = new GameObject("Label");
        txtObj.transform.SetParent(btnObj.transform, false);
        
        TextMeshProUGUI txt = txtObj.AddComponent<TextMeshProUGUI>();
        txt.text = label;
        txt.fontSize = 18;
        txt.color = Color.white;
        txt.alignment = TextAlignmentOptions.Center;
        txt.raycastTarget = false;
        
        RectTransform txtRT = txt.GetComponent<RectTransform>();
        txtRT.sizeDelta = new Vector2(250, 30);
        txtRT.anchoredPosition = Vector2.zero;
        
        if (name == "InspectButton") { inspectButton = btn; inspectButtonText = txt; btn.onClick.AddListener(OnInspectClicked); }
        else if (name == "UseButton") { useButton = btn; useButtonText = txt; btn.onClick.AddListener(OnUseClicked); }
        else if (name == "TalkButton") { talkButton = btn; talkButtonText = txt; btn.onClick.AddListener(OnTalkClicked); }
    }
    
    public void ShowMenu(ClickableObject target, Vector3 worldPosition)
    {
        if (target == null) return;
        
        HideMenu(); // cleanup previous
        
        currentTarget = target;
        targetWorldPosition = worldPosition;
        if (mainCamera == null) mainCamera = Camera.main;
        
        BuildMenu();
        
        if (inspectButtonText != null) inspectButtonText.text = target.inspectText;
        if (useButtonText != null) useButtonText.text = target.useText;
        if (talkButtonText != null) talkButtonText.text = target.talkText;
        
        PositionOverObject();
        menuGO.SetActive(true);
        isMenuActive = true;
        
        Debug.Log("[InteractionMenu] Menu shown over " + target.objectName);
    }
    
    private void PositionOverObject()
    {
        if (mainCamera == null || panelRect == null) return;
        
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetWorldPosition);
        screenPos.y -= 70; // well below the object, below the nameplate
        panelRect.position = screenPos; // move the child Panel (same as SimpleDescriptionPopup)
    }
    
    public void HideMenu()
    {
        if (menuGO != null) Destroy(menuGO);
        menuGO = null;
        panelRect = null;
        isMenuActive = false;
        currentTarget = null;
    }
    
    private void OnInspectClicked()
    {
        if (currentTarget != null)
        {
            string msg = string.IsNullOrEmpty(currentTarget.inspectResult)
                ? currentTarget.objectDescription : currentTarget.inspectResult;
            ShowDescription(msg);
        }
        HideMenu();
    }
    
    private void OnUseClicked()
    {
        if (currentTarget != null)
        {
            if (currentTarget.isPickupable)
            {
                if (InventoryManager.Instance != null)
                {
                    InventoryManager.Instance.AddItem(currentTarget.itemId, currentTarget.objectName,
                        currentTarget.objectDescription, currentTarget.itemQuantity);
                    ShowDescription("Added " + currentTarget.objectName + " to inventory!");
                }
                currentTarget.gameObject.SetActive(false);
            }
            else
            {
                ShowDescription(string.IsNullOrEmpty(currentTarget.useResult)
                    ? "Nothing happens." : currentTarget.useResult);
            }
        }
        HideMenu();
    }
    
    private void OnTalkClicked()
    {
        if (currentTarget != null)
        {
            // If the target has a Dialogue System trigger, start its conversation
            // (this is set up automatically for NPCs via NPCController/Awake).
            if (currentTarget.startConversationOnTalk && currentTarget.StartConversation())
            {
                // Conversation starting; menu will close as a side effect.
            }
            else
            {
                ShowDescription(string.IsNullOrEmpty(currentTarget.talkResult)
                    ? "The object doesn't respond." : currentTarget.talkResult);
            }
        }
        HideMenu();
    }
    
    private void ShowDescription(string msg)
    {
        if (SimpleObjectUI.Instance != null && currentTarget != null)
        {
            SimpleObjectUI.Instance.ShowDescription(msg, currentTarget.transform.position + new Vector3(0, 2.5f, 0));
        }
    }
    
    void Update()
    {
        if (!isMenuActive) return;
        
        if (currentTarget != null && mainCamera != null)
            PositionOverObject();
        
        if (Input.GetKeyDown(KeyCode.Escape))
            HideMenu();
    }
    
    void OnDestroy()
    {
        if (menuGO != null) Destroy(menuGO);
    }
}