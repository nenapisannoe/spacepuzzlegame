using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ActivatableLight : MonoBehaviour
{
    public ActivatorColorType lightColor; 
    private SpriteRenderer spriteRenderer;
    public bool lightActivated = false;

    [SerializeField] ObjectActivator objectActivator;

    public static Action OnLightActivated;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (objectActivator == ObjectActivator.Buttons)
            ColorBasedActivatorObject.OnActivatorButtonPressed += ActivateLight;
        else
            Field.OnWiresSolved += ActivateLight;
    }

    void ActivateLight(ActivatorColorType buttonColor)
    {
        Debug.Log("hi");
        if (buttonColor == lightColor)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
            lightActivated = true;
            OnLightActivated?.Invoke();
        }
    }
}
