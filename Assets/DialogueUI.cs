using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Data.Common;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button acceptButton;
    public Button rejectButton;
    public Button proceedButton;
    public event Action OpenDialogue;
    public event Action CloseDialogue;
    private int currentLineIndex;

    private DialogueData currentDialogue;


    void Awake()
    {
        if(Instance==null)
            Instance = this;
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(DialogueData dialogue, System.Action onAccept = null, System.Action onReject = null)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        if (dialoguePanel == null)
            dialoguePanel = transform.Find("DialoguePanel").gameObject;

        OpenDialogue?.Invoke();
        dialoguePanel.SetActive(true);

        if (dialogue.isQuestOffer)
        {
            if (onAccept != null)
                acceptButton.onClick.AddListener(() => { onAccept(); HideDialoguePanel(); });

            if (onReject != null)
                rejectButton.onClick.AddListener(() => { HideDialoguePanel(); });
            
            if(dialogue.dialogueLines.Length > 1)
            {
                proceedButton.onClick.AddListener(ProceedDialogue);
                SwitchButtonsToProceed();
            }
            else
            {
                SwitchButtonsToQuestChoice();
            }
        }
        else
        {
            proceedButton.onClick.AddListener(ProceedDialogue);
            proceedButton.gameObject.SetActive(true);
            acceptButton.gameObject.SetActive(false);
            rejectButton.gameObject.SetActive(false);
        }

        ShowCurrentLine();
    }

    private void SwitchButtonsToProceed()
    {
        proceedButton.gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(false);
        rejectButton.gameObject.SetActive(false);
    }
    private void SwitchButtonsToQuestChoice()
    {
        proceedButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(true);
        rejectButton.gameObject.SetActive(true);
    }

    private void ProceedDialogue()
    {
        currentLineIndex++;

        if (currentLineIndex < currentDialogue.dialogueLines.Length)
        {
            ShowCurrentLine();
            if(currentDialogue.isQuestOffer && currentLineIndex == currentDialogue.dialogueLines.Length-1)
            {
                 SwitchButtonsToQuestChoice();
            }
        }
        else
        {
            HideDialoguePanel();
        }
    }

    private void ShowCurrentLine()
    {
        if (currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Length)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    public void HideDialoguePanel()
    {
        currentLineIndex = 0;
        CloseDialogue?.Invoke();
        dialoguePanel.SetActive(false);
    }
}
