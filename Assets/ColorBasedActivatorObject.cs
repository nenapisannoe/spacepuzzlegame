using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivatorColorType { Green, Magenta, Blue, Multiple }
public class ActivatableObject : MonoBehaviour
{
    public ActivatorColorType objectColor; 
}


public class ColorBasedActivatorObject : MonoBehaviour
{
    public static Action<ActivatorColorType> OnActivatorButtonPressed;
    public ActivatorColorType buttonColor; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Rover"))
        {
            OnActivatorButtonPressed?.Invoke(buttonColor);
            Debug.Log("Object activated!");
        }
    }
}
