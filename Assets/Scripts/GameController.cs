using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public InputManager InputManager { get; private set; }
    
    public event Action<bool> OnResumeOpen;
    public event Action<bool> OnMapOpen;
    [SerializeField] GameObject pointer;

    public List<int> PuzzlezAvailable = new List<int>();
    
    private bool TimeIsPaused() => Math.Abs(Time.timeScale) < float.Epsilon;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InputManager = gameObject.AddComponent<InputManager>();
        InputManager.OpenResume += ViewResume;
        InputManager.OpenMap += ViewMap;
        DialogueUI.Instance.OpenDialogue += PauseTime;
        DialogueUI.Instance.CloseDialogue += ResumeTime;
        PuzzlezAvailable = SaveManager.Instance.saveData.unlockedPuzzles;
    }

    public void ViewResume()
    {
        Time.timeScale = TimeIsPaused() ? 1 : 0;
        OnResumeOpen?.Invoke(TimeIsPaused());
    }

    public void ViewMap()
    {
        Time.timeScale = TimeIsPaused() ? 1 : 0;
        OnMapOpen?.Invoke(TimeIsPaused());
    }

    void PauseTime()
    {
        Time.timeScale = 0;
    }

    void ResumeTime()
    {
        Time.timeScale = 1;
    }
}
