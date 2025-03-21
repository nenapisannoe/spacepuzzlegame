using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjective {
    public ObjectiveType Type { get; set; }
    public string TargetID { get; set; }  
    public bool IsCompleted { get; set; }
    public string Description { get; set; }

    public QuestObjective(ObjectiveType type, string targetID, string description) {
        Type = type;
        TargetID = targetID;
        IsCompleted = false;
        Description = description;
    }

    public void CompleteObjective() {
        IsCompleted = true;
    }
}

