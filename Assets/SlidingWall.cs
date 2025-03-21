using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class SlidingWall : MonoBehaviour
{
    [SerializeField] int ActivatableObjectID;
    public enum WallActivator { Field, Buttons }
    public float shrinkDuration = 1f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float positionOffset = 4f;
    [SerializeField] public WallActivator wallActivator;
    
    bool wallDown = false;
    private float elapsedTime = 0f;

    [SerializeField] private List<ActivatableLight> lights = new List<ActivatableLight>();

    /*List<Transform> FindChildrenWithTag(GameObject parent, string tag)
    {
        Transform parentTransform = parent.GetComponent<Transform>();
        List<Transform> childrenWithTag = new List<Transform>();
        foreach (Transform child in parentTransform)
        {
            if (child.CompareTag(tag))
            {
                childrenWithTag.Add(child);
            }
        }
        if(childrenWithTag.Count == 0)
            return null;
        else
            return childrenWithTag;
    }*/
    void Start()
    {
        originalScale = transform.localScale;
        targetScale = new Vector3(0.1f, originalScale.y, originalScale.z);
        
        originalPosition = transform.localPosition;
        targetPosition = new Vector3(originalPosition.x, originalPosition.y - positionOffset, originalPosition.z);

        //lights = FindChildrenWithTag(gameObject, "Light");
        if(wallActivator == WallActivator.Field)
            Field.OnWiresSolved += ActivateWall;
        else if(wallActivator == WallActivator.Buttons)
            ActivatableLight.OnLightActivated += CheckLights;
            
    }

    void CheckLights()
    {
        bool allLightsActivated = true;
        foreach(var light in lights)
        {
            if(!light.lightActivated)
                allLightsActivated = false;
        }
        if(allLightsActivated)
            SlideWall();
    }
    void ActivateWall(int fieldID)
    {
        if(fieldID == ActivatableObjectID)
            SlideWall();
    }

    void SlideWall()
    {
        if(!wallDown)
            StartCoroutine(SlidingAnimation());
    }

    IEnumerator SlidingAnimation()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < shrinkDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / shrinkDuration;
            
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            transform.localPosition = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }
        wallDown = true;
  
    }
    
}
