using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    public event Action OpenResume;
    public event Action OpenMap;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenResume?.Invoke();
        if (Input.GetKeyDown(KeyCode.M))
            OpenMap?.Invoke();
        
    }
}
