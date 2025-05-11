using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResumeUI : MonoBehaviour
{

    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questDesc;
    [SerializeField] TMP_Text questStep;

    [SerializeField] GameObject Resume;
    [SerializeField] GameObject mapPrefab;

    static ResumeUI instance;

    void OnEnable() 
    {    
           
    }
    private void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("Resume instance created: " + gameObject.name);
        } else if (instance != this) {
            Debug.Log("Duplicate Resume instance destroyed: " + gameObject.name);
            Destroy(gameObject);
        }

        GameController.instance.OnResumeOpen += Switch;
        QuestEvents.OnQuestNameUpdated += SetQuestName;
        QuestEvents.OnQuestDescriptionUpdated += SetQuestDescription;
        //QuestEvents.OnQuestStepUpdated += SetQuestStep;
        
        if (FindObjectOfType<MapUI>() == null)
        {
            Instantiate(mapPrefab, this.transform);
        }   
        
        Switch(false);
    }

    void Start()
    {

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
}
