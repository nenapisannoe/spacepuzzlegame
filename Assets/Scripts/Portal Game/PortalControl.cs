using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControl : MonoBehaviour
{
    [SerializeField] private GameObject leftPortal;
    [SerializeField] private GameObject rightPortal;
    
    [SerializeField] private Transform leftPortalSpawnPoint;
    [SerializeField] private Transform rightPortalSpawnPoint;
    
    [SerializeField] private Collider2D leftPortalCollider;
    [SerializeField] private Collider2D rightPortalCollider;

    [SerializeField] private GameObject clone;
    
    
    void Start()
    {
        leftPortalCollider = leftPortal.GetComponent<BoxCollider2D>();
        rightPortalCollider = rightPortal.GetComponent<BoxCollider2D>();
    }

    public void SpawnClone(string portal)
    {
        if (portal == "left")
        {
            var cloneInstance = Instantiate(clone, leftPortalSpawnPoint.position, Quaternion.identity);
            cloneInstance.gameObject.name = "clone";
        }
        else if (portal == "right")
        {
            var cloneInstance = Instantiate(clone, rightPortalSpawnPoint.position, Quaternion.identity);
            cloneInstance.gameObject.name = "clone";
        }
        
    }

    public void DisableCollider(string colliderToDisable)
    {
        if (colliderToDisable == "left")
        {
            leftPortalCollider.enabled = false;
        }
        else if (colliderToDisable == "right")
        {
            rightPortalCollider.enabled = false;
        }
    }

    public void EnableColliders()
    {
        leftPortalCollider.enabled = true;
        rightPortalCollider.enabled = true;
    }
}
