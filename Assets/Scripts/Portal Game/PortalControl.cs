using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PortalType {left, right};
public class PortalControl : MonoBehaviour
{
    [SerializeField] private GameObject leftPortal;
    [SerializeField] private GameObject rightPortal;
    
    [SerializeField] private Transform leftPortalSpawnPoint;
    [SerializeField] private Transform rightPortalSpawnPoint;
    
    [SerializeField] private Collider2D leftPortalCollider;
    [SerializeField] private Collider2D rightPortalCollider;

    [SerializeField] private GameObject clone;
    
    [SerializeField] private bool isPortalOn;
    [SerializeField] private int portalColor;
    
    
    void Start()
    {
        leftPortalCollider = leftPortal.GetComponent<BoxCollider2D>();
        rightPortalCollider = rightPortal.GetComponent<BoxCollider2D>();
        
        if(isPortalOn)
            EnablePortals();
        else
        {
            DisablePortals();
            Field.OnWireConnected += AcceptPortalEnable;
        }
    }

    void AcceptPortalEnable(int color)
    {
        if(color == portalColor)
            EnablePortals();
    }

    void EnablePortals()
    {
        EnableColliders();
        leftPortal.GetComponent<Portal>().SwitchTransparency(false);
        rightPortal.GetComponent<Portal>().SwitchTransparency(false);
    }

    void DisablePortals()
    {
        DisableColliders();
        leftPortal.GetComponent<Portal>().SwitchTransparency(true);
        rightPortal.GetComponent<Portal>().SwitchTransparency(true);
    }

    public void SpawnClone(PortalType portal)
    {
        if (portal == PortalType.left)
        {
            var cloneInstance = Instantiate(clone, leftPortalSpawnPoint.position, Quaternion.identity);
            cloneInstance.gameObject.tag = "PlayerClone";
        }
        else if (portal == PortalType.right)
        {
            var cloneInstance = Instantiate(clone, rightPortalSpawnPoint.position, Quaternion.identity);
            cloneInstance.gameObject.tag = "PlayerClone";
        }
        
    }
    
    public void DisableCollider(PortalType colliderToDisable)
    {
        if (colliderToDisable == PortalType.left)
        {
            leftPortalCollider.enabled = false;
        }
        else if (colliderToDisable == PortalType.right)
        {
            rightPortalCollider.enabled = false;
        }
    }

    public void DisableColliders()
    {
        DisableCollider(PortalType.left);
        DisableCollider(PortalType.right);
    }

    public void EnableColliders()
    {
        leftPortalCollider.enabled = true;
        rightPortalCollider.enabled = true;
    }
}
