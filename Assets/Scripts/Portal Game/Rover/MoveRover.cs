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
    [SerializeField] public int RoverId;
    [SerializeField] public int ParentId;
    [SerializeField] GameObject pointer;

    public Transform currentFloor; 

    public int halves;

    [SerializeField] private bool isOriginal = false;

    [SerializeField] private bool Conveyored = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            currentFloor = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            if (currentFloor == collision.transform)
                currentFloor = null;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        roverController = FindObjectOfType<RoverController>();
        if(isOriginal)
            RoverManager.Instance.RegisterRover(this.gameObject);
    }

    public void ShowPointer()
    {
        pointer.SetActive(true);
    }

    public void HidePointer()
    {
        pointer.SetActive(false);
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

    public void RoverOnConveyor(float newSpeed)
    {
        Conveyored = true;
        dirX = newSpeed;
    }

    public void RoverOffConveyor()
    {
        Conveyored = false;
        dirX = moveSpeed;
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
        RoverManager.Instance.DeregisterRover(this);
    }
}
