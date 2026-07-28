using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

/// <summary>
/// Custom dialogue UI with a wide panel on the right side of the screen, in the
/// style of Disco Elysium. It implements IDialogueUI so the Dialogue System can
/// find and drive it through the Dialogue Manager's Display Settings.
/// </summary>
public class RightPanelDialogueUI : MonoBehaviour, IDialogueUI
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Transform responseOptionsContainer;
    public GameObject responseButtonPrefab;

    [Header("Styling")]
    public Color defaultNPCColor = Color.white;
    public float panelWidth = 500f;
    public float panelHeight = 800f;
    public float panelRightMargin = 50f;
    public float responseButtonHeight = 60f;

    [Header("Keyboard Shortcuts")]
    public KeyCode[] numberKeys = {
        KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6,
        KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0
    };

    // Required by IDialogueUI: fired when the player selects a response.
    public event EventHandler<SelectedResponseEventArgs> SelectedResponseHandler;

    private List<GameObject> activeResponseButtons = new List<GameObject>();
    private NPCController currentNPC;

    void Awake()
    {
        if (dialoguePanel == null) dialoguePanel = gameObject;
        SetupPanel();
    }

    void SetupPanel()
    {
        if (dialoguePanel == null) return;

        RectTransform panelRect = dialoguePanel.GetComponent<RectTransform>();
        if (panelRect != null)
        {
            // Fixed-size panel pinned to the right edge, vertically centered.
            // Both Y anchors are equal (0.5) so the panel keeps its sizeDelta
            // height instead of stretching to fill the parent.
            panelRect.anchorMin = new Vector2(1, 0.5f);
            panelRect.anchorMax = new Vector2(1, 0.5f);
            panelRect.pivot = new Vector2(1, 0.5f);
            panelRect.sizeDelta = new Vector2(panelWidth, panelHeight);
            panelRect.anchoredPosition = new Vector2(-panelRightMargin, 0);
        }

        Image panelImage = dialoguePanel.GetComponent<Image>();
        if (panelImage != null)
        {
            panelImage.color = new Color(0, 0, 0, 0.9f);
        }

        // NPC name pinned to the top-center of the panel.
        if (npcNameText != null)
        {
            RectTransform nameRect = npcNameText.GetComponent<RectTransform>();
            if (nameRect != null)
            {
                nameRect.anchorMin = new Vector2(0.5f, 1);
                nameRect.anchorMax = new Vector2(0.5f, 1);
                nameRect.pivot = new Vector2(0.5f, 1);
                nameRect.sizeDelta = new Vector2(panelWidth - 40, 50);
                nameRect.anchoredPosition = new Vector2(0, -30);
            }
        }

        // Dialogue text fills the region between the name and the responses.
        if (dialogueText != null)
        {
            RectTransform textRect = dialogueText.GetComponent<RectTransform>();
            if (textRect != null)
            {
                textRect.anchorMin = new Vector2(0, 0);
                textRect.anchorMax = new Vector2(1, 1);
                textRect.pivot = new Vector2(0.5f, 0.5f);
                textRect.offsetMin = new Vector2(20, 230);
                textRect.offsetMax = new Vector2(-20, -70);
            }
        }

        dialoguePanel.SetActive(false);
    }

    #region IDialogueUI implementation

    public void Open()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        ClearResponseButtons();
        currentNPC = null;
    }

    public void Close()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        ClearResponseButtons();
        currentNPC = null;
    }

    public void ShowSubtitle(Subtitle subtitle)
    {
        if (dialogueText == null) return;

        dialogueText.text = subtitle.formattedText.text;

        if (subtitle.speakerInfo != null && subtitle.speakerInfo.transform != null)
        {
            var npc = subtitle.speakerInfo.transform.GetComponent<NPCController>();
            if (npc != null)
            {
                currentNPC = npc;
                dialogueText.color = npc.dialogueColor;
                if (npcNameText != null)
                {
                    npcNameText.text = npc.npcName;
                    npcNameText.color = npc.dialogueColor;
                }
                return;
            }
        }

        dialogueText.color = defaultNPCColor;
        if (npcNameText != null) npcNameText.color = defaultNPCColor;
    }

    public void HideSubtitle(Subtitle subtitle) { }

    public void ShowResponses(Subtitle subtitle, Response[] responses, float timeout)
    {
        ClearResponseButtons();
        for (int i = 0; i < responses.Length; i++)
        {
            CreateResponseButton(responses[i], i);
        }
    }

    public void HideResponses()
    {
        ClearResponseButtons();
    }

    public void ShowAlert(string message, float duration)
    {
        if (dialogueText != null) dialogueText.text = message;
    }

    public void HideAlert() { }

    public void ShowQTEIndicator(int index) { }
    public void HideQTEIndicator(int index) { }

    #endregion

    void CreateResponseButton(Response response, int index)
    {
        if (responseButtonPrefab == null || responseOptionsContainer == null) return;

        GameObject buttonObj = Instantiate(responseButtonPrefab, responseOptionsContainer);
        buttonObj.SetActive(true);

        DiscoElysiumResponseButton button = buttonObj.GetComponent<DiscoElysiumResponseButton>();
        if (button == null)
        {
            button = buttonObj.AddComponent<DiscoElysiumResponseButton>();
        }

        button.Setup(response, index, this);

        RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
        if (buttonRect != null)
        {
            buttonRect.anchoredPosition = new Vector2(0, -index * responseButtonHeight);
            buttonRect.sizeDelta = new Vector2(panelWidth - 40, responseButtonHeight);
        }

        activeResponseButtons.Add(buttonObj);
    }

    void ClearResponseButtons()
    {
        foreach (var button in activeResponseButtons)
        {
            if (button != null) Destroy(button);
        }
        activeResponseButtons.Clear();
    }

    void Update()
    {
        if (activeResponseButtons.Count == 0) return;

        for (int i = 0; i < Mathf.Min(numberKeys.Length, activeResponseButtons.Count); i++)
        {
            if (Input.GetKeyDown(numberKeys[i]))
            {
                SelectResponse(i);
            }
        }
    }

    /// <summary>
    /// Select a response by index. Fires SelectedResponseHandler so the Dialogue
    /// System advances the conversation.
    /// </summary>
    public void SelectResponse(int index)
    {
        if (index < 0 || index >= activeResponseButtons.Count) return;

        var button = activeResponseButtons[index].GetComponent<DiscoElysiumResponseButton>();
        if (button != null && button.response != null && SelectedResponseHandler != null)
        {
            SelectedResponseHandler(this, new SelectedResponseEventArgs(button.response));
        }
    }
}
