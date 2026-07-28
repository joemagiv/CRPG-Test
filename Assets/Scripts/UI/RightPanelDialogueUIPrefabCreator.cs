using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Creates a basic right panel dialogue UI prefab programmatically
/// This is for demonstration - in practice you'd set this up in the Unity Editor
/// </summary>
public class RightPanelDialogueUIPrefabCreator : MonoBehaviour
{
    public GameObject CreateDialogueUI()
    {
        // Create main canvas
        GameObject canvasObj = new GameObject("DialogueCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create dialogue panel (right side black band)
        GameObject panelObj = new GameObject("DialoguePanel");
        panelObj.transform.SetParent(canvasObj.transform);
        
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(1, 0.5f);
        panelRect.sizeDelta = new Vector2(500, 800);
        panelRect.anchoredPosition = new Vector2(-50, 0);
        
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f);
        
        // Create NPC name text
        GameObject nameObj = new GameObject("NPCNameText");
        nameObj.transform.SetParent(panelObj.transform);
        
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0.5f, 1);
        nameRect.anchorMax = new Vector2(0.5f, 1);
        nameRect.pivot = new Vector2(0.5f, 1);
        nameRect.sizeDelta = new Vector2(400, 40);
        nameRect.anchoredPosition = new Vector2(0, -20);
        
        TextMeshProUGUI nameText = nameObj.AddComponent<TextMeshProUGUI>();
        nameText.fontSize = 28;
        nameText.color = Color.white;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.text = "NPC Name";
        
        // Create dialogue text area
        GameObject dialogueObj = new GameObject("DialogueText");
        dialogueObj.transform.SetParent(panelObj.transform);
        
        RectTransform dialogueRect = dialogueObj.AddComponent<RectTransform>();
        dialogueRect.anchorMin = new Vector2(0.05f, 0.8f);
        dialogueRect.anchorMax = new Vector2(0.95f, 0.95f);
        dialogueRect.pivot = new Vector2(0.5f, 1);
        dialogueRect.offsetMin = Vector2.zero;
        dialogueRect.offsetMax = Vector2.zero;
        
        TextMeshProUGUI dialogueText = dialogueObj.AddComponent<TextMeshProUGUI>();
        dialogueText.fontSize = 24;
        dialogueText.color = Color.white;
        dialogueText.alignment = TextAlignmentOptions.TopLeft;
        dialogueText.enableWordWrapping = true;
        dialogueText.text = "This is where the dialogue text will appear.";
        
        // Create response options container
        GameObject responsesObj = new GameObject("ResponseOptions");
        responsesObj.transform.SetParent(panelObj.transform);
        
        RectTransform responsesRect = responsesObj.AddComponent<RectTransform>();
        responsesRect.anchorMin = new Vector2(0, 0);
        responsesRect.anchorMax = new Vector2(1, 0.7f);
        responsesRect.pivot = new Vector2(0.5f, 0);
        responsesRect.offsetMin = new Vector2(20, 20);
        responsesRect.offsetMax = new Vector2(-20, -20);
        
        VerticalLayoutGroup layoutGroup = responsesObj.AddComponent<VerticalLayoutGroup>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.spacing = 10;
        layoutGroup.padding = new RectOffset(5, 5, 5, 5);
        
        // Add RightPanelDialogueUI component
        RightPanelDialogueUI dialogueUI = canvasObj.AddComponent<RightPanelDialogueUI>();
        dialogueUI.dialoguePanel = panelObj;
        dialogueUI.npcNameText = nameText;
        dialogueUI.dialogueText = dialogueText;
        dialogueUI.responseOptionsContainer = responsesObj.transform;
        
        // Create response button prefab
        GameObject buttonPrefab = CreateResponseButtonPrefab();
        dialogueUI.responseButtonPrefab = buttonPrefab;
        
        // Disable the panel initially
        panelObj.SetActive(false);
        
        return canvasObj;
    }
    
    GameObject CreateResponseButtonPrefab()
    {
        GameObject buttonObj = new GameObject("ResponseButton");
        
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(460, 60);
        
        Image background = buttonObj.AddComponent<Image>();
        background.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        Button button = buttonObj.AddComponent<Button>();
        
        // Create text container
        GameObject textContainer = new GameObject("TextContainer");
        textContainer.transform.SetParent(buttonObj.transform);
        
        RectTransform textRect = textContainer.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(40, 0);
        textRect.offsetMax = new Vector2(-10, 0);
        
        TextMeshProUGUI responseText = textContainer.AddComponent<TextMeshProUGUI>();
        responseText.fontSize = 24;
        responseText.color = Color.white;
        responseText.alignment = TextAlignmentOptions.Left;
        responseText.enableWordWrapping = true;
        responseText.text = "Response text goes here";
        
        // Create shortcut container
        GameObject shortcutContainer = new GameObject("ShortcutContainer");
        shortcutContainer.transform.SetParent(buttonObj.transform);
        
        RectTransform shortcutRect = shortcutContainer.AddComponent<RectTransform>();
        shortcutRect.anchorMin = Vector2.zero;
        shortcutRect.anchorMax = Vector2.zero;
        shortcutRect.sizeDelta = new Vector2(30, 30);
        shortcutRect.anchoredPosition = new Vector2(10, -15);
        
        TextMeshProUGUI shortcutText = shortcutContainer.AddComponent<TextMeshProUGUI>();
        shortcutText.fontSize = 24;
        shortcutText.color = Color.yellow;
        shortcutText.alignment = TextAlignmentOptions.Center;
        shortcutText.text = "1.";
        
        // Add DiscoElysiumResponseButton component
        DiscoElysiumResponseButton responseButton = buttonObj.AddComponent<DiscoElysiumResponseButton>();
        responseButton.responseText = responseText;
        responseButton.shortcutText = shortcutText;
        responseButton.button = button;
        
        return buttonObj;
    }
}