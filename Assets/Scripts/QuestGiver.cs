using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] public Quest quest;

    public override void Interact()
    {
        if(QuestManager.instance.currentQuest == null)
        {
            DialogueUI.Instance.ShowDialogue(dialogueData, AcceptQuest, RejectQuest);
        }
    }

    void AcceptQuest()
    {
        QuestManager.instance.AcceptQuest(quest);
    }

    void RejectQuest()
    {
        Debug.Log("ok bro");
    }
}
