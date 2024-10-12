using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingWall : MonoBehaviour
{

    public float shrinkDuration = 1f;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Vector3 originalPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float positionOffset = 4f;
    
    private float elapsedTime = 0f;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = new Vector3(0.1f, originalScale.y, originalScale.z);
        
        originalPosition = transform.localPosition;
        targetPosition = new Vector3(originalPosition.x, originalPosition.y - positionOffset, originalPosition.z);

        Field.OnWiresSolved += SlideWall;
    }

    void SlideWall()
    {
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
  
    }
    
}
