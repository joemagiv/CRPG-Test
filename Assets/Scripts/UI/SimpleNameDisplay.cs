using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Simple name display that follows world objects
/// </summary>
public class SimpleNameDisplay : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public RectTransform panelRect;
    
    private Camera mainCamera;
    private Vector3 targetWorldPosition;
    private bool isInitialized = false;
    private float showDelay = 0.1f; // Small delay to prevent initial snap
    private float delayTimer = 0f;
    private bool isFadingOut = false;
    private float fadeOutTimer = 0f;
    private float fadeOutDuration = 0.5f;
    
    void Start()
    {
        mainCamera = Camera.main;
        // Start active but invisible
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
    }
    
    void Update()
    {
        // Handle fade out if triggered
        if (isFadingOut)
        {
            fadeOutTimer += Time.deltaTime;
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f - Mathf.Min(1f, fadeOutTimer / fadeOutDuration);
                
                if (fadeOutTimer >= fadeOutDuration)
                {
                    Destroy(gameObject);
                }
            }
            return;
        }
        
        // Handle show delay using alpha fade
        if (!isInitialized)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= showDelay)
            {
                isInitialized = true;
            }
            
            // Fade in during delay
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Min(1f, delayTimer / showDelay);
            }
        }
        
        if (mainCamera != null)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition);
            
            if (screenPosition.z > 0) // Only update if in front of camera
            {
                screenPosition.y += 30f; // Offset above object
                panelRect.position = screenPosition;
            }
        }
    }
    
    public void Setup(string name, Vector3 worldPosition)
    {
        targetWorldPosition = worldPosition;
        isInitialized = false;
        delayTimer = 0f;
        
        if (textComponent != null)
        {
            textComponent.text = name;
            textComponent.fontSize = 20; // Larger font size
        }
        
        // Set initial position immediately
        if (mainCamera != null)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition);
            if (screenPosition.z > 0)
            {
                screenPosition.y += 30f;
                panelRect.position = screenPosition;
            }
        }
        
        // Ensure we have a CanvasGroup for fading
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0f;
        
        Debug.Log($"Name display setup: {name} at {worldPosition}");
    }
    
    /// <summary>
    /// Trigger fade out and destruction
    /// </summary>
    public void FadeOutAndDestroy()
    {
        isFadingOut = true;
        fadeOutTimer = 0f;
        Debug.Log("Name display fade out started");
    }
}