using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus {
    Inactive,
    Active,
    Completed
}

public enum ObjectiveType {
    InteractWithObject,
    InteractWithNPC,
    CompletePuzzle
}

public class QuestManager : MonoBehaviour {
    public List<Quest> activeQuests = new List<Quest>();

    public string QuestName = "";
    public string QuestDescription = "";
    public string QuestStep = "";

    public static QuestManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("QuestManager instance created: " + gameObject.name);
        } else if (instance != this) {
            Debug.Log("Duplicate QuestManager instance destroyed: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        QuestEvents.OnObjectInteracted += HandleObjectInteraction;
        QuestEvents.OnNPCInteracted += HandleNPCInteraction;
        QuestEvents.OnPuzzleCompleted += HandlePuzzleCompletion;
    }



    private void OnDisable() {
        QuestEvents.OnObjectInteracted -= HandleObjectInteraction;
        QuestEvents.OnNPCInteracted -= HandleNPCInteraction;
        QuestEvents.OnPuzzleCompleted -= HandlePuzzleCompletion;
    }

    public void AddQuest(Quest quest) {
        activeQuests.Add(quest);
        quest.ActivateQuest();

        QuestEvents.OnQuestNameUpdated?.Invoke(quest.Title);
        QuestEvents.OnQuestDescriptionUpdated?.Invoke(quest.Description);
        QuestEvents.OnQuestStepUpdated?.Invoke(quest.Objectives[0].Description);
    }

    private void HandleObjectInteraction(string targetID) {
        UpdateQuestProgress(ObjectiveType.InteractWithObject, targetID);
    }

    private void HandleNPCInteraction(string npcID) {
        UpdateQuestProgress(ObjectiveType.InteractWithNPC, npcID);
    }

    private void HandlePuzzleCompletion(string puzzleID) {
        UpdateQuestProgress(ObjectiveType.CompletePuzzle, puzzleID);
    }

    private void UpdateQuestProgress(ObjectiveType type, string targetID) {
        foreach (var quest in activeQuests) {
            if (quest.Status == QuestStatus.Active) {
                quest.CompleteObjective(targetID);
            }
        }
    }

    void Start() {

    QuestManager questManager = FindObjectOfType<QuestManager>();

    Quest quest1 = new Quest(
        questID: "quest001",
        title: "Сядь в корабль",
        description: "Сядь в корабль и лети отсюда.",
        objectives: new List<QuestObjective> {
            new QuestObjective(ObjectiveType.InteractWithObject, "Ship", "Нужно пойти и сесть в корабль"),
            new QuestObjective(ObjectiveType.InteractWithObject, "Guy", "Не взлетело. Надо пойти обсудить с человеком."),
            new QuestObjective(ObjectiveType.CompletePuzzle, "FirstPuzzle", "Надо влезть."),
            new QuestObjective(ObjectiveType.InteractWithObject, "Guy", "Че теперь расскажет."),
        },
        rewardPoints: 100
    );

        Quest quest2 = new Quest(
        questID: "quest001",
        title: "Сядь в корабль",
        description: "Сядь в корабль и лети отсюда.",
        objectives: new List<QuestObjective> {
            new QuestObjective(ObjectiveType.InteractWithObject, "SB001", "Нужно пойти перетереть с СБ-шником"),
            new QuestObjective(ObjectiveType.InteractWithObject, "Ship", "Ну пришло время лететь искать че он там от меня хочет"),
            new QuestObjective(ObjectiveType.CompletePuzzle, "SB001", "Ну я принес ему, посмотрим что теперь скажет"),
        },
        rewardPoints: 100
    );

    questManager.AddQuest(quest1);
    questManager.AddQuest(quest2);
}

}
