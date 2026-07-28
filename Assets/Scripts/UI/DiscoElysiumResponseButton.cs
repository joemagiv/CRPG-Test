using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Custom response button for Disco Elysium style dialogue
/// </summary>
public class DiscoElysiumResponseButton : MonoBehaviour
{
    public TextMeshProUGUI responseText;
    public TextMeshProUGUI shortcutText;
    public Button button;
    
    public Response response;
    private int index;
    private RightPanelDialogueUI dialogueUI;
    
    public void Setup(Response response, int index, RightPanelDialogueUI dialogueUI)
    {
        this.response = response;
        this.index = index;
        this.dialogueUI = dialogueUI;
        
        // Set response text
        if (responseText != null)
        {
            responseText.text = response.formattedText.text;
        }
        
        // Set shortcut text (1, 2, 3, etc.)
        if (shortcutText != null)
        {
            shortcutText.text = (index + 1).ToString() + ".";
        }
        
        // Add click listener
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }
    
    public void OnClick()
    {
        if (dialogueUI != null)
        {
            dialogueUI.SelectResponse(index);
        }
    }
    
    /// <summary>
    /// Highlight this button
    /// </summary>
    public void Highlight(bool highlight)
    {
        if (button != null)
        {
            ColorBlock colors = button.colors;
            if (highlight)
            {
                colors.normalColor = new Color(0.8f, 0.8f, 0.8f);
            }
            else
            {
                colors.normalColor = Color.white;
            }
            button.colors = colors;
        }
    }
}