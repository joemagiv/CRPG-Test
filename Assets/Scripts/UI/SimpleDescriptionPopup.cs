using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Simple description popup that follows world objects
/// </summary>
public class SimpleDescriptionPopup : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public RectTransform panelRect;
    public float lifetime = 5f;

    // Linked name display so the description can sit directly below it
    public SimpleNameDisplay linkedNameDisplay;

    private Camera mainCamera;
    private Vector3 targetWorldPosition;
    private CanvasGroup canvasGroup;

    private float fadeInDuration = 0.3f;
    private float fadeOutDuration = 0.5f;
    private bool fadingOut = false;

    void Start()
    {
        mainCamera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        StartCoroutine(LifetimeRoutine());
    }

    void Update()
    {
        // Fade in on appearance
        if (!fadingOut && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = Mathf.Min(1f, canvasGroup.alpha + Time.deltaTime / fadeInDuration);
        }

        if (mainCamera == null)
            return;

        Vector3 screenPosition;

        // Place the description directly below the linked name display so they
        // never overlap. Fall back to the world anchor if no name is linked.
        if (linkedNameDisplay != null && linkedNameDisplay.panelRect != null)
        {
            Vector2 nameCenter = linkedNameDisplay.panelRect.position;
            float nameHeight = linkedNameDisplay.panelRect.sizeDelta.y;
            float gap = 10f;
            float descHeight = panelRect.sizeDelta.y;

            screenPosition = new Vector3(
                nameCenter.x,
                nameCenter.y - (nameHeight * 0.5f + gap + descHeight * 0.5f),
                0f);
        }
        else
        {
            screenPosition = mainCamera.WorldToScreenPoint(targetWorldPosition);

            if (screenPosition.z > 0)
            {
                screenPosition.y -= 50f; // Offset below name
            }
        }

        panelRect.position = screenPosition;
    }

    public void Setup(string description, Vector3 worldPosition)
    {
        targetWorldPosition = worldPosition;

        if (textComponent != null)
        {
            textComponent.text = description;
        }

        Debug.Log($"Description popup setup: {description} at {worldPosition}");
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime);

        // Start fading the name at the same moment so both disappear together
        SimpleObjectUI uiInstance = SimpleObjectUI.Instance;
        if (uiInstance != null)
        {
            uiInstance.DestroyNameDisplay();
        }

        fadingOut = true;
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Min(1f, elapsed / fadeOutDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}