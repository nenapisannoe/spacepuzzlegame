using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public bool SideWalkRuleOn = true;
    
    public InputManager InputManager { get; private set; }
    
    public event Action<bool> OnGamePause;
    
    private bool TimeIsPaused() => Math.Abs(Time.timeScale) < float.Epsilon;

    protected void Awake()
    {
        instance = this;
        InputManager = gameObject.AddComponent<InputManager>();
    }

    private void Start()
    {
        
        InputManager.OpenResume += ViewResume;
    }

    public void ViewResume()
    {
        Time.timeScale = TimeIsPaused() ? 1 : 0;
        OnGamePause?.Invoke(TimeIsPaused());
        
    }
}
