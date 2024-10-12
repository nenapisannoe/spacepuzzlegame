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

    [SerializeField] PortalControl _portalControl;


    public void SwitchTransparency(bool isTransparent)
    {
        if (isTransparent)
        {
            Color newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 0.5f);
            leftHalf.color = newColor;
            rightHalf.color = newColor;
        }
        else
        {
            Color newColor = new Color(rightHalf.color.r, rightHalf.color.g, rightHalf.color.b, 1f);
            leftHalf.color = newColor;
            rightHalf.color = newColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Rover"))
        {
            col.GetComponent<MoveRover>().KillRover();
            return;
        }
        if (!col.CompareTag("Player") || GameObject.Find("clone"))
            return;
        enteredRigidbody = col.gameObject.GetComponent<Rigidbody2D>();
        enterVelocity = enteredRigidbody.velocity.x;

        if (gameObject.name == "LeftPortal")
        {
            _portalControl.DisableCollider("right");
            _portalControl.SpawnClone("right");
             FindObjectOfType<PlayerState>().direction = PlayerState.DirectionEnum.LEFT;
        }
        else if (gameObject.name == "RightPortal")
        {
            _portalControl.DisableCollider("left");
            _portalControl.SpawnClone("left");
            FindObjectOfType<PlayerState>().direction =  PlayerState.DirectionEnum.RIGHT;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;
        exitVelocity = enteredRigidbody.velocity.x;

        if (enterVelocity != exitVelocity)
        {
            Destroy(GameObject.Find("Clone"));
        }
        else if (gameObject.name != "clone")
        {
            Destroy(col.gameObject);
            _portalControl.EnableColliders();
            GameObject.Find("clone").name = "Player";
        }
    }
}
