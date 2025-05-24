using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/QuestGiverDialogueData")]
public class QuestGiverDialogueData : DialogueData
{
    [TextArea(3,5)] public string[] givingQuestLines;

    [TextArea(3,5)] public string[] WaitingForQuestLines;
}
