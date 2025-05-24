using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResumeUI : MonoBehaviour
{

    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questDesc;
    [SerializeField] TMP_Text questStep;

    [SerializeField] TMP_Text questDescResume;
    [SerializeField] TMP_Text questStepResume;

    [SerializeField] TMP_Text workersRelationField;
    [SerializeField] TMP_Text smugglersRelationField;
    [SerializeField] TMP_Text securityRelationField;

    [SerializeField] GameObject Resume;
    [SerializeField] GameObject mapPrefab;

    [SerializeField] GameObject help;
    [SerializeField] Button openHelpButton;
    [SerializeField] Button closeHelpButton;

    static ResumeUI instance;

    void OpenHelp()
    {
        help.SetActive(true);
        openHelpButton.gameObject.SetActive(false);
    }
    void CloseHelp()
    {
        help.SetActive(false);
        openHelpButton.gameObject.SetActive(true);
    }
    void OnEnable()
    {
        FactionManager.onRelationshipChanged += UpdateRelations;
        GameController.instance.OnResumeOpen += Switch;
        QuestEvents.OnQuestNameUpdated += SetQuestName;
        QuestEvents.OnQuestDescriptionUpdated += SetQuestDescription;
        QuestEvents.OnQuestStepUpdated += SetQuestStep;
        openHelpButton.onClick.AddListener(OpenHelp);
        closeHelpButton.onClick.AddListener(CloseHelp);

    }

    void OnDisable()
    {
        FactionManager.onRelationshipChanged -= UpdateRelations;
        GameController.instance.OnResumeOpen -= Switch;
        QuestEvents.OnQuestDescriptionUpdated -= SetQuestDescription;
        QuestEvents.OnQuestStepUpdated -= SetQuestStep;

        openHelpButton.onClick.RemoveAllListeners();
        closeHelpButton.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Resume instance created: " + gameObject.name);
        }
        else if (instance != this)
        {
            Debug.Log("Duplicate Resume instance destroyed: " + gameObject.name);
            Destroy(gameObject);
        }


        if (FindObjectOfType<MapUI>() == null)
        {
            Instantiate(mapPrefab, this.transform);
        }

        Switch(false);
    }

    void UpdateRelations(FactionType type, int newAmount)
    {
        if (type == FactionType.Workers)
        {
            workersRelationField.text = newAmount.ToString();
        }
        else if (type == FactionType.Smugglers)
        {
            smugglersRelationField.text = newAmount.ToString();
        }
        else
        {
            securityRelationField.text = newAmount.ToString();
        }
    }

    private void Switch(bool val)
    {
        Resume.SetActive(val);
    }

    void SetQuestName(string _questName)
    {
        questName.text = _questName;
    }

    void SetQuestDescription(string _questDesc)
    {
        questDesc.text = _questDesc;
    }

    void SetQuestStep(string _questStep)
    {
        questStep.text = _questStep;
    }

    public void ExitGame()
    {
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }
}
