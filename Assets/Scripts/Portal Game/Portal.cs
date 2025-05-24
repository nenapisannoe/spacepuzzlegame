using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Rigidbody2D enteredRigidbody;
    private float enterVelocity;
    private float exitVelocity;

    [SerializeField] private SpriteRenderer rightHalf;
    [SerializeField] private SpriteRenderer leftHalf;

    [SerializeField] private SpriteRenderer curtain;

    [SerializeField] PortalControl _portalControl;


    public void SwitchTransparency(bool isTransparent)
    {
        if (isTransparent)
        {
            Color newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 0.5f);
            leftHalf.color = newColor;
            rightHalf.color = newColor;
            curtain.sortingOrder = 1;
        }
        else
        {
            Color newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 1f);
            leftHalf.color = newColor;
            rightHalf.color = newColor;
            curtain.sortingOrder = 6;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Rover"))
        {
            RoverManager.Instance.KillRovers();
        }
        if (!col.CompareTag("Player") || GameObject.FindGameObjectWithTag("PlayerClone"))
            return;
        enteredRigidbody = col.gameObject.GetComponent<Rigidbody2D>();
        //enterVelocity = enteredRigidbody.velocity.x;

        if (gameObject.CompareTag("LeftPortal"))
        {
            _portalControl.DisableCollider(PortalType.right);
            _portalControl.SpawnClone(PortalType.right);
        }
        else if (gameObject.CompareTag("RightPortal"))
        {
            _portalControl.DisableCollider(PortalType.left);
            _portalControl.SpawnClone(PortalType.left);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;
        //exitVelocity = enteredRigidbody.velocity.x;

        if (enterVelocity != exitVelocity)
        {
            Destroy(GameObject.FindGameObjectWithTag("PlayerClone"));
        }
        else if (gameObject.name != "clone")
        {
            Destroy(col.gameObject);
            _portalControl.EnableColliders();
            GameObject.FindGameObjectWithTag("PlayerClone").name = "Player";
            GameObject.FindGameObjectWithTag("PlayerClone").tag = "Player";
        }
    }
}
