using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRover : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dirX;
    [SerializeField] RoverController roverController;
    [SerializeField] public int roverScale = 1;
    [SerializeField] public int RoverId;// { get; set; } 
    [SerializeField] public int ParentId;// { get; set; }

    [SerializeField] private bool isOriginal = false;

    [SerializeField] private bool Conveyored = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        roverController = FindObjectOfType<RoverController>();
        if(isOriginal)
            RoverManager.Instance.RegisterRover(this.gameObject);
        Conveyor.OnRoverStepOnConveyor += RoverOnConveyor;
        Conveyor.OnRoverStepOffConveyor += RoverOffConveyor;
    }
    void Update()
    {
        if(Conveyored) return;
        if(!RoverManager.Instance.isControlled) return;
        var movement = Input.GetAxisRaw("Horizontal");
        
        if (movement < 0)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (movement > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        
        dirX = movement * moveSpeed;
    }

    void RoverOnConveyor(int incomingID, float newSpeed)
    {
        if(incomingID == RoverId)
        {
            Conveyored = true;
            dirX = newSpeed;
        }
    }

    void RoverOffConveyor(int incomingID)
    {
        if(incomingID == RoverId)
        {
            Conveyored = false;
            dirX = moveSpeed;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }

    public void KillRover()
    {
        if(!GameObject.Find("Rover clone"))
            roverController.RespawnRover();
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        RoverManager.Instance.DeregisterRover(RoverId);
    }
}
