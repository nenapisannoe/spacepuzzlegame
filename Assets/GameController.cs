using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    public bool SideWalkRuleOn = true;

    private void Start()
    {
        instance = this;
    }
}
