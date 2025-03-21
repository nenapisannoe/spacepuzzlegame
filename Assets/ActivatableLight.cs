using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ActivatableLight : MonoBehaviour
{
    public ButtonColorType lightColor; 
    private SpriteRenderer spriteRenderer;
    public bool lightActivated = false;

    public static Action OnLightActivated;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ColorBasedActivatorObject.OnActivatorButtonPressed += ActivateLight;
    }

    void ActivateLight(ButtonColorType buttonColor)
    {
        if(buttonColor == lightColor)
        {
           Color color = spriteRenderer.color;
           color.a = 1f;
           spriteRenderer.color = color;
           lightActivated = true;
           OnLightActivated?.Invoke();
        }
    }
}
