using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeUI : MonoBehaviour
{
    private void Awake()
    {
        GameController.instance.OnGamePause += Switch;
        
        Switch(false);
    }
    private void Switch(bool val)
    {
        gameObject.SetActive(val);
    }
}
