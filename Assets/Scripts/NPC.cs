using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string ID;
    public DialogueData dialogueData;
    private bool playerNearby;



    public virtual void Interact()
    {
        if (dialogueData != null)
        {
            QuestEvents.OnNPCInteracted?.Invoke(ID);
            DialogueUI.Instance.ShowDialogue(dialogueData);
        }
    }

    private void EndDialogue()
    {
        DialogueUI.Instance.HideDialoguePanel();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    void Update()
    {
        if(playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}
