using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Helper script to set up the Disco Elysium style dialogue UI
/// </summary>
public class DialogueUISetup : MonoBehaviour
{
    public RightPanelDialogueUI dialogueUI;
    
    void Start()
    {
        // This script can be used to initialize the dialogue UI
        // The actual UI should be set up in the Unity Editor
    }
    
    /// <summary>
    /// Create a basic response button prefab
    /// </summary>
    public static GameObject CreateResponseButtonPrefab()
    {
        // Create button object
        GameObject buttonObj = new GameObject("ResponseButton");
        
        // Add RectTransform
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 60);
        
        // Add Image for background
        Image background = buttonObj.AddComponent<Image>();
        background.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        // Add Button component
        Button button = buttonObj.AddComponent<Button>();
        
        // Create text container
        GameObject textContainer = new GameObject("TextContainer");
        textContainer.transform.SetParent(buttonObj.transform);
        
        RectTransform textRect = textContainer.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(40, 0);
        textRect.offsetMax = new Vector2(-10, 0);
        
        // Add response text
        TextMeshProUGUI responseText = textContainer.AddComponent<TextMeshProUGUI>();
        responseText.fontSize = 24;
        responseText.color = Color.white;
        responseText.alignment = TextAlignmentOptions.Left;
        responseText.enableWordWrapping = true;
        
        // Create shortcut container
        GameObject shortcutContainer = new GameObject("ShortcutContainer");
        shortcutContainer.transform.SetParent(buttonObj.transform);
        
        RectTransform shortcutRect = shortcutContainer.AddComponent<RectTransform>();
        shortcutRect.anchorMin = Vector2.zero;
        shortcutRect.anchorMax = Vector2.zero;
        shortcutRect.sizeDelta = new Vector2(30, 30);
        shortcutRect.anchoredPosition = new Vector2(10, -15);
        
        // Add shortcut text
        TextMeshProUGUI shortcutText = shortcutContainer.AddComponent<TextMeshProUGUI>();
        shortcutText.fontSize = 24;
        shortcutText.color = Color.yellow;
        shortcutText.alignment = TextAlignmentOptions.Center;
        
        // Add DiscoElysiumResponseButton component
        DiscoElysiumResponseButton responseButton = buttonObj.AddComponent<DiscoElysiumResponseButton>();
        responseButton.responseText = responseText;
        responseButton.shortcutText = shortcutText;
        responseButton.button = button;
        
        return buttonObj;
    }
}