using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBasedActivatableObject : MonoBehaviour
{
    public enum ObjectActivator { Field, Buttons }
    [SerializeField] public ObjectActivator objectActivator;
    [SerializeField] public ButtonColorType ActivatableObjectColor;
    [SerializeField] public List<ActivatableLight> lights = new List<ActivatableLight>();
}
