using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectActivator { Field, Buttons }
public class ColorBasedActivatableObject : MonoBehaviour
{

    [SerializeField] public ObjectActivator objectActivator;
    [SerializeField] public ActivatorColorType ActivatableObjectColor;
    [SerializeField] public List<ActivatableLight> lights = new List<ActivatableLight>();
}
