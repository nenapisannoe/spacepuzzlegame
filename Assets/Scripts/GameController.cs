using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public bool SideWalkRuleOn = true;
    
    public InputManager InputManager { get; private set; }
    
    public event Action<bool> OnResumeOpen;
    public event Action<bool> OnMapOpen;
    
    private bool TimeIsPaused() => Math.Abs(Time.timeScale) < float.Epsilon;

    protected void Awake()
    {
        instance = this;
        InputManager = gameObject.AddComponent<InputManager>();
        InputManager.OpenResume += ViewResume;
        InputManager.OpenMap += ViewMap;
    }

    private void Start()
    {

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
}
